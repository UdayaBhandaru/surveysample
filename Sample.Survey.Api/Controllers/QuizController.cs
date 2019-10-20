using Sample.Survey.Api.Data;
using Sample.Survey.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Sample.Survey.Api.Controllers
{
    public class QuizController : Controller
    {
        private readonly QuizRepository quizReporitory;

        private readonly UserRepository userRepository;

        public QuizController(QuizRepository quizReporitory, UserRepository userRepository)
        {
            this.quizReporitory = quizReporitory;
            this.userRepository = userRepository;
        }


        [HttpGet("/api/quiz")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<List<ProProfQuiz>> Get()
        {
            return await this.quizReporitory.GetQuizzes();
        }

        [HttpGet("/api/quiz/{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ProProfQuiz> Get(string id)
        {
            return await this.quizReporitory.GetQuiz(id);
        }

        [HttpPost("/api/quiz")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ProProfQuiz> Post([FromBody]ProProfQuiz quiz)
        {
            return await this.quizReporitory.SaveQuiz(quiz);
        }

        [HttpGet("/api/quiz/New")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public ProProfQuiz New()
        {
            return new ProProfQuiz();

        }

        [HttpGet("/api/quiz/Take")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ProProfQuiz> Take([FromQuery]string uniqueQuiz)
        {
            var email = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email || x.Type == JwtRegisteredClaimNames.Email);
            if (email != null)
            {
                var user = await this.userRepository.GetUser(email.Value);
                var quiz = await this.quizReporitory.GetQuizByUniqeName(uniqueQuiz);
                if (quiz != null)
                {
                    quiz.QuizLink = $"https://www.proprofs.com/quiz-school/story.php?title={quiz.QuizUniqueName}&user_email={user.Email}&passkey={user.Password}";
                }
                return quiz;
            }

            return null;
        }
    }
}
