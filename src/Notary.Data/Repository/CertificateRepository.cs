using Notary.Contract;
using Notary.Data.Model;
using Notary.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using MongoDB.Driver;

namespace Notary.Data.Repository
{
    public class CertificateRepository : BaseRepository<Certificate, CertificateModel>, ICertificateRepository
    {
        public CertificateRepository(IMongoDatabase db, IMapper map) : base(db, map)
        {
        }

        public async Task<Certificate> GetSigningCertificateAsync()
        {
            // In SQL: WHERE psc = 1 AND active = 1
            var pscFilter = Builders<CertificateModel>.Filter.Eq("psc", true);
            var activeFilter = Builders<CertificateModel>.Filter.Eq("active", true);
            var filter = Builders<CertificateModel>.Filter.And(pscFilter, activeFilter);

            //In SQL: FROM certificates
            var collection = await Collection.FindAsync(filter);
            if (collection.Any())
            {
                //In SQL: SELECT TOP 1 *
                var model = await collection.FirstAsync();

                Certificate cert = Mapper.Map<Certificate>(model);
                return cert;
            }

            return null;
        }
    }
}
