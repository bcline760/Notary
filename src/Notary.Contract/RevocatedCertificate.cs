﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Notary.Contract
{
    /// <summary>
    /// A contract for revoked certificate records
    /// </summary>
    [DataContract]
    public class RevocatedCertificate : Entity
    {
        /// <summary>
        /// Get or set the reason the certificate was revoked
        /// </summary>
        [DataMember]
        public RevocationReason Reason { get; set; }
        /// <summary>
        /// Get or set the revoked certificate thumbprint
        /// </summary>
        [DataMember]
        public string SerialNumber { get; set; }

        /// <summary>
        /// Get or set the revoked certificate SHA-1 thumbprint
        /// </summary>
        [DataMember]
        public string Thumbprint { get; set; }

        public override string[] SlugProperties()
        {
            throw new NotImplementedException();
        }
    }
}
