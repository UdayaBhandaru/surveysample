using Sample.Survey.Api.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Survey.Api.Data
{
    public class QuizRepository
    {

        public QuizRepository(LMSDataContext dataContext)
        {
            this.DataContext = dataContext;
        }

        public LMSDataContext DataContext { get; }

        public virtual async Task<List<ProProfQuiz>> GetQuizzes()
        {
            return await this.DataContext.Quizzes.Find(x => true).ToListAsync();
        }

        public virtual async Task<ProProfQuiz> GetQuiz(string Id)
        {
            return await this.DataContext.Quizzes.Find(x => x.Id == Id).FirstOrDefaultAsync();
        }

        public virtual async Task<ProProfQuiz> GetQuizByUniqeName(string uniqueName)
        {
            return await this.DataContext.Quizzes.Find(x => x.QuizUniqueName == uniqueName).FirstOrDefaultAsync();
        }

        public virtual async Task<ProProfQuiz> SaveQuiz(ProProfQuiz quiz)
        {
            var quizzes = this.DataContext.Quizzes.Find(x => x.Id == quiz.Id);
            var existingQuiz = await quizzes.FirstOrDefaultAsync();
            if (existingQuiz != null)
            {
                FilterDefinition<ProProfQuiz> filter = Builders<ProProfQuiz>.Filter.Where(q => q.Id == quiz.Id);
                await this.DataContext.Quizzes.ReplaceOneAsync(filter, quiz);
                return quiz;
            }
            else
            {
                await this.DataContext.Quizzes.InsertOneAsync(quiz);
                return quiz;
            }
        }
    }
}
