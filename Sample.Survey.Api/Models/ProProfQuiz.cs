using MongoDB.Bson.Serialization.Attributes;

namespace Sample.Survey.Api.Models
{
    public class ProProfQuiz
    {
        [BsonId]
        public string Id { get; set; }

        public string QuizName { get; set; }

        public string QuizUniqueName { get; set; }

        public string QuizLink { get; set; }
    }
}