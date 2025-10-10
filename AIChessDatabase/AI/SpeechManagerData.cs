using AIAssistants.Interfaces;
using AIChessDatabase.Properties;
using GlobalCommonEntities.Interfaces;
using GlobalCommonEntities.UI;
using Resources;
using System.Collections.Generic;
using System.ComponentModel;
using static AIChessDatabase.Properties.UIResources;

namespace AIChessDatabase.AI
{
    /// <summary>
    /// Minimum data structure for ISpeechManager objects.
    /// </summary>
    public class SpeechManagerData : UIDataSheet
    {
        private List<PropertyEditorInfo> _info;
        private string _Identifier = string.Empty;
        private string _Name = string.Empty;
        private string _Description = string.Empty;
        private string _Model = string.Empty;
        public SpeechManagerData(ISpeechManager speech) : base()
        {
            Speech = speech;
            Identifier = speech.Identifier;
            Name = speech.Name;
            Description = speech.Description;
            Model = speech.Model;
        }
        /// <summary>
        /// Element to be edited
        /// </summary>
        [Browsable(false)]
        public ISpeechManager Speech { get; set; }
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
                        new PropertyEditorInfo() { EditorType = InputEditorType.BlockTitle, PropertyName = BTTL_SpeechManager },
                        new PropertyEditorInfo() { EditorType = InputEditorType.SingleLineText, PropertyName = nameof(Identifier) },
                        new PropertyEditorInfo() { EditorType = InputEditorType.SingleLineText, PropertyName = nameof(Name)},
                        new PropertyEditorInfo() { EditorType = InputEditorType.SingleLineText, PropertyName = nameof(Description)},
                        new PropertyEditorInfo() { EditorType = InputEditorType.FixedComboBox, PropertyName = nameof(Model), InitialValue = Model, Values = ((IModelUser)Speech)?.ModelProperty?.Values }
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
        /// Unique identifier
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_SpeechManagerData_Identifier), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_SpeechManagerData_Identifier), typeof(UIResources))]
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
                    Speech.Identifier = value;
                    InvokePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Speech manager name
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_SpeechManagerData_Name), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_SpeechManagerData_Name), typeof(UIResources))]
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
                    Speech.Name = value;
                    InvokePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Speech manager description
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_SpeechManagerData_Description), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_SpeechManagerData_Description), typeof(UIResources))]
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
                    Speech.Description = value;
                    InvokePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Speech manager model
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_SpeechManagerData_Model), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_SpeechManagerData_Model), typeof(UIResources))]
        public string Model
        {
            get
            {
                return _Model;
            }
            set
            {
                if (value != _Model)
                {
                    _Model = value;
                    Speech.Model = value;
                    InvokePropertyChanged();
                }
            }
        }
    }
}
