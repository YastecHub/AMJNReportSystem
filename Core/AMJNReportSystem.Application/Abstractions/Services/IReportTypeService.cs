using AMJNReportSystem.Application.Models.DTOs;
using AMJNReportSystem.Application.Models.RequestModels;
using AMJNReportSystem.Application.Wrapper;

namespace AMJNReportSystem.Application.Abstractions.Services
{
    /// <summary>
    /// Interface class that handles Report Type methods
    /// </summary>
    public interface IReportTypeService
    {
        /// <summary>
        /// Method for creating new report type, accepting the name, description 
        /// and ReportTag(MuqamReport, DilaReport, ZoneReport, QaidReport) as parameters by Admin only
        /// </summary>
        Task<BaseResponse<bool>> CreateReportType(CreateReportTypeRequest request);

        /// <summary>
        /// Method for update report type, accepting the name and description as parameters by Admin only
        /// </summary>
        Task<BaseResponse<bool>> UpdateReportType(Guid reportTypeId, UpdateReportTypeRequest request);

        /// <summary>
        /// Method that get a particular report type, accepting the reportType Id as parameter
        /// </summary>
        Task<BaseResponse<ReportTypeDto>> GetReportType(Guid reportTypeId);

        /// <summary>
        /// Method that get all report types
        /// </summary>
        Task<BaseResponse<IEnumerable<ReportTypeDto>>> GetReportTypes();

        /// <summary>
        /// Method that get Qaid report types by report tag. use the enum directly
        /// </summary>
        Task<BaseResponse<IEnumerable<ReportTypeDto>>> GetQaidReportTypes();

        /// <summary>
        /// Method for update Activeness of Report-Type Activeness state, accepting Id and description as parameters by Admin only
        /// </summary>
        Task<BaseResponse<bool>> SetReportTypeActiveness(Guid reportTypeId, bool state);


    }
}
