using AIAssistants.Interfaces;
using AIChessDatabase.Properties;
using GlobalCommonEntities.DependencyInjection;
using GlobalCommonEntities.Interfaces;
using GlobalCommonEntities.UI;
using Resources;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;
using static AIChessDatabase.Properties.UIResources;

namespace AIChessDatabase.Setup
{
    /// <summary>
    /// Data sheet to setup AI assistants to be used with the application.
    /// </summary>
    public class AssistantsSetup : UIDataSheet
    {
        private List<PropertyEditorInfo> _info;
        private List<ObjectWrapper<IAPIManager>> _apis;
        private ObjectWrapper<IAPIManager> _apiManager;
        private IAppAutomation _app;
        private string _user;
        private PlayerSetupDataSheet _ChessMate;
        private PlayerSetupDataSheet _Caspov;
        private PlayerSetupDataSheet _MorFix;
        public AssistantsSetup(IAppAutomation app)
        {
            _app = app;
            _apis = _app.ApiManagers;
            if (_apis.Count == 1)
            {
                _apiManager = _apis[0];
            }
            _ChessMate = new PlayerSetupDataSheet()
            {
                Name = "ChessMate",
                APIManager = _apiManager?.TypedImplementation
            };
            _Caspov = new PlayerSetupDataSheet()
            {
                Name = "Caspov",
                APIManager = _apiManager?.TypedImplementation
            };
            _MorFix = new PlayerSetupDataSheet()
            {
                Name = "MorFix",
                APIManager = _apiManager?.TypedImplementation
            };
        }
        [JsonIgnore]
        [Browsable(false)]
        public override List<PropertyEditorInfo> Properties
        {
            get
            {
                if (_info == null)
                {
                    _info = new List<PropertyEditorInfo>()
                    {
                        new PropertyEditorInfo() { EditorType = InputEditorType.BlockTitle, PropertyName = BTTL_Assistants },
                        new PropertyEditorInfo() { EditorType = InputEditorType.FixedComboBox, PropertyName = nameof(APIManager) },
                        new PropertyEditorInfo() { EditorType = InputEditorType.SingleLineText, PropertyName = nameof(UserName), Required = true },
                        new PropertyEditorInfo() { EditorType = InputEditorType.DataSheet, PropertyName = nameof(ChessMate), Required = true },
                        new PropertyEditorInfo() { EditorType = InputEditorType.DataSheet, PropertyName = nameof(Caspov), Required = true },
                        new PropertyEditorInfo() { EditorType = InputEditorType.DataSheet, PropertyName = nameof(MorFix), Required = true },
                        new PropertyEditorInfo() { EditorType = InputEditorType.MultilineText, PropertyName = nameof(Instructions), ReadOnly = true, MaxUnits = -1 }
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
        /// API Manager to use for the assistants
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_APIManager), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_APIManager), typeof(UIResources))]
        public ObjectWrapper<IAPIManager> APIManager
        {
            get
            {
                return _apiManager;
            }
            set
            {
                if (_apiManager != value)
                {
                    _apiManager = value;
                    _ChessMate.APIManager = _apiManager?.TypedImplementation;
                    _Caspov.APIManager = _apiManager?.TypedImplementation;
                    _MorFix.APIManager = _apiManager?.TypedImplementation;
                    InvokePropertyChanged();
                }
            }
        }
        /// <summary>
        /// User name for the assistants to address the user
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_UserName), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_UserName), typeof(UIResources))]
        public string UserName
        {
            get
            {
                return _user;
            }
            set
            {
                if (value != _user)
                {
                    _user = value;
                    InvokePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Assistant to help with the application user interface
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_ChessMate), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_ChessMate), typeof(UIResources))]
        public PlayerSetupDataSheet ChessMate
        {
            get
            {
                return _ChessMate;
            }
            set
            {
                if (value != _ChessMate)
                {
                    _ChessMate = value;
                    InvokePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Assistant to play chess and comment on games
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_Caspov), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_Caspov), typeof(UIResources))]
        public PlayerSetupDataSheet Caspov
        {
            get
            {
                return _Caspov;
            }
            set
            {
                if (value != _Caspov)
                {
                    _Caspov = value;
                    InvokePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Assist users with the application configuration and setup
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_MorFix), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_MorFix), typeof(UIResources))]
        public PlayerSetupDataSheet MorFix
        {
            get
            {
                return _MorFix;
            }
            set
            {
                if (value != _MorFix)
                {
                    _MorFix = value;
                    InvokePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Setup instructions
        /// </summary>
        [DILocalizedDisplayName(nameof(CAP_Instructions), typeof(UIResources))]
        public string Instructions { get { return SETUP_Assistants; } set { } }
    }
}
