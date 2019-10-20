using Sample.Survey.Api.Models;
using Sample.Survey.Library.MongoDB;
using Sample.Survey.Library.MongoDB.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Sample.Survey.Api.Data
{
    public class LMSDataContext : MongoContext
    {
        public LMSDataContext(IOptions<MongoDbSettings> options) : base(options)
        {

        }


        public IMongoCollection<ProProfsUser> Users
        {
            get
            {
                return this.Database.GetCollection<ProProfsUser>("Users");
            }
        }

        public IMongoCollection<ProProfQuiz> Quizzes
        {
            get
            {
                return this.Database.GetCollection<ProProfQuiz>("Quizzes");
            }
        }
    }
}
