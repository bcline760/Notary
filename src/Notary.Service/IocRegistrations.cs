using System;
using System.Collections.Generic;
using System.Text;

using Autofac;

using Notary.IOC;
using Notary.Interface.Service;

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
            builder.RegisterType<SessionService>()
                .As<ISessionService>()
                .InstancePerLifetimeScope();
        }
    }
}
