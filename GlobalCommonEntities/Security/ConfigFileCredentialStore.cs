using GlobalCommonEntities.Interfaces;
using System;
using System.Configuration;
using System.Net;

namespace GlobalCommonEntities.Security
{
    /// <summary>
    /// ICredentialStore to retrieve credentials from the AppSettings section of an App.config or Web.config file.
    /// </summary>
    public class ConfigFileCredentialStore : ICredentialStore
    {
        /// <summary>
        /// ICredentialStore: Provides a suitable empty key to provide data to retrieve a credential.
        /// </summary>
        public Type KeyType { get { return typeof(CredentialStoreKey); } }
        /// <summary>
        /// ICredentialStore: Get an empty CredentialStoreKey
        /// </summary>
        public CredentialStoreKey EmptyKey { get { return new CredentialStoreKey(); } }
        /// <summary>
        /// Gets a credential from a configuration file.
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
        public NetworkCredential GetCredentialFromStore(CredentialStoreKey key, out string error)
        {
            error = "";
            string credential = ConfigurationManager.AppSettings[key.Name];
            if (string.IsNullOrEmpty(credential))
            {
                error = "Credential not found in configuration file.";
                return null;
            }
            return new NetworkCredential(key.Name, credential);
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

            if (key == null || string.IsNullOrWhiteSpace(key.Name))
            {
                error = "Key is null or empty.";
                return false;
            }
            if (credential == null)
            {
                error = "Credential is null.";
                return false;
            }

            string secret = credential.Password ?? string.Empty;

            try
            {
                // Open App.config of executable
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = config.AppSettings.Settings;

                if (settings[key.Name] != null)
                {
                    if (!overwrite)
                    {
                        error = "Credential already exists and overwrite=false.";
                        return false;
                    }
                    settings[key.Name].Value = secret;
                }
                else
                {
                    settings.Add(key.Name, secret);
                }

                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
                return true;
            }
            catch (ConfigurationErrorsException ex)
            {
                error = ex.Message;
                return false;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }
    }
}
