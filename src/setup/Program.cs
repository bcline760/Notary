using System;
using System.Threading.Tasks;

using Autofac;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

using Notary.Configuration;
using Notary.Contract;
using Notary.Interface;
using Notary.Interface.Service;
using Notary.Service;

namespace Notary.Setup
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true);
            var config = configBuilder.Build();

            var containerBuilder = new ContainerBuilder();
            var notaryConfig = config.GetSection("Notary").Get<NotaryConfiguration>();
            containerBuilder.RegisterInstance(notaryConfig).SingleInstance();

            RegisterModules.Register(containerBuilder);
            var container = containerBuilder.Build();
        }

        static async Task SetupDatabase(IContainer container)
        {
            var setup = container.Resolve<ICollectionSetup>();

            await setup.Setup();
        }

        static async Task SetupCertificateAuthority(IContainer container)
        {

        }
    }
}
