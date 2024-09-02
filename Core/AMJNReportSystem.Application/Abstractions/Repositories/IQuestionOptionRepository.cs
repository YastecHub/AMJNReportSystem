using AMJNReportSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMJNReportSystem.Application.Abstractions.Repositories
{
    public interface IQuestionOptionRepository
    {
        Task<bool> CreateQuestionOption(QuestionOption questionOption);
        Task<IList<QuestionOption>> GetAllQuestionOptionAsync();
        Task<QuestionOption> GetQuestionOptionById(Guid Id);
        Task<bool> UpdateQuestionOption(QuestionOption questionOption);
    }
}
