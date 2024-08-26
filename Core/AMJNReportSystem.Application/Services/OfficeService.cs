using AMJNReportSystem.Application.Abstractions.Repositories;
using AMJNReportSystem.Application.Abstractions.Services;
using AMJNReportSystem.Application.Models;
using AMJNReportSystem.Application.Models.RequestModels;
using AMJNReportSystem.Application.Wrapper;
using application.Models.ResponseModels;
using Domain.Entities;
using Mapster;

namespace application.Services
{
    public class OfficeService : IOfficeService
    {
        private readonly IOfficeRepository _officerepository;

        public OfficeService(IOfficeRepository officerepository)
        {
            _officerepository = officerepository;
        }

        public async Task<Result<bool>> CreateOfficeAsync(CreateOfficeRequest request)
        {
            var officeToCreate = await _officerepository.ExistByName(request.Name);
            if (officeToCreate)
            {
                return await Result<bool>.FailAsync("Office's Name Already Created");
            }
            var officeToBeCreated = request.Adapt<Office>();
            await _officerepository.AddOffice(officeToBeCreated);
            return await Result<bool>.SuccessAsync("Office Successfully Created");

        }

        public async Task<Result<PaginatedResult<OfficeResponseModel>>> GetAllOffices<T>(PaginationFilter filter)
        {
            var allOffices = await _officerepository.GetAllOfficesAsync(filter);
            return await Result<PaginatedResult<OfficeResponseModel>>.SuccessAsync(allOffices, "Offices Successfully Retrieved");

        }

        public async Task<Result<bool>> UpdateOfficeAsync(UpdateOfficeRequest request)
        {
            var existingOffice = await _officerepository.GetByName(request.Name);
            if (existingOffice is null)
                return await Result<bool>.FailAsync("Office not found");
            existingOffice.Name = request.Name ?? existingOffice.Name;
            existingOffice.Email = request.Email ?? existingOffice.Email;
            existingOffice.Description = request.Description ?? existingOffice.Description;
            await _officerepository.UpdateOffice(existingOffice);
            return await Result<bool>.SuccessAsync("Office successfully updated");

        }
    }
}