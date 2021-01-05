using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using log4net;

using Notary.Contract;
using Notary.Interface.Service;

namespace Notary.Service
{
    internal class SamlSessionService : SessionService, ISessionService
    {
        public SamlSessionService(
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
            throw new NotImplementedException();
        }

        public async Task SignoutAsync(string accountSlug)
        {
            throw new NotImplementedException();
        }
    }
}
