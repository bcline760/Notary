using System;
using System.Collections.Generic;
using System.Text;

namespace Notary.Configuration
{
    public class NotaryCaConfiguration
    {
        public Algorithm CaKeyAlgorithm { get; set; }

        public string CaName { get; set; }

        public string IssuedCertificatePath { get; set; }

        public int KeySize { get; set; }

        public string RootCertificatePath { get; set; }

        public string SigningCertificatePath { get; set; }
    }
}
