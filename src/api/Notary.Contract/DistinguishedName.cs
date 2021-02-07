using System.Runtime.Serialization;
using System.Text;

using Newtonsoft.Json;

namespace Notary.Contract
{
    /// <summary>
    /// Represents a X509.3 Distingished Name
    /// </summary>
    public class DistinguishedName
    {
        [JsonProperty("cn", Required = Required.Always)]
        public string CommonName
        {
            get => default;
            set
            {
            }
        }


        [JsonProperty("c", Required = Required.AllowNull)]
        public string Country
        {
            get => default;
            set
            {
            }
        }

        [JsonProperty("l", Required = Required.AllowNull)]
        public string Locale
        {
            get => default;
            set
            {
            }
        }

        [JsonProperty("o", Required = Required.AllowNull)]
        public string Organization
        {
            get => default;
            set
            {
            }
        }

        [JsonProperty("ou", Required = Required.AllowNull)]
        public string OrganizationalUnit
        {
            get => default;
            set
            {
            }
        }

        [JsonProperty("s", Required = Required.AllowNull)]
        public string StateProvince
        {
            get => default;
            set
            {
            }
        }

        /// <summary>
        /// Generate a distinguished name string from parameters
        /// </summary>
        /// <param name="issuer"></param>
        /// <returns></returns>
        public static string BuildDistinguishedName(DistinguishedName issuer)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(issuer.CommonName))
                sb.AppendFormat("CN={0},", issuer.CommonName);
            if (!string.IsNullOrWhiteSpace(issuer.Country))
                sb.AppendFormat("C={0},", issuer.Country);
            if (!string.IsNullOrWhiteSpace(issuer.Locale))
                sb.AppendFormat("L={0},", issuer.Locale);
            if (!string.IsNullOrWhiteSpace(issuer.Organization))
                sb.AppendFormat("O={0},", issuer.Organization);
            if (!string.IsNullOrWhiteSpace(issuer.OrganizationalUnit))
                sb.AppendFormat("OU={0},", issuer.OrganizationalUnit);
            if (!string.IsNullOrWhiteSpace(issuer.StateProvince))
                sb.AppendFormat("ST={0},", issuer.StateProvince);

            return sb.ToString().Trim(',');
        }

        /// <summary>
        /// Makes a DN string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return BuildDistinguishedName(this);
        }
    }
}