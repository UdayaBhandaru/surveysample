using Sample.Survey.Api.Controllers;
using Sample.Survey.Api.Data;
using Sample.Survey.Api.Models;
using Sample.Survey.Library.MongoDB.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sample.Survey.Api.Test.Quiz
{
    public class QuizControllerTest
    {
        private readonly Mock<QuizRepository> _mockRepository;
        private readonly Mock<UserRepository> _mockUserRepository;
        private readonly Mock<LMSDataContext> _mockDataContext;
        private readonly Mock<IOptions<MongoDbSettings>> _mockMongoSettings;

        public QuizControllerTest()
        {
            _mockMongoSettings = new Mock<IOptions<MongoDbSettings>>();
            _mockMongoSettings.Setup(x => x.Value).Returns(new MongoDbSettings() { ConnectionString = "mongodb://localhost:27017", DBName = "Test" });
            _mockDataContext = new Mock<LMSDataContext>(_mockMongoSettings.Object);
            _mockRepository = new Mock<QuizRepository>(_mockDataContext.Object);
            _mockUserRepository = new Mock<UserRepository>(_mockDataContext.Object);
        }

        [Fact]
        public async Task TestGetAllQuiz()
        {
            var collection = new List<ProProfQuiz>();
            _mockRepository.Setup(x => x.GetQuizzes()).ReturnsAsync(collection);
            var controller = new QuizController(_mockRepository.Object, _mockUserRepository.Object);
            var quizs = await controller.Get();
            Assert.Equal(collection, quizs);
        }

        [Fact]
        public async Task TestGetQuiz()
        {
            var quiz = new ProProfQuiz() { Id = "1", QuizLink = "Test", QuizName = "Test" };
            _mockRepository.Setup(x => x.GetQuiz("1")).ReturnsAsync(quiz);
            var controller = new QuizController(_mockRepository.Object, _mockUserRepository.Object);
            var quizs = await controller.Get("1");
            Assert.Equal(quiz, quizs);
        }

        [Fact]
        public async Task TestPostQuiz()
        {
            var quiz = new ProProfQuiz() { Id = "1", QuizLink = "Test", QuizName = "Test" };
            _mockRepository.Setup(x => x.SaveQuiz(quiz)).ReturnsAsync(quiz);
            var controller = new QuizController(_mockRepository.Object, _mockUserRepository.Object);
            var quizs = await controller.Post(quiz);
            Assert.Equal(quiz, quizs);
        }

        [Fact]
        public async Task TestTakeQuiz()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
               {
                    new Claim(ClaimTypes.Name, "ubhandaru@agility.com"),
                    new Claim(ClaimTypes.Email, "ubhandaru@agility.com")
               }, "mock"));


            var quiz = new ProProfQuiz() { Id = "1", QuizLink = "Test", QuizName = "Test", QuizUniqueName = "test" };

            _mockRepository.Setup(x => x.GetQuizByUniqeName("test")).ReturnsAsync(quiz);
            _mockUserRepository.Setup(x => x.GetUser("ubhandaru@agility.com")).ReturnsAsync(new ProProfsUser() { Email = "ubhandaru@agility.com", Password = "Test" });
            var controller = new QuizController(_mockRepository.Object, _mockUserRepository.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            var quizs = await controller.Take("test");
            Assert.Equal($"https://www.proprofs.com/quiz-school/story.php?title=test&user_email=ubhandaru@agility.com&passkey=Test", quizs.QuizLink);
        }
    }
}
