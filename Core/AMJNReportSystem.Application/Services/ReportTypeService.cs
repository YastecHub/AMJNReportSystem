using AMJNReportSystem.Application.Abstractions.Repositories;
using AMJNReportSystem.Application.Abstractions.Services;
using AMJNReportSystem.Application.Models.DTOs;
using AMJNReportSystem.Application.Models.RequestModels;
using AMJNReportSystem.Application.Wrapper;
using Domain.Entities;
using Mapster;

namespace AMJNReportSystem.Application.Services
{
    public class ReportTypeService : IReportTypeService
    {
        private readonly IReportTypeRepository _reportTypeRepository;
        public ReportTypeService(IReportTypeRepository reportTypeRepository)
        {
            _reportTypeRepository = reportTypeRepository;
        }

        public async Task<Result<bool>> CreateReportType(CreateReportTypeRequest request)
        {
            var reportTypeChecker = await _reportTypeRepository.Exist(request.Name);
            if (reportTypeChecker)
            {
                return await Result<bool>.SuccessAsync("Report's name already exit");
            }
            var reportType = request.Adapt<ReportType>();
            await _reportTypeRepository.AddReportType(reportType);
            return await Result<bool>.SuccessAsync("Report Type Successfully Created");

        }

        public async Task<Result<IEnumerable<ReportTypeDto>>> GetQaidReportTypes()
        {
            var reportTypes = await _reportTypeRepository.GetQaidReports();
            var selectedReport = reportTypes.Select(r => r.Adapt<ReportTypeDto>()).ToList();
            return await Result<IEnumerable<ReportTypeDto>>.SuccessAsync(selectedReport, "Reports Retreived");
        }

        public async Task<Result<ReportTypeDto>> GetReportType(Guid reportTypeId)
        {
            var reportType = await _reportTypeRepository.GetReportTypeById(reportTypeId);
            if (reportType is null)
            {
                return await Result<ReportTypeDto>.FailAsync("ReportType with Id not found");
            }
            var returnedReportType = reportType.Adapt<ReportTypeDto>();
            returnedReportType.Name = reportType.Name;
            returnedReportType.Description = reportType.Description;
            return await Result<ReportTypeDto>.SuccessAsync(returnedReportType, "Report Successfully Retrieved");
        }

        public async Task<Result<IEnumerable<ReportTypeDto>>> GetReportTypes()
        {
            var reportTypes = await _reportTypeRepository.GetAllReportTypes();
            var selectedReport = reportTypes.Select(r => r.Adapt<ReportTypeDto>()).ToList();
            return await Result<IEnumerable<ReportTypeDto>>.SuccessAsync(selectedReport, "All reports retreived Successfully");
        }

        public async Task<Result<bool>> SetReportTypeActiveness(Guid reportTypeId, bool state)
        {
            var reportType = await _reportTypeRepository.GetReportTypeById(reportTypeId);
            if (reportType is null)
                return await Result<bool>.FailAsync("An Id with report-type not found");
            reportType.isActive = state;
            await _reportTypeRepository.UpdateReportType(reportType);
            return await Result<bool>.SuccessAsync("Report-Type Activeness updated");
        }

        public async Task<Result<bool>> UpdateReportType(Guid id, UpdateReportTypeRequest request)
        {
            var reportType = await _reportTypeRepository.GetReportTypeById(id);

            if (reportType is null)
                return await Result<bool>.FailAsync("Report Type with input Id not found");
            reportType.Name = request.Name;
            reportType.Description = request.Description;
            await _reportTypeRepository.UpdateReportType(reportType);
            return await Result<bool>.SuccessAsync("report Type Successfully update");
        }
    }
}
