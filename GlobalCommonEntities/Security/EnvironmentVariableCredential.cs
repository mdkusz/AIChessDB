using GlobalCommonEntities.Interfaces;
using System;
using System.Net;

namespace GlobalCommonEntities.Security
{
    /// <summary>
    /// ICredentialStore to retrieve credentials from an environment variable.
    /// </summary>
    public class EnvironmentVariableCredential : ICredentialStore
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
        /// Gets a credential from an environment variable.
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
            string password = Environment.GetEnvironmentVariable(key.Name);
            if (string.IsNullOrEmpty(password))
            {
                error = "Credential not found in environment variables.";
                return null;
            }
            return new NetworkCredential(key.Name, password);
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

            // Variable name validation
            string varName = key.Name.Trim();
            if (varName.Contains("="))
            {
                error = "Environment variable name cannot contain '='.";
                return false;
            }

            string secret = credential.Password ?? string.Empty;

            try
            {
                // Chexk if the variable already exists in any scope
                bool exists =
                    Environment.GetEnvironmentVariable(varName, EnvironmentVariableTarget.User) != null ||
                    Environment.GetEnvironmentVariable(varName, EnvironmentVariableTarget.Machine) != null ||
                    Environment.GetEnvironmentVariable(varName, EnvironmentVariableTarget.Process) != null;

                if (exists && !overwrite)
                {
                    error = "Environment variable already exists and overwrite=false.";
                    return false;
                }

                // Persistent in User scope
                Environment.SetEnvironmentVariable(varName, secret, EnvironmentVariableTarget.User);

                // Update in the current process scope
                Environment.SetEnvironmentVariable(varName, secret, EnvironmentVariableTarget.Process);

                return true;
            }
            catch (ArgumentException ex)
            {
                error = ex.Message;
                return false;
            }
            catch (System.Security.SecurityException ex)
            {
                // Only for Machine scope
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
