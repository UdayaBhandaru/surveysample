using Sample.Survey.ldap.Identity.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Sample.Survey.ldap.Identity
{
    public class LdapSignInManager
    {
        private LdapUserManager _ldapUserManager;
        public LdapSignInManager(
            LdapUserManager ldapUserManager)

        {
            _ldapUserManager = ldapUserManager;
        }



        public virtual SignInResult CheckPasswordSignInAsync(LdapUser applicationUser, string password, bool remeberlogin)
        {
            var user = this._ldapUserManager.CheckPasswordAsync(applicationUser, password);
            return user ? SignInResult.Success : SignInResult.Failed;
        }
    }
}
