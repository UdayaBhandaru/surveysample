using Sample.Survey.Api.Adapter;
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

    public class ProProfsController : Controller
    {
        private readonly ProProfsAdapter proProfsAdapter;
        private readonly UserRepository userRepository;


        public ProProfsController(ProProfsAdapter proProfsAdapter, UserRepository userRepository)
        {
            this.proProfsAdapter = proProfsAdapter;
            this.userRepository = userRepository;
        }

        [HttpPost("/api/ProProfs/UserQuiz")]
        [Authorize]
        public async Task<ProProfsUser> PostQuiz([FromBody]ProProfsUser proProfsUser)
        {
            // Need to change this logic . need to add more security here as passing user email is not good . Need to allow this based on Role.
            var result = await this.proProfsAdapter.AddUserToQuiz(proProfsUser);
            if (result)
            {
                var user = await this.proProfsAdapter.QuizStatus(proProfsUser.Email);
                return await this.userRepository.AddUser(user);
            }
            return null;
        }


        [HttpGet("/api/ProProfs/User")]
        [Authorize]
        public async Task<ProProfsUser> Get()
        {
            var email = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email || x.Type == JwtRegisteredClaimNames.Email);
            if (email != null)
            {
                var user = await this.userRepository.GetUser(email.Value);
                if (user == null)
                {
                    var userinProProfs = await this.proProfsAdapter.QuizStatus(email.Value);
                    return await this.userRepository.AddUser(userinProProfs);
                }
                return user;
            }
            return null;
        }
    }
}
