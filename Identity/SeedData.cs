// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Identity.Service.Models;
using Identity.Service.Data;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;

namespace Identity.Service
{
    public class SeedData
    {
        public static void EnsureSeedData(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                var configurationcontext = scope.ServiceProvider.GetService<ConfigurationDbContext>();
                var persistedGrantDbContext = scope.ServiceProvider.GetService<PersistedGrantDbContext>();
                try
                {
                    var email = (from users in context.Users select users.Email).FirstOrDefault();
                    if (email == null)
                    {
                        context.Database.Migrate();
                    }
                }
                catch (Exception ex)
                {
                    context.Database.Migrate();
                }

                if (!configurationcontext.Clients.Any())
                {
                    foreach (var client in Config.GetClients())
                    {
                        configurationcontext.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!configurationcontext.IdentityResources.Any())
                {
                    foreach (var resource in Config.GetIdentityResources())
                    {
                        configurationcontext.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!configurationcontext.ApiResources.Any())
                {
                    foreach (var resource in Config.GetApis())
                    {
                        configurationcontext.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}
