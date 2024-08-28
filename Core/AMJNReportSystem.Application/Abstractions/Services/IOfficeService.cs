using AMJNReportSystem.Application.Models;
using AMJNReportSystem.Application.Models.RequestModels;
using AMJNReportSystem.Application.Wrapper;
using application.Models.ResponseModels;

namespace AMJNReportSystem.Application.Abstractions.Services
{
    /// <summary>
    /// Interface class that handles National offices (qaidat) methods
    /// </summary>
    public interface IOfficeService
    {
        /// <summary>
        /// Method that create a office taking name, email and description as parameters in the request model
        /// Note: Record should be populated on  the Reporter table while adding an office, the office Id maps to the UniqueId 
        /// column of the Reporter table.
        /// Offices with the same name should not exist
        /// </summary>
        Task<Result<bool>> CreateOfficeAsync(CreateOfficeRequest request);

        /// <summary>
        /// Method that updates a office taking name, email and description as parameters in the request model
        /// Offices with the same name should not exist
        /// </summary>
        Task<Result<bool>> UpdateOfficeAsync(UpdateOfficeRequest request);

        /// <summary>
        /// Method that gets all offices
        /// </summary>
        Task<Result<PaginatedResult<OfficeResponseModel>>> GetAllOffices<T>(PaginationFilter filter);
    }
}
