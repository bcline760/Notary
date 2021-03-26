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
            var config = Configuration.GetSection("Notary").Get<NotaryConfiguration>();

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
                    ValidAudience = config.TokenSettings.Audience,
                    ValidIssuer = config.TokenSettings.Issuer
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

            if (HostEnvironment.IsDevelopment())
            {
                services.AddCors(o =>
                {
                    o.AddPolicy("CorsPolicy", b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
                });
            }
            if (HostEnvironment.IsProduction())
            {
                string origins = Environment.GetEnvironmentVariable("");
                if (string.IsNullOrEmpty(origins))
                    throw new InvalidOperationException("Please set environment variable ");

                services.AddCors(o =>
                {
                    o.AddPolicy("CorsPolicy", b => b.AllowAnyMethod().AllowAnyHeader().WithOrigins(origins.Split(',')));
                });
            }
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            var config = Configuration.GetSection("Notary").Get<NotaryConfiguration>();

            // Use environment variables for sensitive attributes in production. 
            if (HostEnvironment.IsProduction())
            {
                config.ApplicationKey = Environment.GetEnvironmentVariable(Constants.ApplicationKeyEnvName);
                config.ConnectionString = Environment.GetEnvironmentVariable(Constants.DatabaseConnectionStringEnvName);
                config.ServiceAccountUser = Environment.GetEnvironmentVariable(Constants.ServiceAccountUserEnvName);
                config.ServiceAccountPassword = Environment.GetEnvironmentVariable(Constants.ServiceAccountPassEnvName);
            }
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

            app.UseCors("CorsPolicy");

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
