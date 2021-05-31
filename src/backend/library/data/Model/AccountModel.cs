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

        [BsonElement("role"), BsonRequired]
        public Roles Role { get; set; }

        [BsonElement("username"), BsonRequired]
        public string Username { get; set; }
    }
}
