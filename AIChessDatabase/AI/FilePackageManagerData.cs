using AIAssistants.Interfaces;
using AIChessDatabase.Properties;
using GlobalCommonEntities.DependencyInjection;
using GlobalCommonEntities.Interfaces;
using GlobalCommonEntities.UI;
using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using static AIChessDatabase.Properties.Resources;
using static AIChessDatabase.Properties.UIResources;

namespace AIChessDatabase.AI
{
    /// <summary>
    /// Minimum data structure for IFIlePackegeManager objects.
    /// </summary>
    public class FilePackageManagerData : UIDataSheet, ISelectionObjectProvider
    {
        private List<PropertyEditorInfo> _info;
        private string _Identifier = string.Empty;
        private string _Name = string.Empty;
        private string _Description = string.Empty;
        public FilePackageManagerData(IFilePackageManager pkgmgr) : base()
        {
            PackageManager = pkgmgr;
            Identifier = pkgmgr.Identifier;
            Name = pkgmgr.Name;
            Description = pkgmgr.Description;
        }
        /// <summary>
        /// Element to be edited
        /// </summary>
        [Browsable(false)]
        public IFilePackageManager PackageManager { get; set; }
        /// <summary>
        /// File manager information for editing.
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_FIlePackageManagerData_FileManager), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_FIlePackageManagerData_FileManager), typeof(UIResources))]
        public FileManagerData FileManager
        {
            get
            {
                return new FileManagerData(PackageManager.FileManager);
            }
            set { }
        }
        /// <summary>
        /// Property editor information for editing.
        /// </summary>
        [Browsable(false)]
        public override List<PropertyEditorInfo> Properties
        {
            get
            {
                if (_info == null)
                {
                    _info = new List<PropertyEditorInfo>
                    {
                        new PropertyEditorInfo() { EditorType = InputEditorType.BlockTitle, PropertyName = BTTL_PackageManager },
                        new PropertyEditorInfo() { EditorType = InputEditorType.SingleLineText, PropertyName = nameof(Identifier), ReadOnly = true },
                        new PropertyEditorInfo() { EditorType = InputEditorType.SingleLineText, PropertyName = nameof(Name)},
                        new PropertyEditorInfo() { EditorType = InputEditorType.SingleLineText, PropertyName = nameof(Description)},
                        new PropertyEditorInfo() { EditorType = InputEditorType.ObjectSelector, PropertyName = nameof(InvokeAction), CommandLabel = CMD_ManageFiles, CommandCaptions = new List<string> { CMD_ManageFiles }, MethodParameters = new Type[] { typeof(PropertyEditorInfo) } }
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
        /// Unique identifier for the file package manager.
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_FIlePackageManagerData_Identifier), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_FIlePackageManagerData_Identifier), typeof(UIResources))]
        public string Identifier
        {
            get
            {
                return _Identifier;
            }
            set
            {
                if (value != _Identifier)
                {
                    _Identifier = value;
                    InvokePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Package name.
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_FIlePackageManagerData_Name), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_FIlePackageManagerData_Name), typeof(UIResources))]
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (value != _Name)
                {
                    _Name = value;
                    PackageManager.Name = value;
                    InvokePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Package description.
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_FIlePackageManagerData_Description), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_FIlePackageManagerData_Description), typeof(UIResources))]
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                if (value != _Description)
                {
                    _Description = value;
                    PackageManager.Description = value;
                    InvokePropertyChanged();
                }
            }
        }
        /// <summary>
        /// List of files in the package.
        /// </summary>
        [Browsable(false)]
        public List<FilePackageFile> Files { get; set; }
        /// <summary>
        /// ISelectionObjectProvider: Title for the selection user interface
        /// </summary>
        [Browsable(false)]
        public string Title { get { return BTTL_PackageManager; } set { } }
        /// <summary>
        /// ISelectionObjectProvider: Event to notify that the user wants to select an object
        /// </summary>
        public event SelectObjectsHandler SelectionUIInvoked;
        /// <summary>
        /// ISelectionObjectProvider: Release SelectObjectsHandler event subscriptions
        /// </summary>
        public void ReleaseEvent()
        {
            if (SelectionUIInvoked != null)
            {
                foreach (var d in SelectionUIInvoked.GetInvocationList())
                {
                    SelectionUIInvoked -= (SelectObjectsHandler)d;
                }
            }
            SelectionUIInvoked = null;
        }
        /// <summary>
        /// ISelectionObjectProvider: Check if there are more objects to select from
        /// </summary>
        /// <param name="property">
        /// Property editor invoking the selection
        /// </param>
        public bool HasMore(PropertyEditorInfo property)
        {
            return false;
        }
        /// <summary>
        /// ISelectionObjectProvider: Get a list of objects to select from
        /// </summary>
        /// <returns>
        /// List of objects
        /// </returns>
        /// <param name="property">
        /// Property to get the object
        /// </param>
        public async Task<List<object>> GetSelectionObjects(PropertyEditorInfo property)
        {
            List<object> result = null;
            List<ObjectWrapper> flist = await PackageManager.FileManager.GetFileList();
            if (Files == null)
            {
                Files = new List<FilePackageFile>();
                List<ObjectWrapper> vlist = await PackageManager.ListPackageFiles(PackageManager.Identifier);
                foreach (ObjectWrapper ow in vlist)
                {
                    Files.Add(new FilePackageFile()
                    {
                        FileId = ow.UID
                    });
                }
            }
            result = new List<object>();
            foreach (ObjectWrapper ow in flist)
            {
                FilePackageFile vf = Files.Find(f => f.FileId == ow.UID);
                if (vf == null)
                {
                    result.Add(new FilePackageFile()
                    {
                        FileId = ow.UID,
                        FileName = ow.FriendlyName
                    });
                }
                else
                {
                    vf.FileName = ow.FriendlyName;
                }
            }
            return result;
        }
        /// <summary>
        /// ISelectionObjectProvider: Get a list of objects already selected
        /// </summary>
        /// <returns>
        /// List of objects
        /// </returns>
        /// <param name="property">
        /// Property to get the object list
        /// </param>
        public async Task<List<object>> GetSelectedObjects(PropertyEditorInfo property)
        {
            List<object> result = new List<object>();
            if (Files == null)
            {
                Files = new List<FilePackageFile>();
                List<ObjectWrapper> flist = await PackageManager.FileManager.GetFileList();
                List<ObjectWrapper> vlist = await PackageManager.ListPackageFiles(PackageManager.Identifier);
                foreach (ObjectWrapper ow in vlist)
                {
                    ObjectWrapper found = flist.Find(f => f.UID == ow.UID);
                    Files.Add(new FilePackageFile()
                    {
                        FileId = ow.UID,
                        FileName = found?.FriendlyName
                    });
                }
            }
            result.AddRange(Files);
            return result;
        }
        /// <summary>
        /// ISelectionObjectProvider: Set the list of selected objects
        /// </summary>
        /// <param name="objects">
        /// Selected object list
        /// </param>
        /// <param name="property">
        /// Property to set the selection
        /// </param>
        public async Task<List<string>> SetSelection(List<object> objects, PropertyEditorInfo property)
        {
            List<string> errors = new List<string>();
            foreach (object obj in objects)
            {
                try
                {
                    await PackageManager.AddFile(PackageManager.Identifier, ((FilePackageFile)obj).FileId);
                    await Task.Delay(500); // Wait to not overload the file package API
                }
                catch (Exception ex)
                {
                    errors.Add($"{obj?.ToString()}: {ex.Message}");
                }
            }
            return errors;
        }
        /// <summary>
        /// ISelectionObjectProvider: Remove a list of objects
        /// </summary>
        /// <param name="objects">
        /// Object to remove list
        /// </param>
        /// <param name="property">
        /// Property to remove the selection
        /// </param>
        public async Task<List<string>> RemoveSelection(List<object> objects, PropertyEditorInfo property)
        {
            List<string> errors = new List<string>();
            UpdateAssistantsConfiguration updcfg = JsonSerializer.Deserialize<UpdateAssistantsConfiguration>(File.ReadAllText(Path.Combine(ConfigurationManager.AppSettings[SETTING_ConfigPath],
                ConfigurationManager.AppSettings[SETTING_updateConfiguration])));
            foreach (object obj in objects)
            {
                FilePackageFile file = obj as FilePackageFile;
                if (file != null)
                {
                    try
                    {
                        // Remove the file from the updates configuration
                        foreach (AssistantUpdates au in updcfg.AssistantUpdates)
                        {
                            if (au.Documents != null)
                            {
                                au.Documents.RemoveAll(d => string.Compare(d.FileName, file.FileName, true) == 0);
                            }
                        }
                        foreach (ServiceUpdates su in updcfg.ServiceUpdates)
                        {
                            if (su.Documents != null)
                            {
                                su.Documents.RemoveAll(d => string.Compare(d.FileName, file.FileName, true) == 0);
                            }
                        }
                        // Save the updates configuration
                        File.WriteAllText(Path.Combine(ConfigurationManager.AppSettings[SETTING_dataPath],
                            ConfigurationManager.AppSettings[SETTING_updateConfiguration]), JsonSerializer.Serialize(updcfg,
                                new JsonSerializerOptions { WriteIndented = true }));
                        // Remove the file from the file package
                        await PackageManager.DeletePackageFile(PackageManager.Identifier, file.FileId);
                        await Task.Delay(1000); // Wait for the file to be removed
                    }
                    catch (Exception ex)
                    {
                        errors.Add($"{file.FileName}.{file.FileId}: {ex.Message}");
                    }
                }
            }
            return errors;
        }
        /// <summary>
        /// ISelectionObjectProvider: Remove the object container
        /// </summary>
        /// <param name="property">
        /// Property editor invoking the selection
        /// </param>
        public async Task<string> RemoveContainer(PropertyEditorInfo property)
        {
            await Task.Yield();
            return null;
        }
        /// <summary>
        /// ISelectionObjectProvider: Fire event to select an object
        /// </summary>
        /// <param name="sender">
        /// Object invoking the selection
        /// </param>
        /// <param name="property">
        /// Property editor invoking the selection
        /// </param>
        public void InvokeAction(object sender, PropertyEditorInfo property)
        {
            SelectionUIInvoked?.Invoke(this, new SelectObjectsEventArgs(property));
        }
        /// <summary>
        /// Invoke UI interface to select objets to add or remove from a tool
        /// </summary>
        /// <param name="property">
        /// Invoker property editor information
        /// </param>
        public void InvokeAction(PropertyEditorInfo property)
        {
            InvokeAction(this, property);
        }
        /// <summary>
        /// This method is called by PropertyEditorInfo to resolve the parameter structure for a given method
        /// </summary>
        /// <param name="methodName">
        /// Method name to resolve
        /// </param>
        /// <param name="pinfo">
        /// PropertyEditorInfo object to configure
        /// </param>
        public override void SetMethodParameterTypes(string methodName, PropertyEditorInfo pinfo)
        {
            if (methodName == nameof(InvokeAction))
            {
                pinfo.MethodParameters = new Type[] { typeof(PropertyEditorInfo) };
                pinfo.MethodParameterValues = new object[] { pinfo };
            }
            else
            {
                base.SetMethodParameterTypes(methodName, pinfo);
            }
        }
    }
    /// <summary>
    /// Represents a file in a vector store.
    /// </summary>
    public class FilePackageFile
    {
        /// <summary>
        /// File unique identifier
        /// </summary>
        public string FileId { get; set; }
        /// <summary>
        /// File name
        /// </summary>
        public string FileName { get; set; }
    }
}
