//-------------------------------------------------------------------------------------------------
// <copyright file="AccountController.cs" company="Agility E Services">
// Copyright (c) Agility E Services. All rights reserved.
// </copyright>
// All leave related application specific contoller Operations
//-------------------------------------------------------------------------------------------------

namespace Sample.Survey.Api.Controllers.Account
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using Sample.Survey.Api.Models;
    using Sample.Survey.Api.Settings;
    using Sample.Survey.ldap.Identity;
    using Sample.Survey.ldap.Identity.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;


    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> ilogger;
        private readonly LdapUserManager userManager;
        private readonly IOptions<TokenSettings> options;

        public AccountController(
           LdapUserManager userManager,
            ILogger<AccountController> ilogger, IOptions<TokenSettings> options)
        {
            this.userManager = userManager;
            this.ilogger = ilogger;
            this.options = options;
        }

        [HttpGet]
        [Authorize]
        public async Task<UserProfile> UserProfile()
        {

            UserProfile profileModel = new UserProfile();
            try
            {
                string userEmail = string.Empty;
                if (this.User.Identity.Name.Contains("\\"))
                {
                    // get the email from AD and pass it to the below method, without manupulating the email
                    userEmail = this.User.Identity.Name.Substring(this.User.Identity.Name.IndexOf(@"\") + 1);
                }
                else if (this.User.Identity.Name.Contains("@"))
                {
                    userEmail = this.User.Identity.Name;
                }


                var user = await this.userManager.FindByNameAsync(userEmail);
                profileModel.Email = user.Email;
                profileModel.Email = user.EmailAddress;
                profileModel.Name = user.Name;

                profileModel.UserAccessToken = GetToken(user);
            }
            catch (Exception e)
            {
                this.ilogger.LogError(e.Message);
            }

            return profileModel;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<UserProfile> Login([FromBody]LoginModel loginModel)
        {
            var user = await this.userManager.FindByEmailAddress(loginModel.UserName);
            if (user != null)
            {
                var isValidUser = this.userManager.CheckPasswordAsync(user, loginModel.UserPassword);
                if (isValidUser)
                {
                    UserProfile profileModel = new UserProfile();
                    profileModel.Email = user.Email;
                    profileModel.Email = user.EmailAddress;
                    profileModel.Name = user.Name;
                    profileModel.UserAccessToken = GetToken(user);
                    return profileModel;
                }
            }
            return null;
        }


        private string GetToken(IdentityUser user)
        {
            var utcNow = DateTime.Now;

            var claims = new Claim[]
            {
                        new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Iat, utcNow.ToString()),
                         new Claim(JwtRegisteredClaimNames.Email, user.Email),
            };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.options.Value.Key));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var jwt = new JwtSecurityToken(
                signingCredentials: signingCredentials,
                claims: claims,
                notBefore: utcNow,
                expires: utcNow.AddSeconds(this.options.Value.Lifetime),
                audience: this.options.Value.Audience,
                issuer: this.options.Value.Issuer);

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

    }
}
