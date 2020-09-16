using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Notary.Contract
{
    [DataContract]
    public class Account : Entity
    {
        [DataMember]
        public Address AccountAddress { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string PublicKey { get; set; }

        [DataMember]
        public Roles Roles { get; set; }

        [DataMember]
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
