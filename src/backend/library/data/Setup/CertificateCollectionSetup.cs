using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Driver;

using Notary.Data.Model;
using Notary.Interface;

namespace Notary.Data.Setup
{
    public class CertificateCollectionSetup : ICertificateCollectionSetup
    {
        public CertificateCollectionSetup(IMongoDatabase db)
        {
            Database = db;
        }

        public async Task Setup()
        {
            var collection = Database.GetCollection<CertificateModel>("certificates");
            if (collection != null)
            {
                await Database.DropCollectionAsync("certificates");
            }

            await Database.CreateCollectionAsync("certificates");
            collection = Database.GetCollection<CertificateModel>("certificates");

            var indexBuilder = new IndexKeysDefinitionBuilder<CertificateModel>()
                .Ascending(i => i.Active)
                .Ascending(i => i.Created)
                .Ascending(i => i.SerialNumber)
                .Ascending(i => i.Slug)
                .Ascending(i => i.Thumbprint);

            var model = new CreateIndexModel<CertificateModel>(indexBuilder);
            await collection.Indexes.CreateOneAsync(model);
        }

        public IMongoDatabase Database { get; }
    }
}
