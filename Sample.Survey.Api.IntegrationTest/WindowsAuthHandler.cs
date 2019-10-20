// Copyright 2018 IntelliTect
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Sample.Survey.TestHost.WindowsAuth
{
    /// <inheritdoc />
    /// <summary>
    ///     ASP.NET Core Authentication handler for authenticating as arbitrary users for integration tests.
    ///     Provides behavior similar to the ASP.NET Core IIS integration AuthenticationHandler.
    /// </summary>
    /// <remarks>
    ///     This is inspired largely by
    ///     https://github.com/aspnet/IISIntegration/blob/release/2.0/src/Microsoft.AspNetCore.Server.IISIntegration/AuthenticationHandler.cs
    ///     It aims to be as close to the IIS handler as possible.
    /// </remarks>
    internal class WindowsAuthenticationHandler : IAuthenticationHandler
    {
        public const string AuthenticationScheme = "Test";

        private HttpContext _context;

        public Task<AuthenticateResult> AuthenticateAsync()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
         {
                new Claim(ClaimTypes.Name, "logistics\\ubhandaru"),
                new Claim(ClaimTypes.Email, "ubhandaru@agility.com"),
         }, "mock"));

            return Task.FromResult(
                    AuthenticateResult.Success(new AuthenticationTicket(user, AuthenticationScheme)));
        }

        public Task ChallengeAsync(AuthenticationProperties properties)
        {
            _context.Response.StatusCode = 401;
            return Task.CompletedTask;
        }

        public Task ForbidAsync(AuthenticationProperties properties)
        {
            _context.Response.StatusCode = 403;
            return Task.CompletedTask;
        }

        public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
        {
            _context = context;
            return Task.CompletedTask;
        }
    }
}