using GlobalCommonEntities.Interfaces;
using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace GlobalCommonEntities.Security
{
    /// <summary>
    /// Manage keys from Windows Credential Manager
    /// </summary>
    public class WindowsCredentialManager : ICredentialStore
    {
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool CredRead(string target, CRED_TYPE type, int reservedFlag, out IntPtr credentialPtr);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int FormatMessage(int dwFlags, IntPtr lpSource, int dwMessageId, int dwLanguageId, System.Text.StringBuilder lpBuffer, int nSize, IntPtr Arguments);

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool CredWrite([In] ref CREDENTIAL userCredential, [In] uint flags);

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool CredDelete(string target, CRED_TYPE type, uint flags);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern void CredFree(IntPtr buffer);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct CREDENTIAL
        {
            public uint Flags;
            public CRED_TYPE Type;
            public IntPtr TargetName;
            public IntPtr Comment;
            public System.Runtime.InteropServices.ComTypes.FILETIME LastWritten;
            public uint CredentialBlobSize;
            public IntPtr CredentialBlob;
            public uint Persist;
            public uint AttributeCount;
            public IntPtr Attributes;
            public IntPtr TargetAlias;
            public IntPtr UserName;
        }
        private enum CRED_TYPE : uint
        {
            GENERIC = 1,
            DOMAIN_PASSWORD = 2,
            DOMAIN_CERTIFICATE = 3,
            DOMAIN_VISIBLE_PASSWORD = 4,
            GENERIC_CERTIFICATE = 5,
            DOMAIN_EXTENDED = 6,
            MAXIMUM = 7,      // Maximum supported cred type
            MAXIMUM_EX = 1000, // Maximum supported cred type extended
        }
        private enum CRED_PERSIST : uint
        {
            Session = 1,
            LocalMachine = 2,
            Enterprise = 3
        }

        /// <summary>
        /// ICredentialStore: Provides a suitable empty key to provide data to retrieve a credential.
        /// </summary>
        public Type KeyType { get { return typeof(CredentialStoreKey); } }
        /// <summary>
        /// ICredentialStore: Get an empty CredentialStoreKey
        /// </summary>
        public CredentialStoreKey EmptyKey { get { return new CredentialStoreKey(); } }
        /// <summary>
        /// ICredentialStore: Gets a credential from Windows Credential Manager store.
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
            IntPtr credPtr;
            bool result = CredRead(key.Name, CRED_TYPE.GENERIC, 0, out credPtr);
            if (!result)
            {
                int errorCode = Marshal.GetLastWin32Error();
                error = GetErrorMessage(errorCode);
                return null;
            }
            try
            {
                var credential = (CREDENTIAL)Marshal.PtrToStructure(credPtr, typeof(CREDENTIAL));
                string password = Marshal.PtrToStringUni(credential.CredentialBlob, (int)credential.CredentialBlobSize / 2);
                string username = Marshal.PtrToStringUni(credential.UserName);
                return new NetworkCredential(username, password);
            }
            finally
            {
                CredFree(credPtr);
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
            CRED_PERSIST persist = CRED_PERSIST.Enterprise;
            error = "";

            // Secret Blob in bytes (UTF-16LE)
            byte[] secretBytes = Encoding.Unicode.GetBytes(credential.Password ?? string.Empty);

            // Allocate memory for string / blob flieds
            IntPtr targetNamePtr = IntPtr.Zero;
            IntPtr userNamePtr = IntPtr.Zero;
            IntPtr commentPtr = IntPtr.Zero;
            IntPtr credentialBlobPtr = IntPtr.Zero;

            try
            {
                targetNamePtr = Marshal.StringToCoTaskMemUni(key.Name);
                userNamePtr = Marshal.StringToCoTaskMemUni(credential.UserName ?? string.Empty);
                commentPtr = IntPtr.Zero; // You can store a comment if needed

                credentialBlobPtr = Marshal.AllocCoTaskMem(secretBytes.Length);
                Marshal.Copy(secretBytes, 0, credentialBlobPtr, secretBytes.Length);

                var cred = new CREDENTIAL
                {
                    Flags = 0,
                    Type = CRED_TYPE.GENERIC,
                    TargetName = targetNamePtr,
                    Comment = commentPtr,
                    LastWritten = default,
                    CredentialBlobSize = (uint)secretBytes.Length, // in bytes
                    CredentialBlob = credentialBlobPtr,
                    Persist = (uint)persist,
                    AttributeCount = 0,
                    Attributes = IntPtr.Zero,
                    TargetAlias = IntPtr.Zero,
                    UserName = userNamePtr
                };

                // Try to write the credential
                if (CredWrite(ref cred, 0))
                    return true;

                int err = Marshal.GetLastWin32Error();

                // If it already exists and overwrite is allowed, delete and try again
                const int ERROR_ALREADY_EXISTS = 183;
                if (err == ERROR_ALREADY_EXISTS && overwrite)
                {
                    if (!CredDelete(key.Name, CRED_TYPE.GENERIC, 0))
                    {
                        error = GetErrorMessage(Marshal.GetLastWin32Error());
                        return false;
                    }
                    if (CredWrite(ref cred, 0))
                        return true;

                    err = Marshal.GetLastWin32Error();
                }

                error = GetErrorMessage(err);
                return false;
            }
            finally
            {
                if (targetNamePtr != IntPtr.Zero) Marshal.FreeCoTaskMem(targetNamePtr);
                if (userNamePtr != IntPtr.Zero) Marshal.FreeCoTaskMem(userNamePtr);
                if (commentPtr != IntPtr.Zero) Marshal.FreeCoTaskMem(commentPtr);
                if (credentialBlobPtr != IntPtr.Zero) Marshal.FreeCoTaskMem(credentialBlobPtr);
            }
        }
        private string GetErrorMessage(int errorCode)
        {
            const int FORMAT_MESSAGE_IGNORE_INSERTS = 0x00000200;
            const int FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000;

            var messageBuffer = new StringBuilder(512);
            int size = FormatMessage(
                FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS,
                IntPtr.Zero, errorCode, 0, messageBuffer, messageBuffer.Capacity, IntPtr.Zero);

            if (size == 0)
            {
                return $"Unknown error (code {errorCode})";
            }
            return messageBuffer.ToString().Trim();
        }
    }
}
