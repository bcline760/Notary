using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Security.Claims;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

using Autofac;

using Notary.Contract;
using Notary.IOC;
using Autofac.Core;
using Notary.Service;
using log4net;
using log4net.Repository.Hierarchy;

namespace Notary.Api
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            IConfigurationBuilder builder = null;

            string appSettingsFile = env.IsDevelopment() ? "appsettings.Development.json" : "appsettings.json";

            builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile(appSettingsFile, false, true)
                .AddEnvironmentVariables();

            this.Configuration = builder.Build();
            IsDevelopment = env.IsDevelopment();
        }

        public IConfiguration Configuration { get; private set; }

        public ILifetimeScope AutofacContainer { get; private set; }

        public bool IsDevelopment { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddCors(o =>
            {
                o.AddPolicy("CorsPolicy", b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = !IsDevelopment,
                    ValidateIssuer = !IsDevelopment,
                    ValidateTokenReplay = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new X509SecurityKey(new X509Certificate2("notary.cer"))
                };
            });
            services.AddAuthorization(o =>
            {
                o.AddPolicy("Admin", p => p.RequireClaim(ClaimTypes.Role, "Admin"));
                o.AddPolicy("User", p => p.RequireClaim(ClaimTypes.Role, "User"));
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            var config = Configuration.Get<NotaryConfiguration>();
            builder.RegisterInstance(config).SingleInstance();

            builder.Register(r => LogManager.GetLogger(typeof(Startup))).As<ILog>().SingleInstance();
            RegisterModules.Register(builder);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
