using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Notary.Contract
{
    [DataContract]
    public class CertificateRequest
    {
        public CertificateRequest()
        {
            SubjectAlternativeNames = new List<SubjectAlternativeName>();
        }

        /// <summary>
        /// Get or set the expiration length in hours.
        /// </summary>
        [DataMember]
        public int LengthInHours { get; set; }

        /// <summary>
        /// Get or set the X500 Distinguished Name for the certificate subject.
        /// </summary>
        [DataMember]
        public DistinguishedName Subject { get; set; }

        /// <summary>
        /// Get a list of SAN for the certificate
        /// </summary>
        [DataMember]
        public List<SubjectAlternativeName> SubjectAlternativeNames
        {
            get;
        }

        /// <summary>
        /// Get or set the account slug that requested this certificate
        /// </summary>
        [DataMember]
        public string RequestedBySlug { get; set; }
    }
}
