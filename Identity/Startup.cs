// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Identity.Service.Crypto;
using Identity.Service.Data;
using Identity.Service.ldap.Identity;
using Identity.Service.ldap.Identity.Models;
using Identity.Service.ldap.Services;
using Identity.Service.ldap.Settings;
using Identity.Service.Models;
using Identity.Service.PasswordValidator;
using Identity.Service.Services;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Reflection;

namespace StsServer
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName.ToLower()}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddTransient<LdapService>();
            services.AddTransient<LdapUserManager>();
            services.AddTransient<LdapSignInManager>();
            services.AddTransient<LdapService>();
            services.AddTransient<UserRepository>();
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString);

            });
            string migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddUserManager<UserManager<ApplicationUser>>()
                .AddSignInManager<SignInManager<ApplicationUser>>()
                .AddDefaultTokenProviders();
            services
                  .Configure<LdapSettings>(
                      Configuration.GetSection("LdapSettings"));
            services.AddCors(options => options.AddPolicy("anyOrigin", p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials()));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.Configure<IISOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });
            //services.Configure<IISOptions>(iis =>
            //{
            //    iis.AuthenticationDisplayName = "Windows";
            //    iis.AutomaticAuthentication = true;
            //});
            // services.AddTransient<ITokenCreationService, CustomDefaultTokenCreationService>();

            IIdentityServerBuilder identityServerServices = services.AddIdentityServer()

                   .AddAspNetIdentity<ApplicationUser>()

                   .AddConfigurationStore(options =>
                   {
                       options.ConfigureDbContext = b =>
                           b.UseSqlServer(connectionString,
                               sql => sql.MigrationsAssembly(migrationsAssembly));
                       options.DefaultSchema = string.IsNullOrEmpty(Configuration.GetValue<string>("DatabaseSchema")) ? "dbo" : Configuration.GetValue<string>("DatabaseSchema");
                   })
                // this adds the operational data from DB (codes, tokens, consents)
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b =>
                        b.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));
                    options.DefaultSchema = string.IsNullOrEmpty(Configuration.GetValue<string>("DatabaseSchema")) ? "dbo" : Configuration.GetValue<string>("DatabaseSchema");
                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;
                })// Custom Profile Service for Custom Claims
                   .AddProfileService<AspNetIdentityProfileService>()

                   //Custom Password Validator
                   .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>();


            identityServerServices.AddCustomSigningCredential(Configuration["SigningKey"]);
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCors("anyOrigin");
            app.UseIdentityServer();
            app.UseMvcWithDefaultRoute();
        }

        private IEnumerable<ApiResource> ConfigureApiResources()
        {
            List<ApiResource> apiresourcesfromConfig = new List<ApiResource>();
            List<ApiResource> apiResources = new List<ApiResource>();
            Configuration.GetSection("Clients:APiClient:ApiResource").Bind(apiresourcesfromConfig);
            apiresourcesfromConfig.ForEach(apiResource => apiResources.Add(
             new ApiResource(apiResource.Name, apiResource.UserClaims) { ApiSecrets = apiResource.ApiSecrets }));
            apiResources.ForEach(clientResource =>
            {
                List<Secret> clientSecrets = new List<Secret>();

                foreach (Secret clientSecret in clientResource.ApiSecrets)
                {
                    clientSecrets.Add(new Secret(clientSecret.Value.Sha256(), null));
                }
                clientResource.ApiSecrets = clientSecrets;
            });
            return apiResources;
        }

        private IEnumerable<Client> ConfigureClientResources()
        {
            List<Client> clientResources = new List<Client>();
            Configuration.GetSection("Clients:WebClient:Client").Bind(clientResources);

            clientResources.ForEach(clientResource =>
            {
                List<Secret> clientSecrets = new List<Secret>();

                foreach (Secret clientSecret in clientResource.ClientSecrets)
                {
                    clientSecrets.Add(new Secret(clientSecret.Value.Sha256(), null));
                }
                clientResource.ClientSecrets = clientSecrets;
            });
            return clientResources;
        }

        private IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }
    }
}
