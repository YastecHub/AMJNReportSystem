using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AMJNReportSystem.Application.Abstractions.Repositories
{
    public interface IQuestionRepository
    {
        Task<bool> AddQuestion(Question question);
        Task<IList<Question>> GetQuestionsBySection(Guid sectionId);
        Task<Question> GetQuestionAsync(Expression<Func<Question, bool>> expression);
        Task<bool> UpdateQuestion(Question question);
        Task<IList<Question>> GetQuestions(Expression<Func<Question, bool>> expression);
        Task<Question> GetQuestion(Expression<Func<Question, bool>> expression);
        Task<Question> GetQuestionById(Guid id);
    }
}
