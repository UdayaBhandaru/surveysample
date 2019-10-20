using Sample.Survey.Api.Adapter;
using Sample.Survey.Api.Controllers;
using Sample.Survey.Api.Data;
using Sample.Survey.Api.Models;
using Sample.Survey.Api.ProProfs;
using Sample.Survey.Api.Settings;
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

namespace Sample.Survey.Api.Test.ProProfs
{
    public class ProProfControllerTest
    {
        private readonly Mock<UserRepository> _mockUserRepository;
        private readonly Mock<LMSDataContext> _mockDataContext;
        private readonly Mock<IOptions<MongoDbSettings>> _mockMongoSettings;
        private readonly Mock<IOptions<ProProfsSettings>> _mockProProfsSettings;
        private readonly Mock<ProProfsProxy> _mockproxy;
        private readonly Mock<ProProfsAdapter> _mockAdapter;
        private readonly Mock<HttpHandler> _httpHandler;

        public ProProfControllerTest()
        {
            _mockMongoSettings = new Mock<IOptions<MongoDbSettings>>();
            _mockMongoSettings.Setup(x => x.Value).Returns(new MongoDbSettings() { ConnectionString = "mongodb://localhost:27017", DBName = "Test" });
            this._mockProProfsSettings = new Mock<IOptions<ProProfsSettings>>();
            _mockDataContext = new Mock<LMSDataContext>(_mockMongoSettings.Object);
            _mockUserRepository = new Mock<UserRepository>(_mockDataContext.Object);
            this._httpHandler = new Mock<HttpHandler>();
            this._mockProProfsSettings.Setup(x => x.Value).Returns(new ProProfsSettings() { ApplicationKey = "6d6ad80cd8ef81e63cab7c8689334d69", UserName = "AGILITYGTT", ProProfsRegisterUrl = "https://www.proprofs.com/api/classroom/v1/user/register/", ProProfsReportUrl = "https://www.proprofs.com/api/classroom/v1/reports/users/" });
            this._mockproxy = new Mock<ProProfsProxy>(this._mockProProfsSettings.Object, this._httpHandler.Object);
            this._mockAdapter = new Mock<ProProfsAdapter>(this._mockproxy.Object);
        }

        [Fact]
        public async Task Register()
        {
            var user = new ProProfsUser();
            user.Email = "ubhandaru@agilty.com";
            this._mockAdapter.Setup(x => x.AddUserToQuiz(user)).ReturnsAsync(true);
            this._mockAdapter.Setup(x => x.QuizStatus(user.Email)).ReturnsAsync(user);
            this._mockUserRepository.Setup(x => x.AddUser(user)).ReturnsAsync(user);
            var controller = new ProProfsController(this._mockAdapter.Object, this._mockUserRepository.Object);
            var result = await controller.PostQuiz(user);

            Assert.NotNull(result);
            Assert.Equal(user, result);
        }

        [Fact]
        public async Task Status()
        {
            var userClaim = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "ubhandaru@agility.com"),
                new Claim(ClaimTypes.Email, "ubhandaru@agility.com")
            }, "mock"));

            var user = new ProProfsUser();
            user.Email = "ubhandaru@agility.com";
            this._mockAdapter.Setup(x => x.AddUserToQuiz(user)).ReturnsAsync(true);
            this._mockAdapter.Setup(x => x.QuizStatus(user.Email)).ReturnsAsync(user);
            this._mockUserRepository.Setup(x => x.AddUser(user)).ReturnsAsync(user);
            this._mockUserRepository.Setup(x => x.GetUser(user.Email)).ReturnsAsync(user);
            var controller = new ProProfsController(this._mockAdapter.Object, this._mockUserRepository.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = userClaim }
            };
            var result = await controller.Get();

            Assert.NotNull(result);
            Assert.Equal(user, result);
        }

        [Fact]
        public async Task StatusNull()
        {
            var userClaim = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "ubhandaru@agility.com"),
                new Claim(ClaimTypes.Email, "ubhandaru@agility.com")
            }, "mock"));

            var user = new ProProfsUser();
            user.Email = "ubhandaru@agility.com";
            this._mockAdapter.Setup(x => x.AddUserToQuiz(user)).ReturnsAsync(true);
            this._mockAdapter.Setup(x => x.QuizStatus(user.Email)).ReturnsAsync(user);
            this._mockUserRepository.Setup(x => x.AddUser(user)).ReturnsAsync(user);
            this._mockUserRepository.Setup(x => x.GetUser(string.Empty)).ReturnsAsync(user);
            var controller = new ProProfsController(this._mockAdapter.Object, this._mockUserRepository.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = userClaim }
            };
            var result = await controller.Get();

            Assert.NotNull(result);
            Assert.Equal(user, result);
        }
    }
}
