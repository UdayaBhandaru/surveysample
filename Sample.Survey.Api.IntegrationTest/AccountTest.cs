using Sample.Survey.Api.IntegrationTest.Models;
using Sample.Survey.Api.Models;
using Sample.Survey.TestHost.WindowsAuth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
// using WideWorldImporters.API.Models;
using Xunit;
namespace Sample.Survey.Api.IntegrationTest
{
    public class AccountTest : IClassFixture<TestFixture<Startup>>
    {
        private HttpClient Client;
        private string UserAccessToken;

        public AccountTest(TestFixture<Startup> fixture)
        {
            Client = fixture.Client;
        }

        [Fact]
        public async Task UserProfile()
        {
            // Arrange
            var request = new
            {
                Url = "/api/Account/Userprofile"
            };

            // Act
            var response = await Client.GetAsync(request.Url);
            var json = await response.Content.ReadAsStringAsync();

            var singleResponse = JsonConvert.DeserializeObject<UserProfile>(json);
            UserAccessToken = singleResponse.UserAccessToken;
            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Login()
        {
            // Arrange
            var request = new
            {
                Url = "/api/Account/Login"
            };

            var model = new LoginModel();
            model.UserName = "ubhandaru@agility.com";
            model.UserPassword = "bh_uday@1";
            var input = JsonConvert.SerializeObject(model);
            var httpContent = new StringContent(input, Encoding.UTF8, "application/json");

            // Act
            var response = await Client.PostAsync(request.Url, httpContent);
            var json = await response.Content.ReadAsStringAsync();

            var singleResponse = JsonConvert.DeserializeObject<UserProfile>(json);
            UserAccessToken = singleResponse.UserAccessToken;
            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(UserAccessToken);
        }
    }
}
