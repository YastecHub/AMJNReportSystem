using AMJNReportSystem.Application.Models.DTOs;
using AMJNReportSystem.Application.Models.RequestModels;
using AMJNReportSystem.Application.Wrapper;

namespace AMJNReportSystem.Application.Abstractions.Services
{
    /// <summary>
    /// Interface class that handles Report Section methods
    /// </summary>
    public interface IReportSectionService
    {
        /// <summary>
        /// Method for creating new report section, accepting the name, description, and reporttypeId 
        /// as parameters by Admin only
        /// </summary>
        Task<Result<bool>> CreateReportSection(CreateReportSectionRequest request);

        /// <summary>
        /// Method for update report section, accepting the name and description and reporttypeId
        /// as parameters by Admin only
        /// </summary>
        Task<Result<bool>> UpdateReportSection(Guid reportSectionId, UpdateReportSectionRequest request);

        /// <summary>
        /// Method that get a particular report section, accepting the reportSection Id as parameter
        /// </summary>
        Task<Result<ReportSectionDto>> GetReportSection(Guid reportSectionId);

        /// <summary>
        /// Method that get all report sections for a particular reporttype
        /// </summary>
        Task<Result<IEnumerable<ReportSectionDto>>> GetReportSections(Guid reportTypeId);

        /// <summary>
        /// Method for update Report-Section, accepting Id and description as parameters by Admin only
        /// </summary>
        ///
        Task<Result<bool>> SetreportSectionActiveness(Guid reportSectionId, bool state);

        /// <summary>
        /// Method for deleting Report-Section
        /// </summary>
        /// <param name="reportSectionId"></param>
        /// <returns></returns>
       Task<Result<bool>> DeleteReportSection(Guid reportSectionId);
    }
}

