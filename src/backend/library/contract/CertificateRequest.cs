using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Newtonsoft.Json;

namespace Notary.Contract
{
    [DataContract]
    public class CertificateRequest
    {
        public CertificateRequest()
        {
        }

        /// <summary>
        /// Get or set the expiration length in hours.
        /// </summary>
        [JsonProperty("expireLength", Required = Required.Always)]
        public int LengthInHours { get; set; }

        [JsonProperty("password", Required = Required.Always)]
        public string CertificatePassword { get; set; }

        /// <summary>
        /// Get or set the X500 Distinguished Name for the certificate subject.
        /// </summary>
        [JsonProperty("subject", Required = Required.Always)]
        public DistinguishedName Subject { get; set; }

        /// <summary>
        /// Get a list of SAN for the certificate
        /// </summary>
        [JsonProperty("sanList", Required = Required.AllowNull)]
        public List<SubjectAlternativeName> SubjectAlternativeNames
        {
            get; set;
        }

        /// <summary>
        /// Get or set the account slug that requested this certificate
        /// </summary>
        [JsonProperty("reqeustedBy", Required = Required.Always)]
        public string RequestedBySlug { get; set; }
    }
}
