using System;
using System.Collections.Generic;
using System.Text;

using Autofac;
using AutoMapper;
using MongoDB.Driver;
using Notary.Contract;
using Notary.Data.Model;
using Notary.Data.Repository;
using Notary.Interface.Repository;
using Notary.IOC;

namespace Notary.Data
{
    public class IocRegistration : Module, IRegister
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(r =>
            {
                var mapConfig = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Certificate, CertificateModel>()
                        .ForMember(c => c.Algorithm, obj => obj.MapFrom(m => m.Algorithm.ToString()))
                        .ForMember(c => c.KeyUsage, obj => obj.MapFrom(m => (short)m.KeyUsage))
                        .ReverseMap();
                    cfg.CreateMap<Account, AccountModel>().ReverseMap();
                    cfg.CreateMap<AuthenticatedUser, TokenModel>().ReverseMap();
                    cfg.CreateMap<RevocatedCertificate, RevocatedCertificateModel>().ReverseMap();
                    cfg.CreateMap<Address, AddressModel>().ReverseMap();
                });

                var map = mapConfig.CreateMapper();
                return map;
            }).As<IMapper>().SingleInstance();

            builder.Register(r =>
            {
                var config = r.Resolve<NotaryConfiguration>();
                var connectionString = config.ConnectionString;

                var settings = MongoClientSettings.FromUrl(MongoUrl.Create(connectionString));

                IMongoClient client = new MongoClient(settings);
                IMongoDatabase db = client.GetDatabase("notary");
                return db;
            }).As<IMongoDatabase>().SingleInstance();

            builder.RegisterType<AccountRepository>().As<IAccountRepository>().InstancePerLifetimeScope();
            builder.RegisterType<CertificateRepository>().As<ICertificateRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RevocatedCertificateRepository>().As<IRevocatedCertificateRepository>().InstancePerLifetimeScope();
            builder.RegisterType<TokenRepository>().As<ITokenRepository>().InstancePerLifetimeScope();
        }
    }
}
