using AIAssistants.Interfaces;
using AIAssistants.JSON;
using AIChessDatabase.Properties;
using GlobalCommonEntities.DependencyInjection;
using GlobalCommonEntities.Interfaces;
using GlobalCommonEntities.UI;
using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static AIChessDatabase.Properties.UIResources;

namespace AIChessDatabase.AI
{
    /// <summary>
    /// Configuration to update assistants and their documents
    /// </summary>
    public class UpdateAssistantsConfiguration
    {
        private string _dataPath = string.Empty;
        [JsonPropertyName("clean_expressions")]
        [DILocalizedDisplayName(nameof(NAME_UpdateAssistantsConfiguration_CleanExpressions), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_UpdateAssistantsConfiguration_CleanExpressions), typeof(UIResources))]
        public List<string> CleanExpressions { get; set; }
        [JsonPropertyName("assistants")]
        [DILocalizedDisplayName(nameof(NAME_UpdateAssistantsConfiguration_Assistants), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_UpdateAssistantsConfiguration_Assistants), typeof(UIResources))]
        public List<AssistantUpdates> AssistantUpdates { get; set; } = new List<AssistantUpdates>();
        [JsonPropertyName("services")]
        [DILocalizedDisplayName(nameof(NAME_UpdateAssistantsConfiguration_Services), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_UpdateAssistantsConfiguration_Services), typeof(UIResources))]
        public List<ServiceUpdates> ServiceUpdates { get; set; } = new List<ServiceUpdates>();
        [JsonIgnore]
        [Browsable(false)]
        public int UpdateCount
        {
            get
            {
                int count = 0;
                if (AssistantUpdates != null)
                {
                    foreach (AssistantUpdates au in AssistantUpdates)
                    {
                        if (au.Documents != null)
                        {
                            count += au.Documents.Count(d => (d.Update || d.Remove) && File.Exists(Path.Combine(DataPath, d.FileName)));
                            count += 2 * au.Documents.Count(d => d.Update && File.Exists(Path.Combine(DataPath, d.FileName)));
                        }
                        if (au.UpdateInstructions)
                        {
                            count++;
                        }
                    }
                }
                if (ServiceUpdates != null)
                {
                    foreach (ServiceUpdates su in ServiceUpdates)
                    {
                        if (su.Documents != null)
                        {
                            count += su.Documents.Count(d => (d.Update || d.Remove) && File.Exists(Path.Combine(DataPath, d.FileName)));
                            count += 2 * su.Documents.Count(d => d.Update && File.Exists(Path.Combine(DataPath, d.FileName)));
                            foreach (AssistantUpdates au in AssistantUpdates)
                            {
                                List<string> srv = ServicesByPlayerID(au.Identifier);
                                if (srv != null && srv.Contains(su.ServiceId, StringComparer.OrdinalIgnoreCase))
                                {
                                    // If the service is allowed by the assistant, count the documents to update
                                    count += su.Documents.Count(d => (d.Update || d.Remove) && File.Exists(Path.Combine(DataPath, d.FileName)));
                                }
                            }
                        }
                    }
                }
                return count;
            }
        }
        /// <summary>
        /// Application automation object.
        /// </summary>
        [JsonIgnore]
        [Browsable(false)]
        public IAppAutomation AppAutomation { get; set; }
        /// <summary>
        /// Application documentation files path.
        /// </summary>
        [JsonIgnore]
        [Browsable(false)]
        public string DataPath
        {
            get
            {
                return _dataPath;
            }
            set
            {
                _dataPath = value;
                if (AssistantUpdates != null)
                {
                    foreach (AssistantUpdates au in AssistantUpdates)
                    {
                        au.DataPath = value;
                    }
                }
            }
        }
        /// <summary>
        /// Progress monitor object
        /// </summary>
        [JsonIgnore]
        [Browsable(false)]
        public IProgressMonitor ProgressMonitor { get; set; }
        /// <summary>
        /// Remove old session working files from assistant stores
        /// </summary>
        public async Task CleanStorage()
        {
            if (CleanExpressions == null || CleanExpressions.Count == 0)
            {
                return;
            }
            foreach (AssistantUpdates au in AssistantUpdates)
            {
                IAPIPlayer player = await AppAutomation.GetPlayerByID(au.Identifier);
                IFilePackageManager fmp = player.Assets?.FirstOrDefault(a => a is IFilePackageManager) as IFilePackageManager;
                if (fmp != null)
                {
                    IDocumentStoreManager dsmn = fmp as IDocumentStoreManager;
                    List<ObjectWrapper> files = await fmp.ListPackageFiles(fmp.Identifier);
                    if (files != null)
                    {
                        foreach (string s in CleanExpressions)
                        {
                            Regex reg = new Regex(s, RegexOptions.IgnoreCase);
                            foreach (ObjectWrapper file in files)
                            {
                                if (reg.IsMatch(await fmp.FileManager.GetFileNameById(file.UID)))
                                {
                                    try
                                    {
                                        await dsmn.RemoveDocument(file.UID, false);
                                    }
                                    catch { }
                                }
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Performa the updates
        /// </summary>
        /// <returns>
        /// Error list. Empty list if no errors.
        /// </returns>
        public async Task<List<string>> PerformUpdates()
        {
            List<string> errors = new List<string>();
            try
            {
                await Task.Run(async () =>
                {
                    Dictionary<string, IAPIPlayer> players = new Dictionary<string, IAPIPlayer>();
                    ProgressMonitor?.Reset(this);
                    ProgressMonitor?.SetTotalSteps(UpdateCount);
                    Dictionary<string, WorkDocument> updatelist = new Dictionary<string, WorkDocument>();
                    List<WorkDocument> docstoupdate = new List<WorkDocument>();
                    List<WorkDocument> newdocuments = new List<WorkDocument>();
                    foreach (AssistantUpdates au in AssistantUpdates)
                    {
                        IAPIPlayer player = players.ContainsKey(au.Identifier) ? players[au.Identifier] : await AppAutomation.GetPlayerByID(au.Identifier);
                        players[au.Identifier] = player;
                        // Get asssitant document store
                        IDocumentAnalyzer pdoc = player as IDocumentAnalyzer;
                        IDocumentStoreManager dsmn = await pdoc?.GetDocumentStoreManager();
                        IFileManager filemanager = dsmn?.FileManager ?? player.APIManager.CreateElement(nameof(IFileManager), null) as IFileManager;
                        // Update assistant instructions.
                        // Instructions for IAssistant objects are stored in the provider cloud, so them need to be uploaded via API.
                        if (!string.IsNullOrEmpty(au.Instructions) && au.UpdateInstructions)
                        {
                            if (player is ITaskAgent)
                            {
                                string ipath = Path.Combine(DataPath, au.Instructions);
                                if (File.Exists(ipath))
                                {
                                    ITaskAgent assistant = player as ITaskAgent;
                                    au.UpdateInstructions = !await assistant.ChangeInstructions(File.ReadAllText(ipath));
                                    ProgressMonitor?.Step();
                                }
                            }
                            else
                            {
                                au.UpdateInstructions = false; // Only ITaskAgent can update instructions
                                ProgressMonitor?.Step();
                            }
                        }
                        // Remove old document versions first from assistant store
                        if (au.Documents != null)
                        {
                            try
                            {
                                foreach (UpdateDocument doc in au.Documents)
                                {
                                    if (doc.Update || doc.Remove)
                                    {
                                        // Create a work document to manage the update
                                        WorkDocument wdoc = new WorkDocument
                                        {
                                            FileName = Path.Combine(DataPath, doc.FileName),
                                            FileManager = filemanager,
                                            ProviderId = player.APIManager.AccountId,
                                            FileId = await filemanager.GetFileIdByName(doc.FileName)
                                        };
                                        if (!string.IsNullOrEmpty(wdoc.FileId))
                                        {
                                            if (!doc.Remove && !docstoupdate.Contains(wdoc) && !newdocuments.Contains(wdoc))
                                            {
                                                // Add the document to the update dictionary
                                                updatelist.Add(Guid.NewGuid().ToString("N"), wdoc);
                                                docstoupdate.Add(wdoc);
                                            }
                                            // Remove the document from the assistant store if it exists
                                            await dsmn?.RemoveDocument(wdoc.FileId, true);
                                            // Wait to avoid errors with the store and file manager
                                            await Task.Delay(1000);
                                            ProgressMonitor?.Step();
                                        }
                                        else if (File.Exists(Path.Combine(DataPath, doc.FileName)) && !doc.Remove && !newdocuments.Contains(wdoc))
                                        {
                                            // This is a new document, upload it
                                            ObjectWrapper ow = await filemanager.UploadFile(Path.Combine(DataPath, doc.FileName));
                                            wdoc.FileId = ow.UID;
                                            newdocuments.Add(wdoc);
                                            ProgressMonitor?.Step();
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                errors.Add($"{au.Name}.Documents: {ex.Message}");
                            }
                        }
                        // Process assistant service documents
                        if (player.AllowedServices != null)
                        {
                            foreach (string srv in player.AllowedServices)
                            {
                                ServiceUpdates su = ServiceUpdates.FirstOrDefault(s => s.ServiceId.Equals(srv, StringComparison.OrdinalIgnoreCase));
                                if (su?.Documents != null)
                                {
                                    try
                                    {
                                        foreach (UpdateDocument doc in su.Documents)
                                        {
                                            if (doc.Update || doc.Remove)
                                            {
                                                // Create a work document to manage the update
                                                WorkDocument wdoc = new WorkDocument
                                                {
                                                    FileName = doc.FileName,
                                                    FileManager = filemanager,
                                                    ProviderId = player.APIManager.AccountId,
                                                    FileId = await filemanager.GetFileIdByName(doc.FileName)
                                                };
                                                if (!doc.Remove && !string.IsNullOrEmpty(wdoc.FileId))
                                                {
                                                    if (!docstoupdate.Contains(wdoc) && !newdocuments.Contains(wdoc))
                                                    {
                                                        // Add the document to the update dictionary
                                                        updatelist.Add(Guid.NewGuid().ToString("N"), wdoc);
                                                        docstoupdate.Add(wdoc);
                                                    }
                                                    // Remove the document from the assistant store if it exists
                                                    await dsmn?.RemoveDocument(wdoc.FileId, true);
                                                    // Wait to avoid errors with the store and file manager
                                                    await Task.Delay(1000);
                                                    ProgressMonitor?.Step();
                                                }
                                                else if (File.Exists(Path.Combine(DataPath, doc.FileName)) && !doc.Remove && !newdocuments.Contains(wdoc))
                                                {
                                                    // This is a new document, upload it
                                                    ObjectWrapper ow = await filemanager.UploadFile(Path.Combine(DataPath, doc.FileName));
                                                    wdoc.FileId = ow.UID;
                                                    newdocuments.Add(wdoc);
                                                    ProgressMonitor?.Step();
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        errors.Add($"{su.ServiceId}.Documents: {ex.Message}");
                                    }
                                }
                            }
                        }
                    }
                    // Now, all documents to update are in the IFileManager store and removed from the assistants store. Remove them from the file manager store
                    foreach (string id in updatelist.Keys)
                    {
                        // Delete the file from the file manager store if it exists
                        try { await updatelist[id].FileManager.DeleteFile(updatelist[id].FileId, null); } catch (Exception ex) { errors.Add($"{updatelist[id].FileName}.Update: {ex.Message}"); }
                        await Task.Delay(1000);
                        if (File.Exists(Path.Combine(DataPath, updatelist[id].FileName)))
                        {
                            // Upload the file new version to the file manager store
                            ObjectWrapper ow = await updatelist[id].FileManager.UploadFile(Path.Combine(DataPath, updatelist[id].FileName));
                            // Renew the file id in the update list
                            updatelist[id].FileId = ow.UID;
                            ProgressMonitor?.Step();
                        }
                    }
                    // Now, all the documents to update are in the file manager store.
                    newdocuments.AddRange(updatelist.Values);
                    foreach (AssistantUpdates au in AssistantUpdates)
                    {
                        IAPIPlayer player = players.ContainsKey(au.Identifier) ? players[au.Identifier] : await AppAutomation.GetPlayerByID(au.Identifier);
                        players[au.Identifier] = player;
                        // Get asssitant document store
                        IDocumentAnalyzer pdoc = player as IDocumentAnalyzer;
                        IDocumentStoreManager dsmn = await pdoc?.GetDocumentStoreManager();
                        // Update assistant store documents from file manager store
                        if (au.Documents != null)
                        {
                            try
                            {
                                foreach (UpdateDocument doc in au.Documents)
                                {
                                    if (doc.Update)
                                    {
                                        // Find the corresponding work document in the new documents list
                                        WorkDocument wdoc = newdocuments.FirstOrDefault(w =>
                                            w.FileName.Equals(doc.FileName, StringComparison.OrdinalIgnoreCase) &&
                                            w.ProviderId == player.APIManager.AccountId);
                                        if (wdoc != null)
                                        {
                                            // Add the document to the assistant store
                                            await dsmn?.StoreFromFileManager(wdoc.FileId);
                                            // Security delay to avoid update errors
                                            await Task.Delay(1000);
                                            doc.Update = false; // Mark the document as updated
                                            ProgressMonitor?.Step();
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                errors.Add($"{au.Name}.Updates: {ex.Message}");
                            }
                        }
                        // Process assistant service documents
                        if (player.AllowedServices != null)
                        {
                            foreach (string srv in player.AllowedServices)
                            {
                                ServiceUpdates su = ServiceUpdates.FirstOrDefault(s => s.ServiceId.Equals(srv, StringComparison.OrdinalIgnoreCase));
                                if (su?.Documents != null)
                                {
                                    try
                                    {
                                        foreach (UpdateDocument doc in su.Documents)
                                        {
                                            if (doc.Update)
                                            {
                                                // Find the corresponding work document in the new documents list
                                                WorkDocument wdoc = newdocuments.FirstOrDefault(w =>
                                                    w.FileName.Equals(doc.FileName, StringComparison.OrdinalIgnoreCase) &&
                                                    w.ProviderId == player.APIManager.AccountId);
                                                if (wdoc != null)
                                                {
                                                    // Add the document to the assistant store
                                                    await dsmn?.StoreFromFileManager(wdoc.FileId);
                                                    // Security delay to avoid update errors
                                                    await Task.Delay(1000);
                                                    ProgressMonitor?.Step();
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        errors.Add($"{su.ServiceId}.Updates: {ex.Message}");
                                    }
                                }
                            }
                        }
                    }
                    // Mark all the service documents as updated
                    foreach (ServiceUpdates su in ServiceUpdates)
                    {
                        if (su.Documents != null)
                        {
                            foreach (UpdateDocument doc in su.Documents)
                            {
                                doc.Update = false;
                            }
                        }
                    }
                    ProgressMonitor?.Stop(this);
                });
                return errors;
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                return errors;
            }
        }
        /// <summary>
        /// Get a list of services allowed for a player
        /// </summary>
        /// <param name="id">
        /// Player identifier
        /// </param>
        /// <returns>
        /// List or services or null if player not found or has no services
        /// </returns>
        private List<string> ServicesByPlayerID(string id)
        {
            foreach (PlaySchema play in AppAutomation.Plays.Plays)
            {
                PlayPlayer player = play.Players.FirstOrDefault(p => p.Id == id);
                if (player != null)
                {
                    return player.Services;
                }
            }
            return null;
        }
    }
    /// <summary>
    /// Assistant updates configuration
    /// </summary>
    public class AssistantUpdates : UIDataSheet, IStandardObject, IEquatable<AssistantUpdates>
    {
        private List<PropertyEditorInfo> _info;
        private string _instructions = string.Empty;
        private string _datapath;
        [Browsable(false)]
        [JsonIgnore]
        public override List<PropertyEditorInfo> Properties
        {
            get
            {
                if (_info == null)
                {
                    _info = new List<PropertyEditorInfo>
                    {
                        new PropertyEditorInfo() { EditorType = InputEditorType.BlockTitle, PropertyName = BTTL_APIPlayer },
                        new PropertyEditorInfo() { EditorType = InputEditorType.SingleLineText, PropertyName = nameof(Name), ReadOnly = true },
                        new PropertyEditorInfo() { EditorType = InputEditorType.FileName, PropertyName = nameof(Instructions), InitialValue = Instructions, Values = new List<object> { FILTER_Text, FILTER_AllFiles } },
                        new PropertyEditorInfo() { EditorType = InputEditorType.BoolValue, PropertyName = nameof(UpdateInstructions) },
                    };
                }
                return _info;
            }
            set
            {
                _info = value;
            }
        }
        /// <summary>
        /// IStandardObject: Unique identifier
        /// </summary>
        [JsonIgnore]
        [Browsable(false)]
        public string StdUID { get; }
        /// <summary>
        /// IStandardObject: Element name
        /// </summary>
        [JsonIgnore]
        [Browsable(false)]
        public string StdName { get { return Name; } }
        /// <summary>
        /// IStandardObject: Element description
        /// </summary>
        [JsonIgnore]
        [Browsable(false)]
        public string StdDescription { get; }
        /// <summary>
        /// IStandardObject: Information to edit the object
        /// </summary>
        [JsonIgnore]
        [Browsable(false)]
        public IUIDataSheet DataSheet { get { return this; } }
        [JsonIgnore]
        [Browsable(false)]
        public string DataPath
        {
            get
            {
                return _datapath;
            }
            set
            {
                _datapath = value;
                if (Documents != null)
                {
                    foreach (UpdateDocument doc in Documents)
                    {
                        doc.DataPath = value;
                    }
                }
            }
        }
        [JsonPropertyName("identifier")]
        public string Identifier { get; set; }
        /// <summary>
        /// Assistant name
        /// </summary>
        [JsonPropertyName("assistant_name")]
        [DILocalizedDisplayName(nameof(NAME_APIPlayerData_Name), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_APIPlayerData_Name), typeof(UIResources))]
        [ReadOnly(true)]
        public string Name { get; set; }
        /// <summary>
        /// Assistant main instructions filename
        /// </summary>
        [JsonPropertyName("instructions")]
        [DILocalizedDisplayName(nameof(NAME_APIPlayerData_Instructions), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_APIPlayerData_Instructions), typeof(UIResources))]
        public string Instructions
        {
            get
            {
                return _instructions;
            }
            set
            {
                if (string.Equals(value ?? "", Path.GetFileName(value ?? "")))
                {
                    _instructions = value;
                }
                else if (!string.IsNullOrEmpty(value) && File.Exists(value))
                {
                    if (string.Compare(Path.GetFullPath(DataPath), Path.GetDirectoryName(value), true) != 0)
                    {
                        File.Copy(value, Path.Combine(DataPath, Path.GetFileName(value)), true);
                    }
                    _instructions = Path.GetFileName(value);
                }
                else
                {
                    _instructions = string.Empty;
                }
            }
        }
        /// <summary>
        /// Set to true to update the instructions
        /// </summary>
        [JsonPropertyName("update_instructions")]
        [DILocalizedDisplayName(nameof(NAME_AssistantUpdates_UpdateInstructions), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_AssistantUpdates_UpdateInstructions), typeof(UIResources))]
        public bool UpdateInstructions { get; set; }
        /// <summary>
        /// Assistant private documents
        /// </summary>
        [JsonPropertyName("documents")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [DILocalizedDisplayName(nameof(NAME_AssistantUpdates_Documents), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_AssistantUpdates_Documents), typeof(UIResources))]
        public List<UpdateDocument> Documents { get; set; } = new List<UpdateDocument>();
        public bool Equals(AssistantUpdates other)
        {
            if (other == null)
            {
                return false;
            }
            return string.Compare(Identifier, other.Identifier, true) == 0;
        }
    }
    /// <summary>
    /// Service updates configuration
    /// </summary>
    public class ServiceUpdates : IStandardObject, IEquatable<ServiceUpdates>
    {
        private string _datapath;
        /// <summary>
        /// IStandardObject: Unique identifier
        /// </summary>
        [JsonIgnore]
        [Browsable(false)]
        public string StdUID { get; }
        /// <summary>
        /// IStandardObject: Element name
        /// </summary>
        [JsonIgnore]
        [Browsable(false)]
        public string StdName { get { return ServiceId; } }
        /// <summary>
        /// IStandardObject: Element description
        /// </summary>
        [JsonIgnore]
        [Browsable(false)]
        public string StdDescription { get; }
        /// <summary>
        /// IStandardObject: Information to edit the object
        /// </summary>
        [JsonIgnore]
        [Browsable(false)]
        public IUIDataSheet DataSheet { get; }
        [JsonIgnore]
        [Browsable(false)]
        public string DataPath
        {
            get
            {
                return _datapath;
            }
            set
            {
                _datapath = value;
                if (Documents != null)
                {
                    foreach (UpdateDocument doc in Documents)
                    {
                        doc.DataPath = value;
                    }
                }
            }
        }
        /// <summary>
        /// Service unique identifier
        /// </summary>
        [JsonPropertyName("service_id")]
        [DILocalizedDisplayName(nameof(NAME_ServiceUpdates_ServiceId), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_ServiceUpdates_ServiceId), typeof(UIResources))]
        public string ServiceId { get; set; }
        /// <summary>
        /// Service private documents
        /// </summary>
        [JsonPropertyName("documents")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [DILocalizedDisplayName(nameof(NAME_ServiceUpdates_Documents), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_ServiceUpdates_Documents), typeof(UIResources))]
        public List<UpdateDocument> Documents { get; set; } = new List<UpdateDocument>();
        public bool Equals(ServiceUpdates other)
        {
            if (other == null)
            {
                return false;
            }
            return string.Compare(ServiceId, other.ServiceId, true) == 0;
        }
    }
    /// <summary>
    /// Assistant or service document
    /// </summary>
    public class UpdateDocument : UIDataSheet, IStandardObject, IEquatable<UpdateDocument>
    {
        private List<PropertyEditorInfo> _info;
        private string _filename;
        [Browsable(false)]
        [JsonIgnore]
        public override List<PropertyEditorInfo> Properties
        {
            get
            {
                if (_info == null)
                {
                    _info = new List<PropertyEditorInfo>
                    {
                        new PropertyEditorInfo() { EditorType = InputEditorType.BlockTitle, PropertyName = BTTL_Document },
                        new PropertyEditorInfo() { EditorType = InputEditorType.FileName, PropertyName = nameof(FileName), InitialValue = FileName, Values = new List<object> { FILTER_Text, FILTER_AllFiles } },
                        new PropertyEditorInfo() { EditorType = InputEditorType.BoolValue, PropertyName = nameof(Update) },
                        new PropertyEditorInfo() { EditorType = InputEditorType.BoolValue, PropertyName = nameof(Remove) }
                    };
                }
                return _info;
            }
            set
            {
                _info = value;
            }
        }
        /// <summary>
        /// IStandardObject: Unique identifier
        /// </summary>
        [JsonIgnore]
        [Browsable(false)]
        public string StdUID { get; }
        /// <summary>
        /// IStandardObject: Element name
        /// </summary>
        [JsonIgnore]
        [Browsable(false)]
        public string StdName { get { return FileName; } }
        /// <summary>
        /// IStandardObject: Element description
        /// </summary>
        [JsonIgnore]
        [Browsable(false)]
        public string StdDescription { get; }
        /// <summary>
        /// IStandardObject: Information to edit the object
        /// </summary>
        [JsonIgnore]
        [Browsable(false)]
        public IUIDataSheet DataSheet { get { return this; } }
        [JsonIgnore]
        [Browsable(false)]
        public string DataPath { get; set; }
        /// <summary>
        /// Set to true to update the document
        /// </summary>
        [JsonPropertyName("update")]
        [DILocalizedDisplayName(nameof(NAME_ApplicationDocument_Update), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_ApplicationDocument_Update), typeof(UIResources))]
        public bool Update { get; set; }
        [JsonPropertyName("remove")]
        [DILocalizedDisplayName(nameof(NAME_ApplicationDocument_Remove), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_ApplicationDocument_Remove), typeof(UIResources))]
        public bool Remove { get; set; }
        /// <summary>
        /// Document filename in the application repository
        /// </summary>
        [JsonPropertyName("file_name")]
        [DILocalizedDisplayName(nameof(NAME_ApplicationDocument_FileName), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_ApplicationDocument_FileName), typeof(UIResources))]
        public string FileName
        {
            get
            {
                return _filename;
            }
            set
            {
                if (string.Equals(value ?? "", Path.GetFileName(value ?? "")))
                {
                    _filename = value;
                }
                else if (!string.IsNullOrEmpty(value) && File.Exists(value))
                {
                    if (string.Compare(Path.GetFullPath(DataPath), Path.GetDirectoryName(value), true) != 0)
                    {
                        File.Copy(value, Path.Combine(DataPath, Path.GetFileName(value)), true);
                    }
                    _filename = Path.GetFileName(value);
                }
                else
                {
                    _filename = string.Empty;
                }
            }
        }
        public bool Equals(UpdateDocument other)
        {
            if (other == null)
            {
                return false;
            }
            return string.Compare(FileName, other.FileName, true) == 0;
        }
    }
    /// <summary>
    /// Internal update document class
    /// </summary>
    public class WorkDocument : IEquatable<WorkDocument>
    {
        /// <summary>
        /// File unique identifier in IFileManager
        /// </summary>
        public string FileId { get; set; }
        /// <summary>
        /// FileManager o store the file
        /// </summary>
        public IFileManager FileManager { get; set; }
        /// <summary>
        /// Provider identifier
        /// </summary>
        public string ProviderId { get; set; }
        /// <summary>
        /// Document filename in the application repository
        /// </summary>
        public string FileName { get; set; }
        public bool Equals(WorkDocument other)
        {
            if (other == null)
            {
                return false;
            }
            if (ProviderId != other.ProviderId)
            {
                return false;
            }
            if (!string.IsNullOrEmpty(FileId) && !string.IsNullOrEmpty(other.FileId))
            {
                return FileId == other.FileId;
            }
            return string.Compare(FileName, other.FileName, true) == 0;
        }
    }
}
