using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using log4net;

using Notary.Contract;
using Notary.Interface.Repository;
using Notary.Interface.Service;

namespace Notary.Service
{
    internal class SystemSessionService : SessionService, ISessionService
    { 
        public SystemSessionService(
            ILog log,
            IAccountService accountRepository,
            ITokenService tokenRepository,
            IEncryptionService encryptionService,
            NotaryConfiguration notaryConfiguration)
        {
            Configuration = notaryConfiguration;
            Log = log;
            Account = accountRepository;
            Encryption = encryptionService;
            Token = tokenRepository;
        }

        public async Task<ApiToken> SignInAsync(ICredentials credentials)
        {
            return await AuthenticateUsernamePassword(credentials);
        }

        public async Task SignoutAsync(string accountSlug)
        {
            throw new NotImplementedException();
        }

        protected async Task<ApiToken> AuthenticateUsernamePassword(ICredentials credentials)
        {
            var user = await Account.GetByEmailAsync(credentials.Key);

            if (user == null)
                return null;

            byte[] givenPassword = Encryption.GeneratePasswordHash(credentials.Secret);
            byte[] userPassword = Encryption.GeneratePasswordHash(user.Password);

            if (!givenPassword.AreBytesEqual(userPassword))
            {
                return null;
            }

            ApiToken apiToken = await GenerateToken(credentials, user);

            return apiToken;
        }
    }
}
