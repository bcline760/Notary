using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace Notary.Contract
{
    public class AuthenticatedUser : Entity
    {
        [JsonProperty("acctSlug", Required = Required.Always)]
        public string AccountSlug { get; set; }

        /// <summary>
        /// Get or set when the token expires
        /// </summary>
        [JsonProperty("expiry", Required = Required.Always)]
        public DateTime Expiry { get; set; }

        /// <summary>
        /// The first name of the authenticated user
        /// </summary>
        [JsonProperty("fName", Required = Required.Always)]
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the authenticated user
        /// </summary>
        [JsonProperty("lName", Required = Required.Always)]
        public string LastName { get; set; }

        /// <summary>
        /// The login of the user
        /// </summary>
        [JsonProperty("login", Required = Required.AllowNull)]
        public string Login { get; set; }

        /// <summary>
        /// The role allocated to the user
        /// </summary>
        [JsonProperty("role", Required = Required.Always)]
        public Roles Role { get; set; }

        /// <summary>
        /// Get or set the token value
        /// </summary>
        /// <remarks>Use this property to retrieve or set the signed JWT</remarks>
        [JsonProperty("token", Required = Required.Always)]
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