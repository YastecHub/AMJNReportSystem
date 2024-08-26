using AMJNReportSystem.Application.Models.DTOs;
using AMJNReportSystem.Application.Models.RequestModels;
using AMJNReportSystem.Application.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace AMJNReportSystem.Application.Abstractions.Services
{
    /// <summary>
    /// Interface class that handles Report questions
    /// </summary>
    public interface ISectionQuestionService
    {
        /// <summary>
        /// Method for creating new question for a report type section, accepting the Text 
        /// responseType(Enum : Num, Text, Bool, Image ), Points, SectionId as parameters by Admin only
        /// </summary>
        Task<Result<bool>> AddQuestion(ReportQuestionRequest request);

        /// <summary>
        /// Method for update question, accepting the text and questionId as parameters by Admin only
        /// </summary>
        Task<Result<bool>> UpdateQuestionText(Guid questionId, string text);

        /// <summary>
        /// Method for update question point, accepting the point  and questionId as parameters by Admin only
        /// </summary>
        Task<Result<bool>> UpdateQuestionPoint(Guid questionId, double point);

        /// <summary>
        /// Method that get a particular question, accepting the question Id as parameter
        /// </summary>
        Task<Result<QuestionDto>> GetQuestion(Guid questionId);

        /// <summary>
        /// Method that get all questions by sectionId
        /// </summary>
        Task<Result<IEnumerable<QuestionDto>>> GetSectionQuestions(Guid sectionId);

        /// <summary>
        /// Method that get all questions for a particular report type
        /// </summary>
        Task<Result<ReportQuestionsModel>> GetReportTypeQuestions(Guid reportTypeId);

        /// <summary>
        /// Method for update Question Activeness State, accepting id and description as parameters by Admin only
        /// </summary>
        Task<Result<bool>> QuestionActivenessState(Guid questionId, bool state);
    }
}