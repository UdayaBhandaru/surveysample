using Identity.Service.ldap.Identity.Models;
using Identity.Service.ldap.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Service.ldap.Identity
{
    public class LdapUserManager
    {
        private readonly LdapService _ldapService;

        public LdapUserManager(
            LdapService ldapService)

        {
            _ldapService = ldapService;
        }

        public LdapUser GetAdministrator()
        {
            return _ldapService.GetAdministrator();
        }

        /// <summary>
        /// Checks the given password agains the configured LDAP server.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public  bool CheckPasswordAsync(LdapUser user, string password)
        {
            return  _ldapService.Authenticate(user.DistinguishedName, password);
        }

        public  Task<bool> HasPasswordAsync(LdapUser user)
        {
            return Task.FromResult(true);
        }

        public  Task<LdapUser> FindByIdAsync(string userId)
        {
            return FindByNameAsync(userId);
        }

        public  Task<LdapUser> FindByNameAsync(string userName)
        {
            return Task.FromResult(_ldapService.GetUserByUserName(userName));
        }

        public  async Task<IdentityResult> CreateAsync(LdapUser user, string password)
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

        public async Task<IdentityResult> DeleteUserAsync(string distinguishedName)
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
