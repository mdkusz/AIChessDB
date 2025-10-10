using AIAssistants.Interfaces;
using AIChessDatabase.Properties;
using GlobalCommonEntities.DependencyInjection;
using GlobalCommonEntities.Interfaces;
using GlobalCommonEntities.Json.Converters;
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

namespace AIChessDatabase.Setup
{
    /// <summary>
    /// UIDataSheet for player configuration.
    /// </summary>
    public class PlayerSetupDataSheet : UIDataSheet
    {
        private List<PropertyEditorInfo> _info;
        private IAPIManager _apiManager;
        private string _name;

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
                        new PropertyEditorInfo() { EditorType = InputEditorType.FixedComboBox, PropertyName = nameof(Type), Required = true },
                        new PropertyEditorInfo() { EditorType = InputEditorType.FixedComboBox, PropertyName = nameof(Model), Required = true },
                        new PropertyEditorInfo() { EditorType = InputEditorType.Color, PropertyName = nameof(BackColor) },
                        new PropertyEditorInfo() { EditorType = InputEditorType.Color, PropertyName = nameof(ForeColor) }
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
        /// APIManager to get and create players and player assets
        /// </summary>
        [Browsable(false)]
        public IAPIManager APIManager
        {
            get
            {
                return _apiManager;
            }
            set
            {
                _apiManager = value;
                Properties[2].Values = new List<object>();
                Properties[3].Values = new List<object>();
                Type = null;
                if (_apiManager != null)
                {
                    if (_apiManager.LLMModels != null)
                    {
                        Properties[3].Values = new List<object>(_apiManager.LLMModels);
                    }
                    foreach (ObjectWrapper<string> type in _apiManager.AvailablePLayerTypes)
                    {
                        Type atype = _apiManager.NameToTYpe(type.TypedImplementation);
                        if ((atype != null) && typeof(IDocumentAnalyzer).IsAssignableFrom(atype))
                        {
                            Properties[2].Values.Add(type);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Player name
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_PlayerName), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_PlayerName), typeof(UIResources))]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value != _name)
                {
                    if (value == null)
                    {
                        _name = value;
                    }
                    else
                    {
                        // The name will be used as part of file names, so remove invalid characters
                        char[] invalidChars = Path.GetInvalidFileNameChars();
                        _name = new string(value.Where(c => !invalidChars.Contains(c)).ToArray());
                    }
                }
            }
        }
        /// <summary>
        /// Interface type
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_PlayerType), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_PlayerType), typeof(UIResources))]
        public ObjectWrapper<string> Type { get; set; }
        /// <summary>
        /// LLM model name used by the player
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_APIPlayerData_Model), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_APIPlayerData_Model), typeof(UIResources))]
        public string Model { get; set; }
        /// <summary>
        /// Player header background color
        /// </summary>
        [JsonConverter(typeof(ColorJsonConverter))]
        [DILocalizedDisplayName(nameof(NAME_PlayerBackColor), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_PlayerBackColor), typeof(UIResources))]
        public Color BackColor { get; set; }
        /// <summary>
        /// Player header foreground color
        /// </summary>
        [JsonConverter(typeof(ColorJsonConverter))]
        [DILocalizedDisplayName(nameof(NAME_PlayerForeColor), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_PlayerForeColor), typeof(UIResources))]
        public Color ForeColor { get; set; }
        public override string ToString()
        {
            return string.Join(".", Name, Type.TypedImplementation, Model);
        }
    }
}
