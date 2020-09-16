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
        /// <summary>
        /// Get or set the connection profile of the database
        /// </summary>
        public Database Database { get; set; }

        /// <summary>
        /// Get or set the file system for issued certificates.
        /// </summary>
        public CertificatePath Issued { get; set; }

        public string EncryptionKey { get; set; }

        /// <summary>
        /// Get or set the intermediate file system structure
        /// </summary>
        public CertificatePath Intermediate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CertificatePath Root { get; set; }

        public string UserKeyPath { get; set; }
    }

    /// <summary>
    /// Represents credentials used to connect to a data store
    /// </summary>
    public class Credentials
    {
        /// <summary>
        /// Get or set the username component of the credentials
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The password of the credentials
        /// </summary>
        public string Password { get; set; }
    }

    /// <summary>
    /// Represents a database connection
    /// </summary>
    public class Database
    {

        /// <summary>
        /// Get or set the credentials used for the database
        /// </summary>
        public Credentials Credentials { get; set; }

        /// <summary>
        /// Get or set the name of the database 
        /// </summary>
        public string DatabaseName { get; set; }

        /// <summary>
        /// Get or set the host server of the database instance
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Get or set the port used for the database instance
        /// </summary>
        public int Port { get; set; }

    }

    public class CertificatePath
    {
        public string CertificateDirectory { get; set; }

        public string CertificateRequestDirectory { get; set; }

        public string PrivateKeyDirectory { get; set; }
    }
}
