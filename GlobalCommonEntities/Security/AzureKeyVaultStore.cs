using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using GlobalCommonEntities.Attributes;
using GlobalCommonEntities.Interfaces;
using GlobalCommonEntities.Properties;
using GlobalCommonEntities.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Text.Json.Serialization;
using static GlobalCommonEntities.Properties.UIResources;

namespace GlobalCommonEntities.Security
{
    /// <summary>
    /// ICredentialStore to retrieve credentials from Azure Key Vault Store.
    /// </summary>
    public class AzureKeyVaultStore : ICredentialStore
    {
        /// <summary>
        /// ICredentialStore: Provides a suitable empty key to provide data to retrieve a credential.
        /// </summary>
        public Type KeyType { get { return typeof(AzureKeyVaultStoreKey); } }
        /// <summary>
        /// ICredentialStore: Get an empty CredentialStoreKey
        /// </summary>
        public CredentialStoreKey EmptyKey { get { return new AzureKeyVaultStoreKey(); } }
        /// <summary>
        /// Gets a credential from Azure Key Vault.
        /// </summary>
        /// <param name="key">
        /// Object identifying the credential to retrieve. It must be of type AzureKeyVaultStoreKey.
        /// </param>
        /// <param name="error">
        /// Error message if the credential cannot be retrieved, otherwise empty string.
        /// </param>
        /// <returns>
        /// NetworkCredential object containing the credential information in the password, or null if not found.
        /// </returns>
        public NetworkCredential GetCredentialFromStore(CredentialStoreKey key, out string error)
        {
            error = null;
            AzureKeyVaultStoreKey akey = key as AzureKeyVaultStoreKey;
            if (akey == null)
            {
                error = "Invalid key type. Expected AzureKeyVaultStoreKey.";
                return null;
            }
            if (string.IsNullOrWhiteSpace(akey.VaultUri))
            {
                throw new ArgumentException("The VaultUry property cannot be null.");
            }

            if (!Uri.TryCreate(akey.VaultUri, UriKind.Absolute, out var uri))
            {
                throw new ArgumentException("URI format not valid.");
            }

            if (uri.Scheme != Uri.UriSchemeHttps)
            {
                throw new ArgumentException("The Key Vault Uri must use HTTPS.");
            }
            try
            {
                SecretClient client = new SecretClient(uri, new DefaultAzureCredential());
                KeyVaultSecret secret = client.GetSecret(akey.SecretName);
                string value = secret.Value;
                return new NetworkCredential("", value);
            }
            catch (Exception ex)
            {
                error = $"Error getting secret: {ex.Message}";
                return null;
            }
        }
        /// <summary>
        /// ICredentialStore: Stores a credential in the given store.
        /// </summary>
        /// <param name="key">
        /// Credential store key to identify the credential.
        /// </param>
        /// <param name="credential">
        /// Credential value to store.
        /// </param>
        /// <param name="error">
        /// Error message if the credential cannot be stored, otherwise empty string.
        /// </param>
        /// <param name="overwrite">
        /// Flag to indicate if an existing credential should be overwritten.
        /// </param>
        /// <returns>
        /// True when the credential is stored, false otherwise.
        /// </returns>
        public bool SaveCredentialToStore(
            CredentialStoreKey key,
            NetworkCredential credential,
            out string error,
            bool overwrite = true)
        {
            error = "";

            var akey = key as AzureKeyVaultStoreKey;
            if (akey == null)
            {
                error = "Invalid key type. Expected AzureKeyVaultStoreKey.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(akey.SecretName))
            {
                error = "SecretName cannot be null or empty.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(akey.VaultUri))
            {
                error = "VaultUri cannot be null or empty.";
                return false;
            }
            if (!Uri.TryCreate(akey.VaultUri, UriKind.Absolute, out var uri) || uri.Scheme != Uri.UriSchemeHttps)
            {
                error = "The Key Vault Uri must be a valid HTTPS URI.";
                return false;
            }

            try
            {
                var client = new SecretClient(uri, new DefaultAzureCredential());

                if (!overwrite)
                {
                    try
                    {
                        // If exists, do not overwrite
                        _ = client.GetSecret(akey.SecretName);
                        error = "Secret already exists and overwrite=false.";
                        return false;
                    }
                    catch (Azure.RequestFailedException rex) when (rex.Status == 404)
                    {
                        // It does not exist, continue
                    }
                }

                string value = credential?.Password ?? string.Empty;

                var kvSecret = new KeyVaultSecret(akey.SecretName, value);
                kvSecret.Properties.ContentType = "text/plain";
                if (!string.IsNullOrWhiteSpace(credential?.UserName))
                {
                    // Optional metadata; GetCredentialFromStore uses only the password
                    kvSecret.Properties.Tags["username"] = credential.UserName;
                }

                client.SetSecret(kvSecret); // If it exists, it is overwritten

                return true;
            }
            catch (Exception ex)
            {
                error = $"Error saving secret: {ex.Message}";
                return false;
            }
        }
    }
    /// <summary>
    /// Sepcialized key for Azure Key Vault credentials.
    /// </summary>
    public class AzureKeyVaultStoreKey : CredentialStoreKey
    {
        public AzureKeyVaultStoreKey() : base() { }
        /// <summary>
        /// Create a key from a string with data.
        /// </summary>
        /// <param name="data">
        /// Semicolon separated string of key data components. 
        /// The first component is always the key name. 
        /// The second component is the secret name, and the third component is the vault URI.
        /// </param>
        public AzureKeyVaultStoreKey(string data) : base(data)
        {
            SecretName = data.Split(';')[1];
            VaultUri = data.Split(';')[2];
        }
        /// <summary>
        /// Set data from a semicolon separated string.
        /// </summary>
        /// <param name="data">
        /// Semicolon separated string of key data components. 
        /// The first component is always the key name. 
        /// The second component is the secret name, and the third component is the vault URI.
        /// </param>
        public override void SetData(string data)
        {
            if (data?.Split(';').Length >= 3)
            {
                base.SetData(data);
                SecretName = data?.Split(';')[1];
                VaultUri = data?.Split(';')[2];
            }
        }
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
                        new PropertyEditorInfo() { EditorType = InputEditorType.BlockTitle, PropertyName = BTTL_AzureKey },
                        new PropertyEditorInfo() { EditorType = InputEditorType.SingleLineText, PropertyName = nameof(SecretName), Required = true },
                        new PropertyEditorInfo() { EditorType = InputEditorType.SingleLineText, PropertyName = nameof(VaultUri), Required = true }
                    };
                }
                return _info;
            }
            set
            {
            }
        }
        /// <summary>
        /// The secret name in the Azure Key Vault.
        /// </summary>
        [JsonPropertyName("secret_name")]
        [GCELocalizedDisplayName(nameof(NAME_AzureKeyVaultStoreKey_SecretName), typeof(UIResources))]
        [GCELocalizedDescription(nameof(DESC_AzureKeyVaultStoreKey_SecretName), typeof(UIResources))]
        public string SecretName { get; set; }
        /// <summary>
        /// Vault URI where the secret is stored.
        /// </summary>
        [JsonPropertyName("vault_uri")]
        [GCELocalizedDisplayName(nameof(NAME_AzureKeyVaultStoreKey_VaultUri), typeof(UIResources))]
        [GCELocalizedDescription(nameof(DESC_AzureKeyVaultStoreKey_VaultUri), typeof(UIResources))]
        public string VaultUri { get; set; }
        public override string ToString()
        {
            return $"{SecretName};{VaultUri}";
        }
    }
}
