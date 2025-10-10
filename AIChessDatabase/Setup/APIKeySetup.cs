using AIAssistants.Interfaces;
using AIChessDatabase.Properties;
using GlobalCommonEntities.Interfaces;
using GlobalCommonEntities.UI;
using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Text.Json.Serialization;
using static AIChessDatabase.Properties.UIResources;

namespace AIChessDatabase.Setup
{
    public class APIKeySetup : UIDataSheet
    {
        private List<PropertyEditorInfo> _info;
        private IAPIManager _api;
        private ICredentialStore _credStore;
        private string _key;
        private string _value;
        public APIKeySetup(string name, string description, IAPIManager apim, ICredentialStore store)
        {
            APIManagerName = name + " (" + description + ")";
            _api = apim;
            _credStore = store;
            if (!string.IsNullOrEmpty(_api.AccountId))
            {
                KeyName = _api.AccountId;
            }
            PropertyChanged += OnPropertyChanged;
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
                        new PropertyEditorInfo() { EditorType = InputEditorType.BlockTitle, PropertyName = BTTL_APIKey },
                        new PropertyEditorInfo() { EditorType = InputEditorType.SingleLineText, PropertyName = nameof(APIManagerName), ReadOnly = true },
                        new PropertyEditorInfo() { EditorType = InputEditorType.SingleLineText, PropertyName = nameof(KeyName) },
                        new PropertyEditorInfo() { EditorType = InputEditorType.Password, PropertyName = nameof(KeyValue) },
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
        [DILocalizedDisplayName(nameof(NAME_APIManagerName), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_APIManagerName), typeof(UIResources))]
        public string APIManagerName { get; }
        /// <summary>
        /// Key name in the credential store
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_APIKeyName), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_APIKeyName), typeof(UIResources))]
        public string KeyName
        {
            get
            {
                return _key;
            }
            set
            {
                if (_key != value)
                {
                    _key = value;
                    try { _api.AccountId = value; } catch { }
                    if (!string.IsNullOrEmpty(_api.AccountId))
                    {
                        try
                        {
                            NetworkCredential cred = _credStore.GetCredentialFromStore(new CredentialStoreKey(_api.AccountId), out string error);
                            if (cred != null)
                            {
                                KeyValue = cred.Password;
                                InvokeRefreshEditor(new RefreshEditorEventArgs(Properties[3], 3));
                            }
                        }
                        catch
                        {
                        }
                        InvokePropertyChanged();
                    }
                }
            }
        }
        /// <summary>
        /// Key value in the credential store
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_APIKeyValue), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_APIKeyValue), typeof(UIResources))]
        public string KeyValue
        {
            get
            {
                return _value;
            }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    InvokePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Setup instructions
        /// </summary>
        [DILocalizedDisplayName(nameof(CAP_Instructions), typeof(UIResources))]
        public string Instructions { get { return SETUP_APIKey; } set { } }
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(KeyName) && !string.IsNullOrEmpty(KeyValue))
            {
                NetworkCredential cred = _credStore.GetCredentialFromStore(new CredentialStoreKey(KeyName), out string error);
                if (!string.IsNullOrEmpty(error) || (cred == null) || (cred.Password != KeyValue))
                {
                    _credStore.SaveCredentialToStore(new CredentialStoreKey(KeyName), new NetworkCredential(KeyName, KeyValue), out error);
                    if (!string.IsNullOrEmpty(error))
                    {
                        throw new Exception(error);
                    }
                    _api.AccountId = KeyName;
                    _api.UpdateConfiguration();
                }
            }
        }
    }
}