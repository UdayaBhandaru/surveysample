using System;
using System.Web;

namespace Sample.Survey.Api.Models
{
    public class ProProfsUserQuiz
    {
        public string QuizId { get; set; }
        public string QuizUniqueName { get; set; }
        public string QuizTitle { get; set; }
        public string QuizLink { get; set; }
        public string EncodedQuizLink
        {
            get
            {
                return HttpUtility.UrlEncode(QuizLink);
            }
        }
        public string QuizStatus { get; set; }
        public int QuizMarks { get; set; }
        public int QuizTotalMarks { get; set; }
        public int CorrectAnswers { get; set; }
        public int WrongAnswers { get; set; }
        public DateTimeOffset? AttemptDate { get; set; }
        public double? PercentComplete { get; set; }
    }
}