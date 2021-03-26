using System;
using Notary.Contract;
using Notary.Data.Model;
using Notary.Interface.Repository;

using MongoDB.Driver;
using AutoMapper;

namespace Notary.Data.Repository
{
    public class RevocatedCertificateRepository : BaseRepository<RevocatedCertificate, RevocatedCertificateModel>, IRevocatedCertificateRepository
    {
        public RevocatedCertificateRepository(IMongoDatabase db, IMapper map) : base(db, map)
        {
        }
    }
}
