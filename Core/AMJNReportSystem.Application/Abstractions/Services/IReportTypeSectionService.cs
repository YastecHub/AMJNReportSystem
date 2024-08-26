using AMJNReportSystem.Application.Models.DTOs;
using AMJNReportSystem.Application.Models.RequestModels;
using AMJNReportSystem.Application.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMJNReportSystem.Application.Abstractions.Services
{
    /// <summary>
    /// Interface class that handles Report Type Section methods
    /// </summary>
    public interface IReportTypeSectionService
    {
        /// <summary>
        /// Method for creating new report type section, accepting the name, description, and reporttypeId 
        /// as parameters by Admin only
        /// </summary>
        Task<Result<bool>> CreateReportTypeSection(CreateReportTypeSectionRequest request);

        /// <summary>
        /// Method for update report type section, accepting the name and description and reporttypeId
        /// as parameters by Admin only
        /// </summary>
        Task<Result<bool>> UpdateReportTypeSection(Guid reportTypeSectionId, UpdateReportTypeSectionRequest request);

        /// <summary>
        /// Method that get a particular report type section, accepting the reportTypeSection Id as parameter
        /// </summary>
        Task<Result<ReportTypeSectionDto>> GetReportTypeSection(Guid reportTypeSectionId);

        /// <summary>
        /// Method that get all report type sections for a particular reporttype
        /// </summary>
        Task<Result<IEnumerable<ReportTypeSectionDto>>> GetReportTypeSections(Guid reportTypeId);

        /// <summary>
        /// Method for update Report-Type Section, accepting Id and description as parameters by Admin only
        /// </summary>
        Task<Result<bool>> SetReportTypeSectionActiveness(Guid reportTypeSectionId, bool state);
    }
}
