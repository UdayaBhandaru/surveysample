// <copyright file="AspNetIdentityProfileService.cs" company="Agility E Services">
// Copyright (c) Agility E Services. All rights reserved.
// </copyright>

namespace Identity.Service.Services
{
    using Identity.Service.Data;
    using Identity.Service.ldap.Identity;
    using Identity.Service.ldap.Identity.Models;
    using Identity.Service.Models;
    using IdentityModel;
    using IdentityServer4.Extensions;
    using IdentityServer4.Models;
    using IdentityServer4.Services;
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class AspNetIdentityProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly LdapUserManager ldapUserManager;
        private readonly UserRepository userRepository;

        // private SecurityRepository securityRepository;
        private string sessionId;

        public AspNetIdentityProfileService(UserManager<ApplicationUser> userManager, LdapUserManager ldapUserManager, UserRepository userRepository)
        {
            this.userManager = userManager;
            this.ldapUserManager = ldapUserManager;
            this.userRepository = userRepository;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            ClaimsPrincipal subject = context.Subject;



            IList<Claim> claims;
            string subjectId = subject.GetSubjectId();
            if (context.Client.IdentityProviderRestrictions.Contains("Windows"))
            {
                LdapUser user = await ldapUserManager.FindByIdAsync(subjectId);
                // Windows Authentication
                if (user == null)
                {
                    throw new ArgumentException("Invalid subject identifier");
                }

                claims = GetAuthenticationUserClaims(user).ToList();
            }
            else
            {
                // Forms Authentication
                if (subject == null)
                {
                    string subjectName = nameof(context.Subject);
                    throw new ArgumentNullException(subjectName);
                }
                ApplicationUser user = await userManager.FindByIdAsync(subjectId);
                if (user == null)
                {
                    throw new ArgumentException("Invalid subject identifier");
                }

                claims = GetAuthenticationUserClaims(user).ToList();

            }

            context.IssuedClaims = claims.ToList();
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            ClaimsPrincipal subject = context.Subject;
            IdentityUser user;
            if (subject == null)
            {
                string subjectName = nameof(context.Subject);
                throw new ArgumentNullException(subjectName);
            }

            string subjectId = subject.GetSubjectId();
            if (context.Client.IdentityProviderRestrictions.Contains("Windows"))
            {

                user = await ldapUserManager.FindByIdAsync(subjectId);
            }
            else
            {
                user = await userManager.FindByIdAsync(subjectId);
            }

            if (user != null)
            {
                context.IsActive = true;
            }
        }

        private IEnumerable<Claim> GetAuthenticationUserClaims(IdentityUser user)
        {
            if (sessionId == null)
            {
                sessionId = Guid.NewGuid().ToString();
            }

            // var profile = await Profile.Get(user.UserName, this.securityRepository);

            // string companies = string.Join(",", profile.Companies.Select(s => s.OrganizationId.ToString()));
            // string adminRoles = (profile.AdminRoles != null) ? string.Join(",", profile.AdminRoles) : string.Empty;

            // List<ClaimModel> sessionClaims = new List<ClaimModel>();
            UserProfile userprofile = userRepository.GetUserProfile(user.Email);

            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, user.Id),
                new Claim(JwtClaimTypes.PreferredUserName, user.UserName),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtClaimTypes.Name, user.UserName),
                new Claim("session_id", sessionId),
                new Claim("email_id", user.Email),
                new Claim("FirstName", userprofile.FirstName),
                new Claim("LastName", userprofile.LastName),
                new Claim("OrganizationId", userprofile.OrganizationId.ToString()),
                new Claim("TenantId", userprofile.TenantId.ToString()),
                new Claim("UserId", user.Id),
            };

            if (userManager.SupportsUserPhoneNumber && !string.IsNullOrWhiteSpace(user.PhoneNumber))
            {
                claims.AddRange(new[]
                {
                    new Claim(JwtClaimTypes.PhoneNumber, user.PhoneNumber),
                    new Claim(JwtClaimTypes.PhoneNumberVerified, user.PhoneNumberConfirmed ? "true" : "false", ClaimValueTypes.Boolean)
                });
            }



            return claims;
        }
    }
}
