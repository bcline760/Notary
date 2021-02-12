﻿using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Notary.Data.Model
{
    public sealed class RevocatedCertificateModel : BaseModel
    {
        public RevocatedCertificateModel()
        {
        }

        /// <summary>
        /// Get or set the reason the certificate was revoked
        /// </summary>
        [BsonElement("reason"), BsonRequired]
        public RevocationReason Reason { get; set; }
        /// <summary>
        /// Get or set the revoked certificate thumbprint
        /// </summary>
        [BsonElement("sn"), BsonRequired]
        public string SerialNumber { get; set; }

        /// <summary>
        /// Get or set the revoked certificate SHA-1 thumbprint
        /// </summary>
        [BsonElement("thumb"), BsonRequired]
        public string Thumbprint { get; set; }
    }
}
