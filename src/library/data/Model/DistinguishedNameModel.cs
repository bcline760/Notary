using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notary.Data.Model
{
    [BsonIgnoreExtraElements]
    public class DistinguishedNameModel
    {
        [BsonElement("CN"), BsonIgnoreIfDefault, BsonIgnoreIfNull]
        public int CommonName
        {
            get => default;
            set
            {
            }
        }

        [BsonElement("C"), BsonIgnoreIfDefault, BsonIgnoreIfNull]
        public string Country
        {
            get => default;
            set
            {
            }
        }

        [BsonElement("L"), BsonIgnoreIfDefault, BsonIgnoreIfNull]
        public string Locale
        {
            get => default;
            set
            {
            }
        }

        [BsonElement("O"), BsonIgnoreIfDefault, BsonIgnoreIfNull]
        public int Organization
        {
            get => default;
            set
            {
            }
        }

        [BsonElement("OU"), BsonIgnoreIfDefault, BsonIgnoreIfNull]
        public string OrganizationalUnit
        {
            get => default;
            set
            {
            }
        }

        [BsonElement("S"), BsonIgnoreIfDefault, BsonIgnoreIfNull]
        public int StateProvince
        {
            get => default;
            set
            {
            }
        }
    }
}
