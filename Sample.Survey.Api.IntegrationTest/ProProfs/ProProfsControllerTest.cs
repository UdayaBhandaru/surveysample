using Sample.Survey.Api.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sample.Survey.Api.IntegrationTest.ProProfs
{
    public class ProProfsControllerTest : IClassFixture<TestFixture<Startup>>
    {
        private HttpClient Client;
        private string UserAccessToken;
        public ProProfsControllerTest(TestFixture<Startup> fixture)
        {
            Client = fixture.Client;
        }

        [Fact]
        public async Task TestStatus()
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
            request = new
            {
                Url = "/api/ProProfs/User"
            };

            Client.DefaultRequestHeaders.Authorization
                         = new AuthenticationHeaderValue("Bearer", UserAccessToken);

            var response1 = await Client.GetAsync(request.Url);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response1.StatusCode);
        }

        [Fact]
        public async Task TestRegistration()
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
            request = new
            {
                Url = "/api/ProProfs/UserQuiz"
            };

            Client.DefaultRequestHeaders.Authorization
                         = new AuthenticationHeaderValue("Bearer", UserAccessToken);

            var model = new ProProfsUser();
            model.Email = "ubhandaru@agility.com";
            model.Quizzes = new List<ProProfsUserQuiz>() { new ProProfsUserQuiz() { QuizLink = "https://www.proprofs.com/quiz-school/story.php?title=hazard-communication-right-to-know" } };
            var input = JsonConvert.SerializeObject(model);
            var httpContent = new StringContent(input, Encoding.UTF8, "application/json");

            var response1 = await Client.PostAsync(request.Url, httpContent);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response1.StatusCode);
        }
    }
}
