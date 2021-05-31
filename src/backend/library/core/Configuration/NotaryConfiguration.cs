using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Notary.Configuration
{
    /// <summary>
    /// Defines the configuration used for this application
    /// </summary>
    public class NotaryConfiguration
    {
        public NotaryActiveDirectoryConfiguration ActiveDirectory { get; set; }

        public string ApplicationKey { get; set; }

        public AuthenticationProvider Authentication { get; set; }

        public NotaryCaConfiguration CertificateAuthority { get; set; }

        public NotaryDatabaseConfiguration Database { get; set; }

        public NotaryTokenSettingsConfiguration TokenSettings { get; set; }
    }
}
