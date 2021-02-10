using System;
using System.Collections.Generic;
using System.Text;

using Autofac;

using Notary.IOC;
using Notary.Interface.Service;
using log4net;

namespace Notary.Service
{
    public class IocRegistrations : Module, IRegister
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AccountService>()
                .As<IAccountService>()
                .InstancePerLifetimeScope();
            builder.RegisterType<CertificateService>()
                .As<ICertificateService>()
                .InstancePerLifetimeScope();
            builder.RegisterType<EncryptionService>()
                .As<IEncryptionService>()
                .InstancePerLifetimeScope();
            builder.RegisterType<TokenService>()
                .As<ITokenService>()
                .InstancePerLifetimeScope();

            builder.Register(r =>
            {
                var config = r.Resolve<NotaryConfiguration>();

                ISessionService session = null;
                switch (config.Authentication)
                {
                    case AuthenticationProvider.System:
                        session = new SystemSessionService(
                            r.Resolve<ILog>(),
                            r.Resolve<IAccountService>(),
                            null,
                            r.Resolve<IEncryptionService>(),
                            config
                        );
                        break;
                    case AuthenticationProvider.ActiveDirectory:
                        session = new LdapSessionService(
                            r.Resolve<ILog>(),
                            r.Resolve<IAccountService>(),
                            r.Resolve<ITokenService>(),
                            r.Resolve<IEncryptionService>(),
                            r.Resolve<NotaryConfiguration>()
                        );
                        break;
                }
                return session;
            }).As<ISessionService>().InstancePerLifetimeScope();
        }
    }
}
