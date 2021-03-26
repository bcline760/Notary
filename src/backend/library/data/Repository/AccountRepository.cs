using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using MongoDB.Driver;

using Notary.Contract;
using Notary.Data.Model;
using Notary.Interface.Repository;

namespace Notary.Data.Repository
{
    public class AccountRepository : BaseRepository<Account, AccountModel>, IAccountRepository
    {
        public AccountRepository(IMongoDatabase db, IMapper map) : base(db, map)
        {
        }

        public async Task<Account> GetByEmailAddressAsync(string email)
        {
            var filter = Builders<AccountModel>.Filter.Eq("email", email);

            return await RunQuery(filter);
        }
    }
}
