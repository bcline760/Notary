using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Newtonsoft.Json;

namespace Notary.Contract
{
    /// <summary>
    /// A basic certificate object
    /// </summary>
    public class Certificate : Entity
    {

        [JsonProperty("algorithm", Required = Required.Always)]
        public Algorithm Algorithm
        {
            get; set;
        }

        /// <summary>
        /// Denotes the X509 Certificate common name
        /// </summary>
        [JsonProperty("issuer", Required = Required.Always)]
        public DistinguishedName Issuer
        {
            get; set;
        }

        [JsonProperty("keuUsage", Required = Required.Always)]
        public int KeyUsage
        {
            get; set;
        }

        /// <summary>
        /// Get or set whether this certificate act as the primary signing certificate.
        /// </summary>
        [JsonProperty("isPrimarySigning", Required = Required.Always)]
        public bool PrimarySigningCertificate { get; set; }

        /// <summary>
        /// The certificate is not valid before this given date.
        /// </summary>
        [JsonProperty("notBefore", Required = Required.Always)]
        public DateTime NotBefore
        {
            get; set;
        }

        /// <summary>
        /// The certificate is not valid after the given date
        /// </summary>
        [JsonProperty("notAfter", Required = Required.Always)]
        public DateTime NotAfter
        {
            get; set;
        }

        /// <summary>
        /// Get or set the date the certificate was issued
        /// </summary>
        [JsonProperty("revokeDate", Required = Required.Always)]
        public DateTime? RevocationDate
        {
            get; set;
        }

        [JsonProperty("sn", Required = Required.Always)]
        public string SerialNumber
        {
            get; set;
        }

        /// <summary>
        /// The signing certificate slug
        /// </summary>
        [JsonProperty("signingCertSlug", Required = Required.AllowNull)]
        public string SigningCertificateSlug { get; set; }

        [JsonProperty("sibject", Required = Required.Always)]
        public DistinguishedName Subject { get; set; }

        [JsonProperty("sanList", Required = Required.AllowNull)]
        public List<SubjectAlternativeName> SubjectAlternativeNames
        {
            get; set;
        }

        [JsonProperty("thumbprint", Required = Required.Always)]
        public string Thumbprint
        {
            get; set;
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