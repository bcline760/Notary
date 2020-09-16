using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Notary.Contract
{
    /// <summary>
    /// A basic certificate object
    /// </summary>
    [DataContract()]
    public class Certificate : Entity
    {

        [DataMember]
        public Algorithm Algorithm
        {
            get => default;
            set
            {
            }
        }

        /// <summary>
        /// Denotes the X509 Certificate common name
        /// </summary>
        [DataMember]
        public DistinguishedName Issuer
        {
            get => default;
            set
            {
            }
        }

        [DataMember]
        public int KeyUsage
        {
            get; set;
        }

        /// <summary>
        /// Get or set whether this certificate act as the primary signing certificate.
        /// </summary>
        [DataMember]
        public bool PrimarySigningCertificate { get; set; }

        /// <summary>
        /// The certificate is not valid before this given date.
        /// </summary>
        [DataMember]
        public DateTime NotBefore
        {
            get => default;
            set
            {
            }
        }

        /// <summary>
        /// The certificate is not valid after the given date
        /// </summary>
        [DataMember]
        public DateTime NotAfter
        {
            get => default;
            set
            {
            }
        }

        /// <summary>
        /// Get or set the date the certificate was issued
        /// </summary>
        [DataMember]
        public DateTime? RevocationDate
        {
            get => default;
            set
            {
            }
        }

        [DataMember]
        public string SerialNumber
        {
            get => default;
            set
            {
            }
        }

        /// <summary>
        /// The signing certificate slug
        /// </summary>
        [DataMember]
        public string SigningCertificateSlug { get; set; }

        [DataMember]
        public DistinguishedName Subject { get; set; }

        [DataMember]
        public List<SubjectAlternativeName> SubjectAlternativeNames
        {
            get => default;
            set
            {
            }
        }

        [DataMember]
        public string Thumbprint
        {
            get => default;
            set
            {
            }
        }


        public override string[] SlugProperties()
        {
            return new[]
            {
                Thumbprint
            };
        }
    }
}