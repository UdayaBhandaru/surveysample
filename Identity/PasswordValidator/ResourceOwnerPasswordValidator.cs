// <copyright file="FocisResourceOwnerPasswordValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Identity.Service.PasswordValidator
{
    using Identity.Service.ldap.Identity;
    using Identity.Service.ldap.Identity.Models;
    using Identity.Service.Models;
    using IdentityServer4.AspNetIdentity;
    using IdentityServer4.Events;
    using IdentityServer4.Models;
    using IdentityServer4.Services;
    using IdentityServer4.Validation;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;
    using System.Threading.Tasks;
    using static IdentityModel.OidcConstants;

    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly LdapUserManager ldapUserManager;
        private readonly LdapSignInManager ldapSignInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private IEventService events;

        public ResourceOwnerPasswordValidator(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, LdapSignInManager ldapSignInManager, LdapUserManager ldapUserManager, IEventService events, ILogger<ResourceOwnerPasswordValidator<LdapUser>> logger)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.ldapUserManager = ldapUserManager;
            this.ldapSignInManager = ldapSignInManager;
            this.events = events;
        }



        public ILogger<ResourceOwnerPasswordValidator<LdapUser>> Logger { get; }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            SignInResult result = SignInResult.Failed;
            IdentityUser user;
            // Perform Custom Validation
            if (context.Request.Client.IdentityProviderRestrictions.Contains("Windows"))
            {
                user = await ldapUserManager.FindByNameAsync(context.UserName);
                result = ldapSignInManager.CheckPasswordSignInAsync((LdapUser)user, context.Password, true);
            }
            else
            {
                user = await userManager.FindByNameAsync(context.UserName);
                result = await signInManager.CheckPasswordSignInAsync((ApplicationUser)user, context.Password, true);
            }


            if (result.Succeeded)
            {
                // this.Logger.LogInformation("Credentials validated for username: {username}", context.UserName);
                await events.RaiseAsync(new UserLoginSuccessEvent(context.UserName, user.Id, context.UserName, interactive: false));

                context.Result = new GrantValidationResult(user.Id, AuthenticationMethods.Password, null, "local", null);
                return;
            }
            else if (result.IsLockedOut)
            {
                // this.Logger.LogInformation("Authentication failed for username: {username}, reason: locked out", context.UserName);
                await events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "locked out", interactive: false));
            }
            else if (result.IsNotAllowed)
            {
                // this.Logger.LogInformation("Authentication failed for username: {username}, reason: not allowed", context.UserName);
                await events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "not allowed", interactive: false));
            }
            else
            {
                // this.Logger.LogInformation("Authentication failed for username: {username}, reason: invalid credentials", context.UserName);
                await events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "invalid credentials", interactive: false));
            }

            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
        }
    }
}