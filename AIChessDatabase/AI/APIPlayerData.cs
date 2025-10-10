using AIAssistants.Interfaces;
using AIChessDatabase.Properties;
using GlobalCommonEntities.Interfaces;
using GlobalCommonEntities.UI;
using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using static AIChessDatabase.Properties.UIResources;

namespace AIChessDatabase.AI
{
    /// <summary>
    /// Minimum data structure for IAPIPlayer objects.
    /// </summary>
    public class APIPlayerData : UIDataSheet
    {
        private List<PropertyEditorInfo> _info;
        private string _Identifier = string.Empty;
        private string _Name = string.Empty;
        private string _Description = string.Empty;
        private string _Instructions = string.Empty;
        private string _Model = string.Empty;
        private string _Voice = string.Empty;
        private string _Reasoning = null;
        private List<string> _reasoninglevels;

        public APIPlayerData(IAPIPlayer player) : base()
        {
            Player = player;
            _Identifier = player.Identifier;
            _Name = player.Name;
            _Description = player.Description;
            _Instructions = player.SystemInstructions;
            _Model = player.Model;
            _Voice = player.Voice;
            _Reasoning = null;
            _reasoninglevels = player.ReasoningEffort(ref _Reasoning);
            if (_reasoninglevels != null)
            {
                // Add an empty value to allow no selection
                _reasoninglevels.Insert(0, "");
            }
        }
        /// <summary>
        /// Element to be edited
        /// </summary>
        [Browsable(false)]
        public IAPIPlayer Player { get; set; }
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
                        new PropertyEditorInfo() { EditorType = InputEditorType.BlockTitle, PropertyName = BTTL_APIPlayer },
                        new PropertyEditorInfo() { EditorType = InputEditorType.SingleLineText, PropertyName = nameof(Identifier) },
                        new PropertyEditorInfo() { EditorType = InputEditorType.SingleLineText, PropertyName = nameof(Name)},
                        new PropertyEditorInfo() { EditorType = InputEditorType.SingleLineText, PropertyName = nameof(Description)},
                        new PropertyEditorInfo() { EditorType = InputEditorType.FixedComboBox, PropertyName = nameof(Model), InitialValue = Model, Values = Player?.APIManager?.LLMModels?.Cast<object>()?.ToList() },
                        new PropertyEditorInfo() { EditorType = InputEditorType.SingleLineText, PropertyName = nameof(Voice) }
                    };
                    if (_reasoninglevels != null)
                    {
                        _info.Add(new PropertyEditorInfo() { EditorType = InputEditorType.FixedComboBox, PropertyName = nameof(ReasoningLevel), InitialValue = _Reasoning, Values = _reasoninglevels.Cast<object>().ToList() });
                    }
                }
                return _info;
            }
            set
            {
                _info = value;
            }
        }
        /// <summary>
        /// Unique identiifer
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_APIPlayerData_Identifier), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_APIPlayerData_Identifier), typeof(UIResources))]
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
                    Player.Identifier = value;
                    InvokePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Player name
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_APIPlayerData_Name), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_APIPlayerData_Name), typeof(UIResources))]
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
                    if (value == null)
                    {
                        _Name = value;
                    }
                    else
                    {
                        // The name will be used as part of file names, so remove invalid characters
                        char[] invalidChars = Path.GetInvalidFileNameChars();
                        _Name = new string(value.Where(c => !invalidChars.Contains(c)).ToArray());
                        Player.Name = _Name;
                    }
                    InvokePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Player description
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_APIPlayerData_Description), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_APIPlayerData_Description), typeof(UIResources))]
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
                    Player.Description = value;
                    InvokePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Player initial (system) instructions
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_APIPlayerData_Instructions), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_APIPlayerData_Instructions), typeof(UIResources))]
        public string Instructions
        {
            get
            {
                return _Instructions;
            }
            set
            {
                if (value != _Instructions)
                {
                    _Instructions = value;
                    Player.SystemInstructions = value;
                    InvokePropertyChanged();
                }
            }
        }
        /// <summary>
        /// LLM model name used by the player
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_APIPlayerData_Model), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_APIPlayerData_Model), typeof(UIResources))]
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
                    Player.Model = value;
                    InvokePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Reasoning level used by the player
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_APIPlayerData_ReasoningLevel), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_APIPlayerData_ReasoningLevel), typeof(UIResources))]
        public string ReasoningLevel
        {
            get
            {
                return _Reasoning;
            }
            set
            {
                if (value != _Reasoning)
                {
                    _Reasoning = value;
                    Player.ReasoningEffort(ref _Reasoning);
                    InvokePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Voice name used by the player if speech is enabled
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_APIPlayerData_Voice), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_APIPlayerData_Voice), typeof(UIResources))]
        public string Voice
        {
            get
            {
                return _Voice;
            }
            set
            {
                if (value != _Voice)
                {
                    _Voice = value;
                    Player.Voice = value;
                    InvokePropertyChanged();
                }
            }
        }
    }
}
