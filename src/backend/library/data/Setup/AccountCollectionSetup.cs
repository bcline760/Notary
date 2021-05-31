using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Driver;

using Notary.Interface;
using Notary.Data.Model;

namespace Notary.Data.Setup
{
    public class AccountCollectionSetup : IAccountCollectionSetup
    {
        public AccountCollectionSetup(IMongoDatabase db)
        {
            Database = db;
        }

        public async Task Setup()
        {
            var collection = Database.GetCollection<AccountModel>("account");
            if (collection != null)
            {
                // Drop the collection...this will blow away everything!
                await Database.DropCollectionAsync("account");
            }

            var createOptions = new CreateCollectionOptions<AccountModel>()
            {
                Capped = false,
                Collation = Collation.Simple,
                UsePowerOf2Sizes = true,
                ValidationAction = DocumentValidationAction.Error,
                ValidationLevel = DocumentValidationLevel.Strict
            };
            await Database.CreateCollectionAsync("account", createOptions);
            collection = Database.GetCollection<AccountModel>("account");

            var indexKeyBuilder = new IndexKeysDefinitionBuilder<AccountModel>()
                .Ascending(i => i.Email)
                .Ascending(i => i.Username)
                .Ascending(i => i.Active)
                .Ascending(i => i.FirstName)
                .Ascending(i => i.LastName)
                .Ascending(i=>i.Slug);

            var indexes = new List<CreateIndexModel<AccountModel>>()
            {
                new CreateIndexModel<AccountModel>(indexKeyBuilder,new CreateIndexOptions() { Name = "AccountIndex" })
            };

            await collection.Indexes.CreateManyAsync(indexes);
        }

        public IMongoDatabase Database { get; }
    }
}
