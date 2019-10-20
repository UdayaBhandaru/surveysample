using Identity.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Service.Data
{
    public class UserRepository
    {
        private ApplicationDbContext dbContext { get; set; }
        public UserRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;

        }

        public UserProfile  GetUserProfile(string emailId)
        {
           return this.dbContext.UserProfiles.Where(x => x.EmailAddress == emailId).FirstOrDefault();
        }
    }
}
