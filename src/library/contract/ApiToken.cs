using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Notary.Contract
{
    [DataContract]
    public class ApiToken : Entity
    {
        [DataMember]
        public string AccountSlug { get; set; }

        /// <summary>
        /// Get or set when the token expires
        /// </summary>
        [DataMember]
        public DateTime Expiry { get; set; }

        /// <summary>
        /// Get or set the token value
        /// </summary>
        /// <remarks>Use this property to retrieve or set the signed JWT</remarks>
        [DataMember]
        public string Token { get; set; }

        public override string[] SlugProperties()
        {
            return new[]
            {
                Guid.NewGuid().ToString().ToUpper()
            };
        }
    }
}