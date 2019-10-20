// <copyright file="StartupExtensions.cs" company="Agility E Services">
// Copyright (c) Agility E Services. All rights reserved.
// </copyright>

namespace Sample.Survey.Api
{
    using System.Text;

    using Sample.Survey.Api.Adapter;
    using Sample.Survey.Api.Data;
    using Sample.Survey.Api.ProProfs;
    using Sample.Survey.Api.Settings;
    using Sample.Survey.ldap.Identity;
    using Sample.Survey.ldap.Services;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public static class StartupExtensions
    {

        public static void ConfigureLDAPSecurityDI(this IServiceCollection services)
        {
            services.TryAddScoped<LdapService>();
            services.TryAddScoped<LdapSignInManager>();
            services.TryAddScoped<LdapUserManager>();
        }


        public static void AddMvcAndJsonOptions(this IServiceCollection services)
        {

            services.AddMvc(options =>
            {

                //// need to enable while session timeout is happening
                // options.Filters.Add(new SessionTimeoutAttribute());

                //// disabling the response cache for all responses.
                options.Filters.Add(new ResponseCacheAttribute { Location = ResponseCacheLocation.None, NoStore = true });

                //// authorize each and every request
                //// options.Filters.Add(new Agility.Framework.Web.Security.Filters.FxAuthorizeFilterAttribute());

                //// authorize each and every request to check whether he logged in 
                // options.Filters.Add(new SingleLoginFilterAttribute());

                // options.Filters.Add(new ElapsedTimeFilterAttribute());
                //// disable AntiforgeryToken for token based authentication enabled
                ////if (CoreSettings.CoreConfiguration.Token != null && !CoreSettings.CoreConfiguration.Token.Enable)
                ////{
                ////    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                ////}
                //IHttpRequestStreamReaderFactory readerFactory = services.BuildServiceProvider().GetRequiredService<IHttpRequestStreamReaderFactory>();
                //options.ModelBinderProviders.Insert(0, new FxBodyModelBinderProvider(options.InputFormatters, readerFactory));
            }).AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public static void AddAppAuthentication(this IServiceCollection services)
        {
            ServiceProvider serviceProvider = services.BuildServiceProvider();
            var tokenSettings = serviceProvider.GetRequiredService<IOptions<TokenSettings>>();
            SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSettings.Value.Key));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Bearer";
                options.DefaultChallengeScheme = "Bearer";
            }).AddJwtBearer(config =>
             {
                 config.RequireHttpsMetadata = false;
                 config.SaveToken = true;
                 config.TokenValidationParameters = new TokenValidationParameters()
                 {
                     IssuerSigningKey = signingKey,
                     ValidateAudience = true,
                     ValidAudience = tokenSettings.Value.Audience,
                     ValidateIssuer = true,
                     ValidIssuer = tokenSettings.Value.Issuer,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,
                 };
             });

        }

        public static void ConfigureApplication(this IServiceCollection services)
        {
            services.TryAddTransient<ProProfsAdapter>();
            services.TryAddTransient<ProProfsProxy>();
            services.TryAddTransient<HttpHandler>();
            services.TryAddTransient<LMSDataContext>();
            services.TryAddTransient<QuizRepository>();
            services.TryAddTransient<UserRepository>();
        }


        public static void UseAppMvc(this IApplicationBuilder app)
        {
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                   name: "webapi",
                   template: "api/{controller}/{action}/{id?}",
                   defaults: new { controller = "App", action = "Index" });

                routes.MapRoute(
                    name: "catchall",
                    template: "{*url}",
                    defaults: new { controller = "Home", action = "Index" },
                    constraints: new { url = @"^$|^([^\.])*$" });
            });
        }
    }
}
