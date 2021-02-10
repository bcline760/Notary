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
        public BasicCredentials()
        {

        }

        [JsonProperty("key", Required = Required.Always)]
        public string Key { get; set; }

        [JsonProperty("secret", Required = Required.Always)]
        public string Secret { get; set; }

        [JsonProperty("persistant", Required = Required.Always)]
        public bool Persistant { get; set; }
    }
}
