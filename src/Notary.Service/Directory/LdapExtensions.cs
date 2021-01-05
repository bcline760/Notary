using System;
using System.Collections.Generic;
using System.Text;

using Novell.Directory.Ldap;

namespace Notary.Service.Directory
{
    /// <summary>
    /// Extensions for LDAP functionality
    /// </summary>
    public static class LdapExtensions
    {
        /// <summary>
        /// Generate a DirectoryEntity object from LDAP attributes
        /// </summary>
        /// <param name="attributeSet">The set of LDAP attributes used to generate the model</param>
        /// <param name="distinguishedName">The distinguished name to use if LDAP attribute doesn't exist</param>
        /// <returns>A DirectoryEntity object modeled from LDAP attributes</returns>
        public static DirectoryEntity CreateEntityFromAttributes(this LdapAttributeSet attributeSet, string distinguishedName)
        {
            var ldapUser = new DirectoryEntity
            {
                ObjectSid = attributeSet.GetAttribute("objectSid")?.StringValue,
                ObjectGuid = attributeSet.GetAttribute("objectGUID")?.StringValue,
                ObjectCategory = attributeSet.GetAttribute("objectCategory")?.StringValue,
                ObjectClass = attributeSet.GetAttribute("objectClass")?.StringValue,
                MemberOf = attributeSet.GetAttribute("memberOf")?.StringValueArray,
                CommonName = attributeSet.GetAttribute("cn")?.StringValue,
                UserName = attributeSet.GetAttribute("name")?.StringValue,
                SamAccountName = attributeSet.GetAttribute("sAMAccountName")?.StringValue,
                UserPrincipalName = attributeSet.GetAttribute("userPrincipalName")?.StringValue,
                Name = attributeSet.GetAttribute("name")?.StringValue,
                DistinguishedName = attributeSet.GetAttribute("distinguishedName")?.StringValue ?? distinguishedName,
                DisplayName = attributeSet.GetAttribute("displayName")?.StringValue,
                FirstName = attributeSet.GetAttribute("givenName")?.StringValue,
                LastName = attributeSet.GetAttribute("sn")?.StringValue,
                Description = attributeSet.GetAttribute("description")?.StringValue,
                Phone = attributeSet.GetAttribute("telephoneNumber")?.StringValue,
                EmailAddress = attributeSet.GetAttribute("mail")?.StringValue,
                Address = new LdapAddress
                {
                    Street = attributeSet.GetAttribute("streetAddress")?.StringValue,
                    City = attributeSet.GetAttribute("l")?.StringValue,
                    PostalCode = attributeSet.GetAttribute("postalCode")?.StringValue,
                    StateName = attributeSet.GetAttribute("st")?.StringValue,
                    CountryName = attributeSet.GetAttribute("co")?.StringValue,
                    CountryCode = attributeSet.GetAttribute("c")?.StringValue
                },

                SamAccountType = int.Parse(attributeSet.GetAttribute("sAMAccountType")?.StringValue ?? "0")
            };

            return ldapUser;
        }
    }
}
