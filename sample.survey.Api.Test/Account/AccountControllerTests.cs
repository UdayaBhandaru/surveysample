using Sample.Survey.Api.Controllers.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Sample.Survey.ldap.Identity.Settings;
using Sample.Survey.ldap.Identity;
using Sample.Survey.ldap.Identity.Models;

using Sample.Survey.Api.Settings;
using System.Threading.Tasks;
using Sample.Survey.Api.Models;

namespace Sample.Survey.Api.Test.Account
{
    public class AccountControllerTests
    {
        private readonly Mock<ILogger<AccountController>> _mockLogger;
        private readonly Mock<LdapUserManager> _mockUserManager;
        private readonly Mock<IOptions<LdapSettings>> _mockLdapSettings;
        private readonly Mock<IOptions<TokenSettings>> _mockTokenSettings;
        public AccountControllerTests()
        {
            _mockLogger = new Mock<ILogger<AccountController>>();
            _mockLdapSettings = new Mock<IOptions<LdapSettings>>();
            _mockUserManager = new Mock<LdapUserManager>(null);
            _mockTokenSettings = new Mock<IOptions<TokenSettings>>();
            this._mockTokenSettings.Setup(x => x.Value).Returns(new TokenSettings() { Audience = "test", Lifetime = 1000, Issuer = "test", Key = "C428A377979E395725A6A1A13A0CE0D25F1B30B7DAE0EFB06F26F79EDC149472" });
        }

        [Fact]
        public async Task GetUserProfile()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "logisticsdev\\ubhandaru"),
            }, "mock"));

            this._mockTokenSettings.Setup(x => x.Value).Returns(new TokenSettings() { Audience = "test", Lifetime = 1000, Issuer = "test", Key = "C428A377979E395725A6A1A13A0CE0D25F1B30B7DAE0EFB06F26F79EDC149472" });
            this._mockUserManager.Setup(x => x.FindByNameAsync("ubhandaru")).ReturnsAsync(() =>
            {
                var ldapuser = new LdapUser()
                {
                    EmailAddress = "ubhandaru@agility.com",
                    Name = "ubhandaru@agility.com"
                };
                ldapuser.EmailAddress = "ubhandaru@agility.com";
                return ldapuser;
            });

            var controller = new AccountController(this._mockUserManager.Object, this._mockLogger.Object, this._mockTokenSettings.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            // Act

            var result = await controller.UserProfile();

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.UserAccessToken);
        }

        [Fact]
        public async Task GetUserProfileWithEmail()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "ubhandaru@agility.com"),
                new Claim(ClaimTypes.Email, "ubhandaru@agility.com")
            }, "mock"));


            this._mockUserManager.Setup(x => x.FindByNameAsync("ubhandaru@agility.com")).ReturnsAsync(() =>
            {
                var ldapuser = new LdapUser()
                {
                    EmailAddress = "ubhandaru@agility.com",
                    Name = "ubhandaru@agility.com"
                };
                ldapuser.EmailAddress = "ubhandaru@agility.com";
                return ldapuser;
            });

            var controller = new AccountController(this._mockUserManager.Object, this._mockLogger.Object, this._mockTokenSettings.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            // Act

            var result = await controller.UserProfile();

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.UserAccessToken);
        }

        [Fact]
        public async Task Login()
        {
            var ldapuser = new LdapUser()
            {
                EmailAddress = "ubhandaru@agility.com",
                Name = "ubhandaru@agility.com"
            };
            ldapuser.EmailAddress = "ubhandaru@agility.com";

            this._mockUserManager.Setup(x => x.FindByEmailAddress("ubhandaru@agility.com")).ReturnsAsync(() =>
            {
                return ldapuser;
            });


            this._mockUserManager.Setup(x => x.CheckPasswordAsync(ldapuser, "*****")).Returns(true);
            var controller = new AccountController(this._mockUserManager.Object, this._mockLogger.Object, this._mockTokenSettings.Object);

            var model = new LoginModel();
            model.UserName = "ubhandaru@agility.com";
            model.UserPassword = "*****";
            var result = await controller.Login(model);


            Assert.NotNull(result);
            Assert.NotNull(result.UserAccessToken);
        }
    }
}