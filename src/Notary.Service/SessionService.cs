using System;
using System.Collections.Generic;
using System.Text;
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
    public class SessionService : ISessionService
    {
        public SessionService(
            ILog log,
            IAccountRepository accountRepository,
            ITokenRepository tokenRepository,
            IEncryptionService encryptionService)
        {
            Log = log;
            Account = accountRepository;
            Encryption = encryptionService;
            Token = tokenRepository;
        }

        public async Task<ApiToken> SignInAsync(ICredentials credentials)
        {
            var user = await Account.GetByEmailAddressAsync(credentials.Key);

            if (user != null)
            {
                byte[] givenPassword = Encryption.GeneratePasswordHash(credentials.Secret);
                byte[] userPassword = Encryption.GeneratePasswordHash(user.Password);

                if (!givenPassword.AreBytesEqual(userPassword))
                {
                    return null;
                }

                var identity = new ClaimsIdentity(new List<Claim>()
                {
                    new Claim(ClaimTypes.Email,credentials.Key),
                    new Claim(ClaimTypes.GivenName, $"{user.FirstName} {user.LastName}"),
                    new Claim(ClaimTypes.AuthenticationMethod, "JWT"),
                    new Claim(ClaimTypes.Expiration, credentials.Expire.ToString()),
                    new Claim(ClaimTypes.IsPersistent, (credentials.Expire == DateTime.MaxValue).ToString()),
                    new Claim(ClaimTypes.Authentication,"true"),
                    new Claim("slug", user.Slug),
                    new Claim(ClaimTypes.Role, user.Roles.ToString())
                });

                var certificate = X509Certificate.CreateFromCertFile("notary.cer");
                var token = Encryption.GenerateJwt(identity, credentials.Expire, (X509Certificate2)certificate, "", "");

                var apiToken = new ApiToken()
                {
                    AccountSlug = user.Slug,
                    Created = DateTime.UtcNow,
                    CreatedBySlug = user.Slug,
                    Active = true,
                    Expiry = credentials.Expire,
                    Token = token
                };

                //Save the token to the database
                await Token.SaveAsync(apiToken);

                return apiToken;
            }

            return null;
        }

        public async Task SignoutAsync(string accountSlug)
        {

        }

        public async Task<List<ApiToken>> GetSessions(string accountSlug, bool activeOnly)
        {
            var tokens = await Token.GetAccountTokens(accountSlug);

            if (activeOnly)
                tokens = tokens.Where(a => a.Active).ToList();

            return tokens;
        }

        protected ILog Log { get; }

        protected IAccountRepository Account { get; }

        protected IEncryptionService Encryption { get; }

        protected ITokenRepository Token { get; }
    }
}
