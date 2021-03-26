using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using log4net;
using Novell.Directory.Ldap;

using Notary.Logging;
using Notary.Contract;
using Notary.Interface.Repository;
using Notary.Interface.Service;
using Notary.Service.Directory;

namespace Notary.Service
{
    internal class LdapSessionService : SessionService, ISessionService, IDisposable
    {
        private readonly string[] _attributes = {
        "objectSid", "objectGUID", "objectCategory", "objectClass", "memberOf", "name", "cn", "distinguishedName",
        "sAMAccountName", "sAMAccountName", "userPrincipalName", "displayName", "givenName", "sn", "description",
        "telephoneNumber", "mail", "streetAddress", "postalCode", "l", "st", "co", "c"
        };
        private bool disposedValue;

        public LdapSessionService(
            ILog log,
            IAccountService accountRepository,
            ITokenService tokenService,
            IEncryptionService encryptionService,
            NotaryConfiguration notaryConfiguration)
        {
            Configuration = notaryConfiguration;
            Log = log;
            Account = accountRepository;
            Encryption = encryptionService;
            Token = tokenService;

            Connection = GetConnection();
        }

        public async Task<AuthenticatedUser> SignInAsync(ICredentials credentials)
        {
            string dn = $"{credentials.Key}@{Configuration.DirectorySettings.Domain}";

            try
            {
                Connection.Bind(dn, credentials.Secret);

                var entity = GetDirectoryUser(credentials.Key);
                if (entity == null)
                    throw new InvalidOperationException("NULL directory user found");

                var account = await Account.GetByEmailAsync(entity.Email);
                if (account == null)
                {
                    var newAccount = new Account
                    {
                        AccountAddress = new Address
                        {
                            AddressLine = entity.Address.StateName,
                            City = entity.Address.City,
                            Country = entity.Address.CountryName,
                            PostalCode = entity.Address.PostalCode,
                            StateProvince = entity.Address.StateName
                        },
                        Active = true,
                        Created = DateTime.UtcNow,
                        Email = entity.Email,
                        FirstName = entity.FirstName,
                        LastName = entity.LastName,
                        Roles = GetRole(entity.MemberOf),
                        Username = entity.SamAccountName
                    };

                    await Account.RegisterAccountAsync(newAccount);
                    account = newAccount;
                }

                //Sign out any orphaned tokens.
                await SignoutAsync(account.Slug);
                AuthenticatedUser token = await GenerateToken(credentials, account);
                return token;
            }
            catch (Exception ex)
            {
                ex.IfNotLoggedThenLog(Log);
                return null;
            }
        }

        private Roles GetRole(string[] memberOf)
        {
            foreach (var group in memberOf)
            {
                if (group.Contains(Configuration.DirectorySettings.AdminGroupName))
                    return Roles.Admin;
                else if (group.Contains(Configuration.DirectorySettings.CertificateAdminGroupName)) ;
                    return Roles.CertificateAdmin;
            }

            return Roles.User;
        }

        private LdapConnection GetConnection()
        {
            var connection = new LdapConnection()
            {
                SecureSocketLayer = false
            };

            connection.Connect(Configuration.DirectorySettings.ServerName, LdapConnection.DefaultPort);

            return connection;
        }

        private DirectoryEntity GetDirectoryUser(string username)
        {
            var filter = $"(&(objectClass=user)(samAccountName={username}))";
            var searchResults = Connection.Search(Configuration.DirectorySettings.SearchBase, LdapConnection.ScopeSub, filter, _attributes, false);

            DirectoryEntity user = null;

            while (searchResults.HasMore())
            {
                LdapEntry entry = searchResults.Next();
                user = entry.GetAttributeSet().CreateEntityFromAttributes(entry.Dn);
            }

            return user;
        }

        private LdapConnection Connection { get; }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (Connection !=null)
                    {
                        Connection.Disconnect();
                        Connection.Dispose();
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
