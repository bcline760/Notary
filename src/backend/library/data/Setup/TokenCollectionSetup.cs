using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Driver;

using Notary.Data.Model;
using Notary.Interface;

namespace Notary.Data.Setup
{
    public class TokenCollectionSetup : ITokenCollectionSetup
    {
        public TokenCollectionSetup(IMongoDatabase db)
        {
            Database = db;
        }

        public async Task Setup()
        {
            var collection = Database.GetCollection<TokenModel>("tokens");
            if (collection != null)
            {
                await Database.DropCollectionAsync("tokens");
            }

            await Database.CreateCollectionAsync("tokens");
            collection = Database.GetCollection<TokenModel>("tokens");

            var indexBuilder = new IndexKeysDefinitionBuilder<TokenModel>()
                .Ascending(i => i.Active)
                .Ascending(i => i.Created)
                .Ascending(i => i.AccountSlug)
                .Ascending(i => i.Slug)
                .Ascending(i => i.Expiry);

            var model = new CreateIndexModel<TokenModel>(indexBuilder);
            await collection.Indexes.CreateOneAsync(model);
        }

        public IMongoDatabase Database { get; }
    }
}
