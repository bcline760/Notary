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
    public class TokenRepository : BaseRepository<ApiToken, TokenModel>, ITokenRepository
    {
        public TokenRepository(IMongoDatabase db, IMapper map) : base(db, map)
        {
        }

        public async Task<List<ApiToken>> GetAccountTokens(string accountSlug)
        {
            var filter = Builders<TokenModel>.Filter.Eq("aslug", accountSlug);
            var result = await Collection.FindAsync(filter);

            List<ApiToken> tokens = new List<ApiToken>();
            if (result.Any())
            {
                tokens.AddRange(result.ToList().Select(Mapper.Map<ApiToken>));
            }
            return tokens;
        }
    }
}
