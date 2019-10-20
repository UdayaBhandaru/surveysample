// <copyright file="Startup.cs" company="Agility E Services">
// Copyright (c) Agility E Services. All rights reserved.
// </copyright>

namespace Sample.Survey.Api
{

    using Sample.Survey.Api.Settings;
    using Sample.Survey.ldap.Identity.Settings;
    using Sample.Survey.Library.MongoDB.Settings;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using NLog.Extensions.Logging;
    using NLog.Web;
    using System;

    public class Startup
    {
        private string[] corsOrigin;

        public Startup(IHostingEnvironment env)
        {
            env.ConfigureNLog("nlog.config");

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);
            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(true);
            }
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();


            this.corsOrigin = this.Configuration.GetSection("CorsUrls").Value.Split(',');
        }

        public IConfiguration Configuration { get; private set; }

        public virtual void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IDistributedCache distributedCache,
            IMemoryCache memoryCache,
            IServiceProvider serviceProvider)

        {

            app.UseDeveloperExceptionPage();
            app.UseAuthentication();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseCors("Cors");
            app.UseAppMvc();


        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();


            services
                  .Configure<LdapSettings>(
                      Configuration.GetSection("LdapSettings"));
            services
                  .Configure<TokenSettings>(
                      Configuration.GetSection("TokenSettings"));
            services
               .Configure<ProProfsSettings>(
                   Configuration.GetSection("ProProfsSettings"));
            services
                .Configure<MongoDbSettings>(
                    Configuration.GetSection("MongoDbSettings"));
            services.ConfigureApplication();
            services.ConfigureLDAPSecurityDI();
            // Uncomment the next line if the .NET Core 1.x compatibility is required 
            // services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.ConfigureLocalizationDI();
            //services.ConfigureWebCoreDI();
            //services.ConfigureSecurityDI();
            //services.ConfigureApplicationDI();
            //services.AddDbContext();           
            services.AddCors(o => o.AddPolicy("Cors", builder =>
            {
                builder.WithOrigins(this.corsOrigin)

                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials();
            }));
            services.AddAppAuthentication();
            services.AddMvcAndJsonOptions();
            return services.BuildServiceProvider();
        }
    }
}

