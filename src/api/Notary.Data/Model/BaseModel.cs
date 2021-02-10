using System;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Notary.Data.Model
{
    [BsonIgnoreExtraElements]
    [BsonDiscriminator(RootClass = true)]
    [BsonKnownTypes(
        typeof(CertificateModel),
        typeof(AccountModel),
        typeof(RevocatedCertificateModel),
        typeof(TokenModel))]
    public abstract class BaseModel
    {
        [BsonIgnoreIfDefault,BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public ObjectId Id { get; set; }

        [BsonElement, BsonRequired]
        public string Slug { get; set; }

        [BsonElement, BsonRequired]
        public DateTime Created { get; set; }

        [BsonElement, BsonRequired]
        public string CreatedBySlug { get; set; }

        [BsonElement]
        public DateTime? Updated { get; set; }

        [BsonElement]
        public string UpdatedBySlug { get; set; }

        [BsonElement, BsonRequired]
        public bool Active { get; set; }
    }
}
