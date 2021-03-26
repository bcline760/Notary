using System;
using System.Collections.Generic;
using System.Text;

namespace Notary
{
    public static class Constants
    {
        #region Environment Variable constants
        /// <summary>
        /// Environment Variable for the Application Key
        /// </summary>
        public static readonly string ApplicationKeyEnvName = "NOTARY_APP_KEY";

        /// <summary>
        /// Environment Variable for the authentication type
        /// </summary>
        public static readonly string AuthenticationTypeEnvName = "NOTARY_AUTH_TYPE";
        
        /// <summary>
        /// Environment Variable for the database connection string
        /// </summary>
        public static readonly string DatabaseConnectionStringEnvName = "NOTARY_DB_CONN_STR";

        /// <summary>
        /// Environment Variable for the certificate authority parent directory
        /// </summary>
        public static readonly string DirectoryCaRootEnvName = "NOTARY_DIR_CA_ROOT";

        /// <summary>
        /// Environment Variable for the intermediate certificate directory
        /// </summary>
        public static readonly string DirectoryIntermediateCertificateEnvName = "NOTARY_DIR_INTERMEDIATE_CERTIFICATE";

        /// <summary>
        /// Environment Variable for the intermediate certificate key directory
        /// </summary>
        public static readonly string DirectoryIntermediatePrivateKeyEnvName = "NOTARY_DIR_INTERMEDIATE_PK";

        /// <summary>
        /// Environment Variable for the intermediate signing certificate request directory
        /// </summary>
        public static readonly string DirectoryIntermediateRequestEnvName = "NOTARY_DIR_INTERMEDIATE_REQUEST";
        public static readonly string DirectoryIssuedCertificateEnvName = "NOTARY_DIR_ISSUED_CERTIFICATE";
        public static readonly string DirectoryIssuedPrivateKeyEnvName = "NOTARY_DIR_ISSUED_PK";
        public static readonly string DirectoryIssuedRequestEnvName = "NOTARY_DIR_ISSUED_REQUEST";
        public static readonly string DirectoryRootCertificateEnvName = "NOTARY_DIR_ROOT_CERTIFICATE";
        public static readonly string DirectoryRootPrivateKeyEnvName = "NOTARY_DIR_ROOT_PK";
        public static readonly string DirectoryRootRequestEnvName = "NOTARY_DIR_ROOT_REQUEST";
        public static readonly string DirectoryUserKeyEnvName = "NOTARY_DIR_USER_KEY";
        public static readonly string LdapAdminGroupEnvName = "NOTARY_LDAP_ADMIN_GROUP";
        public static readonly string LdapCertAdminGroupEnvName = "NOTARY_LDAP_CERT_ADMIN_GROUP";
        public static readonly string LdapDomainEnvName = "NOTARY_LDAP_DOMAIN";
        public static readonly string LdapSearchEnvName = "NOTARY_LDAP_SEARCH";
        public static readonly string LdapServerEnvName = "NOTARY_LDAP_SERVER";
        public static readonly string ServiceAccountUserEnvName = "NOTARY_SERVICE_ACCOUNT_USER";
        public static readonly string ServiceAccountPassEnvName = "NOTARY_SERVICE_ACCOUNT_PASS";
        public static readonly string TokenAudienceEnvName = "NOTARY_TOKEN_AUDIENCE";
        public static readonly string TokenIssuerEnvName = "NOTARY_TOKEN_ISSUER";
        #endregion Environment Variable constants

        /// <summary>
        /// A key size of 1,024 bits
        /// </summary>
        public static readonly int KeySizeSmall = 1024;

        /// <summary>
        /// A key size of 2,048 bits
        /// </summary>
        public static readonly int KeySizeMedium = 2048;

        /// <summary>
        /// A key size of 4,096 bits
        /// </summary>
        public static readonly int KeySizeLarge = 4096;

        /// <summary>
        /// The number of iterations to use for password hashing. More = secure, but more = slower
        /// </summary>
        public static readonly int PasswordHashIterations = 4096;

        /// <summary>
        /// Number of bytes to use for password hashes
        /// </summary>
        public static readonly int PasswordHashLength = 32;

        /// <summary>
        /// Name of the JWT signing certificate
        /// </summary>
        public static string SigningCertificateName = "notary.pem";
    }
}
