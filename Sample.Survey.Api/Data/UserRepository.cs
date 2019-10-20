using Sample.Survey.Api.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Survey.Api.Data
{
    public class UserRepository
    {
        public UserRepository(LMSDataContext dataContext)
        {
            this.DataContext = dataContext;
        }

        public LMSDataContext DataContext { get; }

        public virtual Task<List<ProProfsUser>> GetAllUsers()
        {
            var quizzes = this.DataContext.Users.Find(x => !string.IsNullOrEmpty(x.Email));
            return quizzes.ToListAsync();
        }

        public virtual Task<ProProfsUser> GetUser(string emailId)
        {
            var quizzes = this.DataContext.Users.Find(x => x.Email.ToLower() == emailId.ToLower());
            return quizzes.FirstOrDefaultAsync();
        }

        public virtual Task<List<ProProfsUser>> GetReportees(string emailId)
        {
            var quizzes = this.DataContext.Users.Find(x => x.ManagerEmail.ToLower() == emailId.ToLower());
            return quizzes.ToListAsync();
        }

        public virtual async Task<ProProfsUser> AddUser(ProProfsUser user)
        {
            var users = this.DataContext.Users.Find(x => x.Email.ToLower() == user.Email.ToLower());
            var existingUser = await users.FirstOrDefaultAsync();
            if (existingUser != null)
            {
                user.Password = existingUser.Password;
                user.ManagerEmail = existingUser.ManagerEmail;
                FilterDefinition<ProProfsUser> filter = Builders<ProProfsUser>.Filter.Where(q => q.Email == user.Email);
                await this.DataContext.Users.ReplaceOneAsync(filter, user);
                return user;
            }
            else
            {
                await this.DataContext.Users.InsertOneAsync(user);
                return user;
            }
        }

    }
}
