using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Newtonsoft.Json;

namespace Notary.Contract
{
    /// <summary>
    /// Defines basic credentials of username and password
    /// </summary>
    public class BasicCredentials : ICredentials
    {
        /// <summary>
        /// Construct a new basic credentials object for a username/password combination
        /// </summary>
        /// <param name="username">The username of the credentials</param>
        /// <param name="password">The password of the credentials</param>
        /// <param name="persistent">Set to whether the credentials expire or not.</param>
        public BasicCredentials(string username, string password, bool persistent)
        {
            Key = username;
            Secret = password;

            //TODO: Not hardcode hours
            Expire = persistent ? DateTime.MaxValue : DateTime.UtcNow.AddHours(2);
        }

        [JsonProperty("key", Required = Required.Always)]
        public string Key { get; }

        [JsonProperty("secret", Required = Required.Always)]
        public string Secret { get; }

        [JsonProperty("expire", Required = Required.Always)]
        public DateTime Expire { get; }
    }
}
