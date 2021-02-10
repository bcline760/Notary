using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

using log4net;

using Notary.Contract;
using Notary.Interface.Repository;
using Notary.Interface.Service;
using System.Linq;

namespace Notary.Service
{
    internal abstract class SessionService
    {
        public async Task<List<ApiToken>> GetSessions(string accountSlug, bool activeOnly)
        {
            var tokens = await Token.GetAccountTokens(accountSlug);

            if (activeOnly)
                tokens = tokens.Where(a => a.Active).ToList();

            return tokens;
        }

        protected async Task<ApiToken> GenerateToken(ICredentials credentials, Account user)
        {
            DateTime expiration = credentials.Persistant ? DateTime.MaxValue : DateTime.Now.AddHours(2);
            var identity = new ClaimsIdentity(new List<Claim>()
            {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.GivenName, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Expiration, expiration.ToString()),
                new Claim(ClaimTypes.IsPersistent, credentials.Persistant.ToString()),
                new Claim("slug", user.Slug),
                new Claim(ClaimTypes.Role, user.Roles.ToString()),
                new Claim(ClaimTypes.Hash, Encryption.Hash(DateTime.UtcNow.Ticks.ToString()))
            });


            var token = Encryption.GenerateJwt(identity, expiration);

            var apiToken = new ApiToken()
            {
                AccountSlug = user.Slug,
                Created = DateTime.UtcNow,
                CreatedBySlug = user.Slug,
                Active = true,
                Expiry = expiration,
                Token = token
            };

            //Save the token to the database
            await Token.SaveAsync(apiToken, user.CreatedBySlug);
            return apiToken;
        }

        protected ILog Log { get; set; }

        protected IAccountService Account { get; set; }

        protected IEncryptionService Encryption { get; set; }

        protected ITokenService Token { get; set; }

        protected NotaryConfiguration Configuration { get; set; }
    }
}
