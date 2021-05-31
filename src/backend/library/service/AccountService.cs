
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using log4net;

using Notary.Configuration;
using Notary.Contract;
using Notary.Interface.Repository;
using Notary.Interface.Service;
using Notary.Logging;

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;

namespace Notary.Service
{
    public class AccountService : EntityService<Account>, IAccountService
    {
        protected IEncryptionService EncService { get; }

        protected NotaryConfiguration Configuration { get; }

        public AccountService(
            IAccountRepository repository,
            IEncryptionService encryption,
            ILog log,
            NotaryConfiguration notaryConfiguration) : base(repository, log)
        {
            EncService = encryption;
            Configuration = notaryConfiguration;
        }

        public async Task<Account> GetByEmailAsync(string email)
        {
            var repository = Repository as IAccountRepository;
            return await repository.GetByEmailAddressAsync(email);
        }

        public async Task RegisterAccountAsync(Account account)
        {
            var existingAccount = await Repository.GetAsync(account.Slug);
            if (existingAccount != null)
                throw new NotaryException($"{account.Slug} already exists.");

            try
            {
                //Hash the password
                if (!string.IsNullOrWhiteSpace(account.Password))
                {
                    byte[] passwordHash = EncService.GeneratePasswordHash(account.Password);
                    account.Password = Convert.ToBase64String(passwordHash);
                }

                await SaveAsync(account, null);
            }
            catch (CryptographicException cex)
            {
                throw cex.IfNotLoggedThenLog(Logger);
            }
            catch (Exception ex)
            {
                throw ex.IfNotLoggedThenLog(Logger);
            }
        }
    }
}
