using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notary.Data.Model
{
    public sealed class CertificateModel : BaseModel
    {
        [BsonElement("alg"), BsonRequired]
        public string Algorithm { get; set; }

        [BsonElement("iss"), BsonRequired]
        public DistinguishedNameModel Issuer { get; set; }

        [BsonElement("keyUsage"), BsonRequired]
        public short KeyUsage { get; set; }

        [BsonElement("psc"), BsonRequired]
        public bool PrimarySigningCertificate { get; set; }

        [BsonElement("nb"), BsonRequired]
        public DateTime NotBefore { get; set; }

        [BsonElement("na"), BsonRequired]
        public DateTime NotAfter { get; set; }

        [BsonElement("rd"), BsonIgnoreIfDefault, BsonIgnoreIfNull]
        public DateTime? RevocationDate { get; set; }

        [BsonElement("sn"), BsonRequired]
        public string SerialNumber { get; set; }

        [BsonElement("sub"), BsonRequired]
        public DistinguishedNameModel Subject { get; set; }

        [BsonElement("san"), BsonIgnoreIfDefault, BsonIgnoreIfNull]
        public List<SanModel> SubjectAlternativeNames { get; set; }

        [BsonElement("thumb"), BsonRequired]
        public string Thumbprint { get; set; }
    }
}
