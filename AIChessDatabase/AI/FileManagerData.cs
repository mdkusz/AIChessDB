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
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using static AIChessDatabase.Properties.Resources;
using static AIChessDatabase.Properties.UIResources;

namespace AIChessDatabase.AI
{
    /// <summary>
    /// File manager to upload and manage files in the AI system.
    /// </summary>
    public class FileManagerData : UIDataSheet, ISelectionObjectProvider
    {
        private List<PropertyEditorInfo> _info;
        public FileManagerData(IFileManager fmgr) : base()
        {
            FileManager = fmgr;
        }
        /// <summary>
        /// Element to be edited
        /// </summary>
        [Browsable(false)]
        public IFileManager FileManager { get; set; }
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
                        new PropertyEditorInfo() { EditorType = InputEditorType.BlockTitle, PropertyName = BTTL_FileManager },
                        new PropertyEditorInfo() { EditorType = InputEditorType.FileCollection, PropertyName = nameof(FilePaths), Values = new List<object> { FIL_ALLAllowed, string.Join(";", FileManager.AllowedFileExtensions(ToolTypes.None)), FILTER_AllFiles } },
                        new PropertyEditorInfo() { EditorType = InputEditorType.ObjectSelector, PropertyName = nameof(InvokeAction), CommandLabel = CMD_UploadFiles, CommandCaptions = new List<string> { CMD_UploadFiles }, MethodParameters = new Type[] { typeof(PropertyEditorInfo) } }
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
        /// Data files in the local repositiory data path.
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_FileManagerData_FilePaths), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_FileManagerData_FilePaths), typeof(UIResources))]
        public List<string> FilePaths
        {
            get
            {
                return new List<string>(Directory.GetFiles(ConfigurationManager.AppSettings[SETTING_dataPath]));
            }
            set
            {
                if (value != null)
                {
                    string datapath = Path.GetFullPath(ConfigurationManager.AppSettings[SETTING_dataPath]);
                    foreach (string path in value)
                    {
                        if (string.Compare(datapath, Path.GetFullPath(Path.GetDirectoryName(path)), true) != 0)
                        {
                            File.Copy(path, Path.Combine(datapath, Path.GetFileName(path)), true);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// ISelectionObjectProvider: Title for the selection user interface
        /// </summary>
        [Browsable(false)]
        public string Title { get { return BTTL_FileManager; } set { } }
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
            List<ObjectWrapper> flist = await FileManager.GetFileList();
            result = new List<object>();
            foreach (string path in FilePaths)
            {
                if (flist.FirstOrDefault(f => string.Compare(f.FriendlyName, Path.GetFileName(path), true) == 0) == null)
                {
                    result.Add(Path.GetFileName(path));
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
            foreach (ObjectWrapper f in await FileManager.GetFileList())
            {
                result.Add(new FileManagerFile()
                {
                    FileId = f.UID,
                    FileName = f.FriendlyName
                });
            }
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
            string dataPath = ConfigurationManager.AppSettings[SETTING_dataPath];
            foreach (object obj in objects)
            {
                try
                {
                    await FileManager.UploadFile(Path.Combine(dataPath, obj.ToString()));
                    await Task.Delay(500); // Wait to not overload the file manager API
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
            List<ObjectWrapper<IAPIElement>> vslist = new List<ObjectWrapper<IAPIElement>>(await FileManager.APIManager.GetCurrentElements(nameof(IFilePackageManager)));
            List<IAPIElement> filePackages = ObjectWrapper<IAPIElement>.ConvertList(vslist);
            foreach (object obj in objects)
            {
                FileManagerFile file = obj as FileManagerFile;
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
                        // Remove the file from all the file packages
                        foreach (IFilePackageManager fpm in filePackages)
                        {
                            await fpm.DeletePackageFile(fpm.Identifier, file.FileId);
                            await Task.Delay(1000); // Wait for the file to be removed
                        }
                        // Remove the file from the file manager
                        await FileManager.DeleteFile(file.FileId, null);
                        await Task.Delay(500);
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
    }
    public class FileManagerFile
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
