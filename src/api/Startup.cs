using System;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

using Autofac;
using log4net;
using Newtonsoft.Json;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using Notary.Service;

namespace Notary.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            HostEnvironment = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
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
                    ValidateAudience = HostEnvironment.IsProduction(),
                    ValidateIssuer = HostEnvironment.IsProduction(),
                    ValidateTokenReplay = HostEnvironment.IsProduction(),
                    ValidateIssuerSigningKey = HostEnvironment.IsProduction(),
                    IssuerSigningKey = new SymmetricSecurityKey(LoadEncryptionKey()),
                    ValidAudience = Environment.GetEnvironmentVariable(Constants.TokenAudienceEnvName),
                    ValidIssuer = Environment.GetEnvironmentVariable(Constants.TokenIssuerEnvName)
                };

                x.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = async (AuthenticationFailedContext arg) =>
                    {
                        Console.WriteLine(arg.Exception.Message);
                    },
                    OnTokenValidated = async (TokenValidatedContext arg) =>
                    {
                    }
                };
            });
            services.AddAuthorization(o =>
            {
                o.AddPolicy("Admin", p => p.RequireClaim(ClaimTypes.Role, "Admin"));
                o.AddPolicy("User", p => p.RequireClaim(ClaimTypes.Role, "User"));
            });
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Notary.Api", Version = "v1" });
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            //Environment.SetEnvironmentVariable('','',EnvironmentVariableTarget.User)
            var config = new NotaryConfiguration
            {
                ApplicationKey = Environment.GetEnvironmentVariable(Constants.ApplicationKeyEnvName),
                Authentication = (AuthenticationProvider)Enum.Parse(typeof(AuthenticationProvider), Environment.GetEnvironmentVariable(Constants.AuthenticationTypeEnvName)),
                ConnectionString = Environment.GetEnvironmentVariable(Constants.DatabaseConnectionStringEnvName),
                DirectorySettings = new DirectorySettings
                {
                    AdminGroupName = Environment.GetEnvironmentVariable(Constants.LdapAdminGroupEnvName),
                    CertificateAdminGroupName = Environment.GetEnvironmentVariable(Constants.LdapCertAdminGroupEnvName),
                    Domain = Environment.GetEnvironmentVariable(Constants.LdapDomainEnvName),
                    SearchBase = Environment.GetEnvironmentVariable(Constants.LdapSearchEnvName),
                    ServerName = Environment.GetEnvironmentVariable(Constants.LdapServerEnvName)
                },
                Hashing = ConfigureHashing(),
                Intermediate = new CertificatePath
                {
                    CertificateDirectory = Environment.GetEnvironmentVariable(Constants.DirectoryIntermediateCertificateEnvName),
                    CertificateRequestDirectory = Environment.GetEnvironmentVariable(Constants.DirectoryIntermediateRequestEnvName),
                    PrivateKeyDirectory = Environment.GetEnvironmentVariable(Constants.DirectoryIntermediatePrivateKeyEnvName)
                },
                Issued = new CertificatePath
                {
                    CertificateDirectory = Environment.GetEnvironmentVariable(Constants.DirectoryIssuedCertificateEnvName),
                    CertificateRequestDirectory = Environment.GetEnvironmentVariable(Constants.DirectoryIssuedRequestEnvName),
                    PrivateKeyDirectory = Environment.GetEnvironmentVariable(Constants.DirectoryIssuedPrivateKeyEnvName)
                },
                Root = new CertificatePath
                {
                    CertificateDirectory = Environment.GetEnvironmentVariable(Constants.DirectoryRootCertificateEnvName),
                    CertificateRequestDirectory = Environment.GetEnvironmentVariable(Constants.DirectoryRootRequestEnvName),
                    PrivateKeyDirectory = Environment.GetEnvironmentVariable(Constants.DirectoryRootPrivateKeyEnvName)
                },
                RootDirectory = Environment.GetEnvironmentVariable(Constants.DirectoryCaRootEnvName),
                ServiceAccountPassword = Environment.GetEnvironmentVariable(Constants.ServiceAccountPassEnvName),
                ServiceAccountUser = Environment.GetEnvironmentVariable(Constants.ServiceAccountUserEnvName),
                TokenSettings = new TokenSettings
                {
                    Audience = Environment.GetEnvironmentVariable(Constants.TokenAudienceEnvName),
                    Issuer = Environment.GetEnvironmentVariable(Constants.TokenIssuerEnvName)
                },
                UserKeyPath = Environment.GetEnvironmentVariable(Constants.DirectoryUserKeyEnvName)
            };

            builder.RegisterInstance(config).SingleInstance();

            builder.Register(r => LogManager.GetLogger(typeof(Startup))).As<ILog>().SingleInstance();
            RegisterModules.Register(builder);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (HostEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Notary.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private Hashing ConfigureHashing()
        {
            Hashing hash = null;
            if (File.Exists(".hashing"))
            {
                string rawText = null;
                using (TextReader tr = File.OpenText(".hashing"))
                {
                    rawText = tr.ReadToEnd();
                }

                string json = Encoding.Default.GetString(Convert.FromBase64String(rawText));
                hash = JsonConvert.DeserializeObject<Hashing>(json);
            }
            else
            {
                var provider = RandomNumberGenerator.Create();
                byte[] rngBytes = new byte[32];
                provider.GetNonZeroBytes(rngBytes);

                hash = new Hashing
                {
                    Iterations = Constants.PasswordHashIterations,
                    Length = Constants.PasswordHashLength,
                    Salt = Convert.ToBase64String(rngBytes)
                };
                string json = JsonConvert.SerializeObject(hash);
                string b64Json = Convert.ToBase64String(Encoding.Default.GetBytes(json));

                using (TextWriter tw = new StreamWriter(File.OpenWrite(".hashing")))
                {
                    tw.Write(b64Json);
                }
            }

            return hash;
        }

        //TODO: Figure out how to refactor this properly
        private byte[] LoadEncryptionKey()
        {
            byte[] encryptionKey = null;
            string path = $"{Environment.CurrentDirectory}/notary.key";
            if (File.Exists("notary.key"))
            {
                using (FileStream fs = File.OpenRead(path))
                {
                    encryptionKey = new byte[fs.Length];
                    int bytesRead = fs.Read(encryptionKey);
                }
            }
            else
            {
                using (RSA rsa = RSA.Create())
                {
                    encryptionKey = rsa.ExportRSAPrivateKey();

                    // Write the key to disk for future use.
                    using (FileStream fs = File.OpenWrite(path))
                    {
                        fs.Write(encryptionKey);
                    }
                }
            }

            return encryptionKey;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment HostEnvironment { get; }
    }
}
