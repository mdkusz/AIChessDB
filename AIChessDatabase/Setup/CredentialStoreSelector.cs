using AIAssistants.Interfaces;
using AIChessDatabase.Properties;
using GlobalCommonEntities.Config;
using GlobalCommonEntities.DependencyInjection;
using GlobalCommonEntities.Interfaces;
using GlobalCommonEntities.UI;
using Resources;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;
using static AIChessDatabase.Properties.Resources;
using static AIChessDatabase.Properties.UIResources;

namespace AIChessDatabase.Setup
{
    /// <summary> 
    /// Select a credential store type.
    /// </summary>
    public class CredentialStoreSelector : UIDataSheet
    {
        private List<PropertyEditorInfo> _info;
        private ObjectWrapper _ST_CredentialStore;
        private string _ST_CredentialStoreType;
        private List<IUIIdentifier> _credmgrs;

        public CredentialStoreSelector(IDependencyProvider provider)
        {
            // Accept only simple CredentialStoreKey types for now
            _credmgrs = new List<IUIIdentifier>(provider.GetObjects(nameof(ICredentialStore), typeof(CredentialStoreKey)));
        }
        /// <summary>
        /// Application automation services
        /// </summary>
        [JsonIgnore]
        [Browsable(false)]
        public IAppAutomation AppAutomation { get; set; }
        /// <summary>
        /// Data sheet properties
        /// </summary>
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
                        new PropertyEditorInfo() { EditorType = InputEditorType.BlockTitle, PropertyName = BTTL_CredentialMgr },
                        new PropertyEditorInfo() { EditorType = InputEditorType.FixedComboBox, PropertyName = nameof(ST_CredentialStoreType), Values = new List<object>(_credmgrs) },
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
        /// Credential store to use for storing and retrieving credentials.
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_CredentialStore), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_CredentialStore), typeof(UIResources))]
        public ObjectWrapper ST_CredentialStoreType
        {
            get
            {
                return _ST_CredentialStore;
            }
            set
            {
                if (_ST_CredentialStore != value)
                {
                    _ST_CredentialStore = value;
                    if (value != null)
                    {
                        ICredentialStore store = value.Implementation() as ICredentialStore;
                        AppConfigWriter.SetAppSetting(SETTING_CredentialStore, store.GetType().AssemblyQualifiedName);
                    }
                    else
                    {
                        AppConfigWriter.SetAppSetting(SETTING_CredentialStore, "");
                    }
                }
            }
        }
        /// <summary>
        /// Setup instructions
        /// </summary>
        [DILocalizedDisplayName(nameof(CAP_Instructions), typeof(UIResources))]
        public string Instructions
        {
            get
            {
                return SETUP_CredentialStore;
            }
            set
            {
            }
        }
    }
}
