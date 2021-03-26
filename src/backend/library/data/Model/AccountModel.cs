using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Notary.Data.Model
{
    public class AccountModel : BaseModel
    {
        [BsonElement("accountAddress")]
        public AddressModel AccountAddress { get; set; }

        [BsonElement("email"), BsonRequired]
        public string Email { get; set; }

        [BsonElement("fname"), BsonRequired]
        public string FirstName { get; set; }

        [BsonElement("lname"), BsonRequired]
        public string LastName { get; set; }

        [BsonElement("password"), BsonRequired]
        public string Password { get; set; }

        [BsonElement("pkey")]
        public string PublicKey { get; set; }

        [BsonElement("roles"), BsonRequired]
        public Roles Roles { get; set; }

        [BsonElement("username"), BsonRequired]
        public string Username { get; set; }
    }

    public class AddressModel
    {
        /// <summary>
        /// Get or set the primary address line which is the address and street name
        /// </summary>
        [BsonElement("addressLine")]
        public string AddressLine { get; set; }

        /// <summary>
        /// Get or set the city of residence
        /// </summary>
        [BsonElement("city")]
        public string City { get; set; }

        /// <summary>
        /// Get or set the country of residence
        /// </summary>
        [BsonElement("country")]
        public string Country { get; set; }

        /// <summary>
        /// Get or set the postal code
        /// </summary>
        [BsonElement("pCode")]
        public string PostalCode { get; set; }

        /// <summary>
        /// Get or set the second address line which is usually the unit number
        /// </summary>
        [BsonElement("addressLine2")]
        public string SecondAddressLine { get; set; }

        /// <summary>
        /// Get or set the state or province within the country of residence
        /// </summary>
        [BsonElement("state")]
        public string StateProvince { get; set; }

        /// <summary>
        /// Get or set the third address line which is usually an "attention" line.
        /// </summary>
        [BsonElement("addressLine3")]
        public string ThirdAddressLine { get; set; }
    }
}
