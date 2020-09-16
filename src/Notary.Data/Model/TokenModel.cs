using System;
using System.Collections.Generic;
using System.Text;

using MongoDB.Bson.Serialization.Attributes;

namespace Notary.Data.Model
{
    public class TokenModel : BaseModel
    {
        [BsonElement("aslug")]
        public string AccountSlug { get; set; }

        /// <summary>
        /// Get or set when the token expires
        /// </summary>
        [BsonElement("expiry")]
        public DateTime Expiry { get; set; }

        /// <summary>
        /// Get or set the token value
        /// </summary>
        /// <remarks>Use this property to retrieve or set the signed JWT</remarks>
        [BsonElement("token")]
        public string Token { get; set; }
    }
}
