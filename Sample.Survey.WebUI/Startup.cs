// <copyright file="Startup.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Sample.Survey
{
    using System;
    using Agility.Framework.Core.Common;
    using Agility.Framework.SqlServer;
    using Agility.Framework.Web;
    using Microsoft.AspNetCore.Antiforgery;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SpaServices.AngularCli;
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using NLog.Extensions.Logging;
    using NLog.Web;

    public class Startup : FxSpaStartup
    {
        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            this.Configuration = configuration;
            this.Startup(env, configuration);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public override IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddTransient(typeof(IDbStartup), typeof(SqlServerStartup));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
            return base.ConfigureServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IAntiforgery antiforgery,
            IDistributedCache distributedCache,
            IMemoryCache memoryCache,
            IServiceProvider serviceProvider,
            IFxCacheModel fxCacheModel)
        { 
            

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            base.Configure(app, env, loggerFactory, antiforgery, distributedCache, memoryCache, serviceProvider, fxCacheModel);
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "webapi",
                    template: "api/{controller}/{action}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.Options.StartupTimeout = new System.TimeSpan(0, 0, 90);
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}