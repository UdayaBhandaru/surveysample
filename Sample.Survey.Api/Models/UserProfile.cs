using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Survey.Api.Models
{
    public class UserProfile
    {

        public string UserId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string NormalizedUserName { get; set; }
        public string UserAccessToken { get; set; }
    }
}
