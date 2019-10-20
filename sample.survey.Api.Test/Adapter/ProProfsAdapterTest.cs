using Sample.Survey.Api.Adapter;
using Sample.Survey.Api.Models;
using Sample.Survey.Api.ProProfs;
using Sample.Survey.Api.Settings;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sample.Survey.Api.Test.Adapter
{
    public class ProProfsAdapterTest
    {
        private readonly ProProfsProxy proProfsProxy;
        private readonly HttpHandler httpHandler;
        private readonly Mock<IOptions<ProProfsSettings>> _mockProProfsSettings;
        public ProProfsAdapterTest()
        {
            this.httpHandler = new HttpHandler();
            this._mockProProfsSettings = new Mock<IOptions<ProProfsSettings>>();
            this._mockProProfsSettings.Setup(x => x.Value).Returns(new ProProfsSettings() { ApplicationKey = "6d6ad80cd8ef81e63cab7c8689334d69", UserName = "AGILITYGTT", ProProfsRegisterUrl = "https://www.proprofs.com/api/classroom/v1/user/register/", ProProfsReportUrl = "https://www.proprofs.com/api/classroom/v1/reports/users/" });
            this.proProfsProxy = new ProProfsProxy(this._mockProProfsSettings.Object, this.httpHandler);
        }

        [Fact]
        public async Task TestUserQuizRegistration()
        {
            var adapter = new ProProfsAdapter(this.proProfsProxy);
            var usermodel = new ProProfsUser();
            usermodel.Email = "ubhandaru@agility.com";
            var quizzes = new List<ProProfsUserQuiz>();
            quizzes.Add(new ProProfsUserQuiz() { QuizLink = "https://www.proprofs.com/quiz-school/story.php?title=agility-security-awareness-quiz" });
            usermodel.Quizzes = quizzes;

            var result = await adapter.AddUserToQuiz(usermodel);

            Assert.True(result);
        }

        [Fact]
        public async Task TestUserGroupRegistration()
        {
            var adapter = new ProProfsAdapter(this.proProfsProxy);
            var usermodel = new ProProfsUser();
            usermodel.Email = "ubhandaru@agility.com";
            usermodel.Group = new List<string>() { "Test" };
            var result = await adapter.AddUserToGroup(usermodel);
            Assert.True(result);
        }

        [Fact]
        public async Task TestUserStatus()
        {
            var adapter = new ProProfsAdapter(this.proProfsProxy);
            var usermodel = new ProProfsUser();
            usermodel.Email = "ubhandaru@agility.com";
            usermodel.Group = new List<string>() { "Test" };
            var result = await adapter.QuizStatus("ubhandaru@agility.com");
            Assert.NotNull(result);
        }
    }
}
