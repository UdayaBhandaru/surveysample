using Sample.Survey.ldap.Identity.Models;
using Sample.Survey.ldap.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Survey.ldap.Identity
{
    public class LdapUserManager
    {
        private readonly LdapService _ldapService;

        public LdapUserManager(
            LdapService ldapService)

        {
            _ldapService = ldapService;
        }

        public virtual LdapUser GetAdministrator()
        {
            return _ldapService.GetAdministrator();
        }

        /// <summary>
        /// Checks the given password agains the configured LDAP server.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public virtual bool CheckPasswordAsync(LdapUser user, string password)
        {
            return _ldapService.Authenticate(user.DistinguishedName, password);
        }

        public virtual Task<bool> HasPasswordAsync(LdapUser user)
        {
            return Task.FromResult(true);
        }

        public virtual Task<LdapUser> FindByIdAsync(string userId)
        {
            return FindByNameAsync(userId);
        }

        public virtual Task<LdapUser> FindByNameAsync(string userName)
        {
            return Task.FromResult(_ldapService.GetUserByUserName(userName));
        }

        public virtual Task<LdapUser> FindByEmailAddress(string userName)
        {
            return Task.FromResult(_ldapService.GetUsersByEmailAddress(userName).FirstOrDefault());
        }

        public virtual async Task<IdentityResult> CreateAsync(LdapUser user, string password)
        {
            try
            {
                _ldapService.AddUser(user, password);
            }
            catch (Exception e)
            {
                return await Task.FromResult(IdentityResult.Failed(new IdentityError() { Code = "LdapUserCreateFailed", Description = e.Message ?? "The user could not be created." }));
            }

            return await Task.FromResult(IdentityResult.Success);
        }

        public virtual async Task<IdentityResult> DeleteUserAsync(string distinguishedName)
        {
            try
            {
                _ldapService.DeleteUser(distinguishedName);
            }
            catch (Exception e)
            {
                return await Task.FromResult(IdentityResult.Failed(new IdentityError() { Code = "LdapUserDeleteFailed", Description = e.Message ?? "The user could not be deleted." }));
            }

            return await Task.FromResult(IdentityResult.Success);
        }
    }
}
