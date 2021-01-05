using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Notary
{
    /// <summary>
    /// Defines the configuration used for this application
    /// </summary>
    public class NotaryConfiguration
    {
        public AuthenticationProvider Authentication { get; set; }

        /// <summary>
        /// Get or set the connection profile of the database
        /// </summary>
        public string ConnectionString { get; set; }

        public DirectorySettings DirectorySettings { get; set; }

        public string EncryptionKey { get; set; }

        /// <summary>
        /// Get or set the intermediate file system structure
        /// </summary>
        public CertificatePath Intermediate { get; set; }

        /// <summary>
        /// Get or set the file system for issued certificates.
        /// </summary>
        public CertificatePath Issued { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CertificatePath Root { get; set; }

        public TokenSettings TokenSettings { get; set; }

        public string UserKeyPath { get; set; }
    }

    public class CertificatePath
    {
        public string CertificateDirectory { get; set; }

        public string CertificateRequestDirectory { get; set; }

        public string PrivateKeyDirectory { get; set; }
    }

    public class DirectorySettings
    {
        public string AdminGroupName { get; set; }

        public string CertificateAdminGroupName { get; set; }

        public string Domain { get; set; }

        public string SearchBase { get; set; }

        public string ServerName { get; set; }

        public string ServiceAccountPassword { get; set; }

        public string ServiceAccountUser { get; set; }
    }

    public class TokenSettings
    {
        public string Audience { get; set; }

        public string Issuer { get; set; }

        public string SigningCertificatePath { get; set; }
    }
}
