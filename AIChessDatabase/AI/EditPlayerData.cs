using AIAssistants.Interfaces;
using AIChessDatabase.Properties;
using GlobalCommonEntities.Interfaces;
using GlobalCommonEntities.UI;
using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using static AIChessDatabase.Properties.UIResources;

namespace AIChessDatabase.AI
{
    /// <summary>
    /// Data to edit players in the console
    /// </summary>
    public class EditPlayerData : UIDataSheet
    {
        private List<PropertyEditorInfo> _info;
        private string _Name = string.Empty;
        private string _Instructions = string.Empty;
        private string _Model = string.Empty;
        private string _Voice = string.Empty;
        private Color _bColor;
        private Color _fColor;
        private double _teperature;
        private double _topP;
        private bool? _parallelCalls = null;
        private bool _webSearch = false;
        private string _Reasoning = null;
        private List<string> _reasoninglevels;

        public EditPlayerData(IAPIPlayer player)
        {
            Player = player;
            _Name = player.Name;
            _Instructions = player.SystemInstructions;
            _Model = player.Model;
            _Voice = player.Voice;
            _bColor = player.BackColor;
            _fColor = player.ForeColor;
            _teperature = player.Temperature;
            _topP = player.TopP;
            _parallelCalls = player.ParallelTools;
            _webSearch = player.Tools.HasFlag(ToolTypes.WebSearch);
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
        [JsonIgnore]
        [Browsable(false)]
        public override List<PropertyEditorInfo> Properties
        {
            get
            {
                if (_info == null)
                {
                    _info = new List<PropertyEditorInfo>
                    {
                        new PropertyEditorInfo() { EditorType = InputEditorType.BlockTitle, PropertyName = BTTL_PLayPlayer },
                        new PropertyEditorInfo() { EditorType = InputEditorType.SingleLineText, PropertyName = nameof(Name), Required = true },
                        new PropertyEditorInfo() { EditorType = InputEditorType.FixedComboBox, PropertyName = nameof(Model), Required = true, InitialValue = Model, Values = Player?.APIManager?.LLMModels?.Cast<object>()?.ToList() },
                        new PropertyEditorInfo() { EditorType = InputEditorType.FloatValue, PropertyName = nameof(TopP), Values = new List<object> { 0, 1 } },
                        new PropertyEditorInfo() { EditorType = InputEditorType.FloatValue, PropertyName = nameof(Temperature), Values = new List<object> { 0, 2 } },
                        new PropertyEditorInfo() { EditorType = InputEditorType.BoolValue, PropertyName = nameof(ParallelCalls) }
                    };
                    if (_reasoninglevels != null)
                    {
                        _info.Add(new PropertyEditorInfo() { EditorType = InputEditorType.FixedComboBox, PropertyName = nameof(ReasoningLevel), InitialValue = _Reasoning, Values = _reasoninglevels.Cast<object>().ToList() });
                    }
                    if (Player.Capabilities.HasFlag(ElementCapabilities.WebSearch))
                    {
                        _info.Add(new PropertyEditorInfo() { EditorType = InputEditorType.BoolValue, PropertyName = nameof(WebSearch) });
                    }
                    _info.Add(new PropertyEditorInfo() { EditorType = InputEditorType.MultilineText, PropertyName = nameof(Instructions), MaxUnits = 10 });
                    _info.Add(new PropertyEditorInfo() { EditorType = InputEditorType.SingleLineText, PropertyName = nameof(Voice) });
                    _info.Add(new PropertyEditorInfo() { EditorType = InputEditorType.Color, PropertyName = nameof(BackColor) });
                    _info.Add(new PropertyEditorInfo() { EditorType = InputEditorType.Color, PropertyName = nameof(ForeColor) });
                }
                return _info;
            }
            set
            {
                _info = value;
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
                    }
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
                    InvokePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Player header background color
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_PlayerBackColor), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_PlayerBackColor), typeof(UIResources))]
        public Color BackColor
        {
            get
            {
                return _bColor;
            }
            set
            {
                if (value != _bColor)
                {
                    _bColor = value;
                    InvokePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Player header foreground color
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_PlayerForeColor), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_PlayerForeColor), typeof(UIResources))]
        public Color ForeColor
        {
            get
            {
                return _fColor;
            }
            set
            {
                if (value != _fColor)
                {
                    _fColor = value;
                    InvokePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Model temperature parameter
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_PlayerTemperature), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_PlayerTemperature), typeof(UIResources))]
        public double Temperature
        {
            get
            {
                return _teperature;
            }
            set
            {
                if (value != _teperature)
                {
                    _teperature = value;
                    InvokePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Model top-p parameter
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_PlayerTopP), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_PlayerTopP), typeof(UIResources))]
        public double TopP
        {
            get
            {
                return _topP;
            }
            set
            {
                if (value != _topP)
                {
                    _topP = value;
                    InvokePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Parallel tool calls if supported by the API
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_PlayerParallelCalls), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_PlayerParallelCalls), typeof(UIResources))]
        public bool? ParallelCalls
        {
            get
            {
                return _parallelCalls;
            }
            set
            {
                if (value != _parallelCalls)
                {
                    _parallelCalls = value;
                    InvokePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Parallel tool calls if supported by the API
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_PlayerWebSearch), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_PlayerWebSearch), typeof(UIResources))]
        public bool WebSearch
        {
            get
            {
                return _webSearch;
            }
            set
            {
                if (value != _webSearch)
                {
                    _webSearch = value;
                    InvokePropertyChanged();
                }
            }
        }
    }
}
