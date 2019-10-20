using Sample.Survey.Api.Models;
using Sample.Survey.TestHost.WindowsAuth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;
namespace Sample.Survey.Api.IntegrationTest
{
    public class QuizTest : IClassFixture<TestFixture<Startup>>
    {
        private HttpClient Client;
        private string UserAccessToken;

        public QuizTest(TestFixture<Startup> fixture)
        {
            Client = fixture.Client;
        }

        [Fact]
        public async Task TestGetAllQuiz()
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
                Url = "/api/Quiz"
            };

            Client.DefaultRequestHeaders.Authorization
                         = new AuthenticationHeaderValue("Bearer", UserAccessToken);

            var response1 = await Client.GetAsync(request.Url);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response1.StatusCode);
        }


        [Fact]
        public async Task TestSaveQuiz()
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
                Url = "/api/Quiz"
            };

            Client.DefaultRequestHeaders.Authorization
                         = new AuthenticationHeaderValue("Bearer", UserAccessToken);
            var getallresponse = await Client.GetAsync(request.Url);
            var output = await getallresponse.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<IList<ProProfQuiz>>(output);
            var quiz = new ProProfQuiz();
            quiz.Id = Guid.NewGuid().ToString();
            quiz.QuizName = "Test Quiz";
            quiz.QuizLink = "Test Quiz Link";
            var input = JsonConvert.SerializeObject(quiz);
            var httpContent = new StringContent(input, Encoding.UTF8, "application/json");
            var sqveQuizResponse = await Client.PostAsync(request.Url, httpContent);
            getallresponse = await Client.GetAsync(request.Url);
            output = await getallresponse.Content.ReadAsStringAsync();
            var data1 = JsonConvert.DeserializeObject<IList<ProProfQuiz>>(output);
            request = new
            {
                Url = $"/api/Quiz/{quiz.Id}"
            };
            getallresponse = await Client.GetAsync(request.Url);
            output = await getallresponse.Content.ReadAsStringAsync();
            var quizouput = JsonConvert.DeserializeObject<ProProfQuiz>(output);
            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, sqveQuizResponse.StatusCode);
            Assert.True(data1.Count > data.Count);
            Assert.True(quizouput.QuizLink == quiz.QuizLink);
        }

        [Fact]
        public async Task TestGetQuiz()
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
                Url = "/api/Quiz/1"
            };

            Client.DefaultRequestHeaders.Authorization
                         = new AuthenticationHeaderValue("Bearer", UserAccessToken);

            var response1 = await Client.GetAsync(request.Url);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NoContent, response1.StatusCode);
        }

        [Fact]
        public async Task TestTakeQuiz()
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
                Url = "/api/Quiz/Take?uniqueQuiz=Test"
            };

            Client.DefaultRequestHeaders.Authorization
                         = new AuthenticationHeaderValue("Bearer", UserAccessToken);

            var response1 = await Client.GetAsync(request.Url);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response1.StatusCode);
        }

    }
}
