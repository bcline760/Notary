using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Newtonsoft.Json;

namespace Notary.Contract
{
    public class Account : Entity
    {
        [JsonProperty("address", Required = Required.Always)]
        public Address AccountAddress { get; set; }

        [JsonProperty("email", Required = Required.Always)]
        public string Email { get; set; }

        [JsonProperty("fname", Required = Required.AllowNull)]
        public string FirstName { get; set; }

        [JsonProperty("lname", Required = Required.AllowNull)]
        public string LastName { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

        [JsonProperty("pKey", Required = Required.Always)]
        public string PublicKey { get; set; }

        [JsonProperty("roles", Required = Required.Always)]
        public Roles Roles { get; set; }

        [JsonProperty("username", Required = Required.Always)]
        public string Username { get; set; }

        /// <summary>
        /// The properties used to make the slug
        /// </summary>
        /// <returns>An array of slug properties</returns>
        public override string[] SlugProperties()
        {
            return new string[]
            {
                LastName,
                Guid.NewGuid().ToString().ToLowerInvariant().FirstEight()
            };
        }
    }
}
