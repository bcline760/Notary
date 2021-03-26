using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

namespace Notary.Contract
{
    public sealed class CertificateAuthoritySetup
    {
        [JsonProperty("root_dn", Required = Required.Always)]
        public DistinguishedName RootDn { get; set; }

        [JsonProperty("intermediate_dn", Required = Required.Always)]
        public DistinguishedName IntermediateDn { get; set; }

        [JsonProperty("key_length", Required = Required.Always)]
        public int KeyLength { get; set; }

        [JsonProperty("length", Required = Required.Always)]
        public int LengthInYears { get; set; }

        [JsonIgnore]
        public string Requestor { get; set; }
    }
}
