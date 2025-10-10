using GlobalCommonEntities.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Text.Json.Serialization;

namespace GlobalCommonEntities.Interfaces
{
    /// <summary>
    /// Implements a way to get credentials from a store.
    /// </summary>
    public interface ICredentialStore
    {
        /// <summary>
        /// Type of credential store key to be used.
        /// </summary>
        Type KeyType { get; }
        /// <summary>
        /// Get an empty CredentialStoreKey
        /// </summary>
        CredentialStoreKey EmptyKey { get; }
        /// <summary>
        /// Gets a credential from a given store.
        /// </summary>
        /// <param name="key">
        /// Object identifying the credential to retrieve.
        /// </param>
        /// <param name="error">
        /// Error message if the credential cannot be retrieved, otherwise empty string.
        /// </param>
        /// <returns>
        /// NetworkCredential object containing the credential information in the password, or null if not found.
        /// </returns>
        NetworkCredential GetCredentialFromStore(CredentialStoreKey key, out string error);
        /// <summary>
        /// Stores a credential in the given store.
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
        bool SaveCredentialToStore(
            CredentialStoreKey key,
            NetworkCredential credential,
            out string error,
            bool overwrite = true);
    }
    /// <summary>
    /// Base class for keys used to identify credentials in a store.
    /// </summary>
    /// <remarks>
    /// Store those objects as JSON objects in your configuration files or databases.
    /// </remarks>
    public class CredentialStoreKey : UIDataSheet
    {
        protected List<PropertyEditorInfo> _info;
        public CredentialStoreKey()
        {
        }
        /// <summary>
        /// Create a key from a string with data.
        /// </summary>
        /// <param name="data">
        /// Semicolon separated string of key data components. The first component is always the key name.
        /// </param>
        public CredentialStoreKey(string data)
        {
            Name = data.Split(';')[0];
        }
        /// <summary>
        /// Set data from a string.
        /// </summary>
        /// <param name="data">
        /// Data string to parse.
        /// </param>
        public virtual void SetData(string data)
        {
            Name = data?.Split(';')[0];
        }
        [JsonIgnore]
        [Browsable(false)]
        public override List<PropertyEditorInfo> Properties
        {
            get
            {
                return null;
            }
            set
            {
            }
        }
        /// <summary>
        /// Credential store key name. 
        /// </summary>
        [JsonPropertyName("key_name")]
        public string Name { get; set; }
    }
}
