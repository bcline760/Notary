using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using MongoDB.Driver;

using Notary.Contract;
using Notary.Data.Model;
using Notary.Interface.Repository;

namespace Notary.Data.Repository
{
    public class TokenRepository : BaseRepository<AuthenticatedUser, TokenModel>, ITokenRepository
    {
        public TokenRepository(IMongoDatabase db, IMapper map) : base(db, map)
        {
        }

        public async Task<List<AuthenticatedUser>> GetAccountTokens(string accountSlug)
        {
            var filter = Builders<TokenModel>.Filter.Eq("aslug", accountSlug);
            var result = await Collection.FindAsync(filter);

            List<AuthenticatedUser> tokens = new List<AuthenticatedUser>();
            if (result.Any())
            {
                tokens.AddRange(result.ToList().Select(Mapper.Map<AuthenticatedUser>));
            }
            return tokens;
        }
    }
}
