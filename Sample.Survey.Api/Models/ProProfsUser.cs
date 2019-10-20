using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Sample.Survey.Api.Models
{
    [BsonIgnoreExtraElements]
    public class ProProfsUser
    {

        public string UserName { get; set; }

        public string ManagerEmail { get; set; }

        public string Password { get; set; }

        [BsonId]
        public string Email { get; set; }

        public IList<string> Group { get; set; }

        public IList<ProProfsUserQuiz> Quizzes { get; set; }
    }
}
