using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Driver;

using Notary.Data.Model;
using Notary.Interface;

namespace Notary.Data.Setup
{
    public class RevocatedCertificateCollectionSetup : IRevocatedCertificateCollectionSetup
    {
        public RevocatedCertificateCollectionSetup(IMongoDatabase db)
        {
            Database = db;
        }

        public async Task Setup()
        {
            var collection = Database.GetCollection<RevocatedCertificateModel>("revocated_certificate");
            if (collection != null)
            {
                await Database.DropCollectionAsync("revocated_certificates");
            }

            await Database.CreateCollectionAsync("revocated_certificates");
            collection = Database.GetCollection<RevocatedCertificateModel>("revocated_certificates");

            var indexBuilder = new IndexKeysDefinitionBuilder<RevocatedCertificateModel>()
                .Ascending(i => i.Active)
                .Ascending(i => i.Created)
                .Ascending(i => i.SerialNumber)
                .Ascending(i => i.Slug);

            var model = new CreateIndexModel<RevocatedCertificateModel>(indexBuilder);
            await collection.Indexes.CreateOneAsync(model);
        }
        public IMongoDatabase Database { get; }
    }
}
