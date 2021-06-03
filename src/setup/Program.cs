using System;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

using Autofac;

using Microsoft.Extensions.Configuration;

using Notary.Configuration;
using Notary.Contract;
using Notary.Interface;
using Notary.Interface.Service;
using Notary.Service;
using System.Text;
using log4net;

namespace Notary.Setup
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var configBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true);
            var config = configBuilder.Build();

            var containerBuilder = new ContainerBuilder();
            var notaryConfig = config.GetSection("Notary").Get<NotaryConfiguration>();
            containerBuilder.RegisterInstance(notaryConfig).SingleInstance();
            containerBuilder.Register(r => LogManager.GetLogger(typeof(Program))).As<ILog>().SingleInstance();

            RegisterModules.Register(containerBuilder);
            var container = containerBuilder.Build();

            try
            {
                Console.WriteLine("Setting up the database...💾");
                await SetupDatabase(container);
                Console.WriteLine("Done.");

                Console.WriteLine("Setting up admin account...👤");
                string generatedPassworde = await SetupAdminAccount(container);
                Console.WriteLine("Done");

                Console.WriteLine("Setting up the certificate authority...🔒");
                await SetupCertificateAuthority(container);
                Console.WriteLine("Done");

                Console.WriteLine("All done! 🙌🏼");
                Console.WriteLine("The password for the admin account is {0}", generatedPassworde);
            }
            catch (Exception ex)
            {
                await Console.Error.WriteLineAsync(ex.Message);
                await Console.Error.WriteLineAsync(ex.StackTrace);
            }
        }

        static async Task SetupDatabase(IContainer container)
        {
            //var setup = container.Resolve<ICollectionSetup>();

            var accountSetup = container.Resolve<IAccountCollectionSetup>();
            var tokenSetup = container.Resolve<ITokenCollectionSetup>();
            var certificateSetup = container.Resolve<ICertificateCollectionSetup>();
            var revocatedSetup = container.Resolve<IRevocatedCertificateCollectionSetup>();

            await accountSetup.Setup();

            await tokenSetup.Setup();

            await certificateSetup.Setup();

            await revocatedSetup.Setup();
        }

        static async Task<string> SetupAdminAccount(IContainer container)
        {
            var passwordBytes = new byte[1];
            var sb = new StringBuilder();

            const int passwordLength = 16;
            int counter = 0;

            string generatedPassword = null;
            using (var rng = RandomNumberGenerator.Create())
            {
                while (true)
                {
                    rng.GetNonZeroBytes(passwordBytes);
                    //var integer = BitConverter.ToUInt16(passwordBytes);

                    if ((passwordBytes[0] >= 33 && passwordBytes[0] <= 126) && counter < passwordLength)
                    {
                        sb.Append((char)passwordBytes[0]);
                        counter++;
                    }
                    else if (counter >= passwordLength)
                    {
                        generatedPassword = sb.ToString();

                        var service = container.Resolve<IAccountService>();

                        var newAccount = new Account
                        {
                            Active = true,
                            Created = DateTime.UtcNow,
                            CreatedBySlug = "setup",
                            Email = "admin@notary",
                            FirstName = "Notary",
                            LastName = "Administrator",
                            Password = generatedPassword,
                            Roles = Roles.Admin,
                            Username = "admin"
                        };

                        await service.SaveAsync(newAccount, "setup");
                        break;
                    }
                }
            }

            return generatedPassword;
        }

        static async Task SetupCertificateAuthority(IContainer container)
        {
            var svc = container.Resolve<ICertificateService>();
            var config = container.Resolve<NotaryConfiguration>();

            var rootCn = $"{config.CertificateAuthority.CaName} Root Certificate";
            var signingCn = $"{config.CertificateAuthority.CaName} Signing Certificate";

            // Create directories
            if (!Directory.Exists(config.CertificateAuthority.RootCertificatePath))
            {
                Directory.CreateDirectory(config.CertificateAuthority.RootCertificatePath);
                string keyPath = $"{config.CertificateAuthority.RootCertificatePath}/{Constants.KeyDirectoryPath}/";
                string certPath = $"{config.CertificateAuthority.RootCertificatePath}/{Constants.CertificateDirectoryPath}/";
                Directory.CreateDirectory(keyPath);
                Directory.CreateDirectory(certPath);
            }
            if (!Directory.Exists(config.CertificateAuthority.SigningCertificatePath))
            {
                Directory.CreateDirectory(config.CertificateAuthority.SigningCertificatePath);
                string keyPath = $"{config.CertificateAuthority.SigningCertificatePath}/{Constants.KeyDirectoryPath}/";
                string certPath = $"{config.CertificateAuthority.SigningCertificatePath}/{Constants.CertificateDirectoryPath}/";
                Directory.CreateDirectory(keyPath);
                Directory.CreateDirectory(certPath);
            }
            if (!Directory.Exists(config.CertificateAuthority.IssuedCertificatePath))
            {
                Directory.CreateDirectory(config.CertificateAuthority.IssuedCertificatePath);
                string keyPath = $"{config.CertificateAuthority.IssuedCertificatePath}/{Constants.KeyDirectoryPath}/";
                string certPath = $"{config.CertificateAuthority.IssuedCertificatePath}/{Constants.CertificateDirectoryPath}/";
                Directory.CreateDirectory(keyPath);
                Directory.CreateDirectory(certPath);
            }

            var caSetup = new CertificateAuthoritySetup
            {
                KeyLength = config.CertificateAuthority.KeySize,
                Requestor = "setup",
                LengthInYears = 10,
                RootDn = new DistinguishedName
                {
                    CommonName = rootCn
                },
                SigningDn = new DistinguishedName
                {
                    CommonName = signingCn
                }
            };

            await svc.GenerateCaCertificates(caSetup);
        }
    }
}
