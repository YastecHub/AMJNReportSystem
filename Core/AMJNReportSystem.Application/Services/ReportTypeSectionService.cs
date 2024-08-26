using AMJNReportSystem.Application.Abstractions.Repositories;
using AMJNReportSystem.Application.Abstractions.Services;
using AMJNReportSystem.Application.Models.DTOs;
using AMJNReportSystem.Application.Models.RequestModels;
using AMJNReportSystem.Application.Wrapper;
using Domain.Entities;
using Mapster;

namespace AMJNReportSystem.Application.Services
{
    public class ReportTypeSectionService : IReportTypeSectionService
    {
        private readonly IReportTypeSectionRepository _reportTypeSectionRepository;

        public ReportTypeSectionService(IReportTypeSectionRepository reportTypeSectionRepository)
        {
            _reportTypeSectionRepository = reportTypeSectionRepository;
        }

        public async Task<Result<bool>> CreateReportTypeSection(CreateReportTypeSectionRequest request)
        {
            var reportTypeSectionExist = await _reportTypeSectionRepository.GetReportTypeSection(x => x.Name == request.Name);

            if (reportTypeSectionExist is not null) return await Result<bool>.FailAsync($"ReportTypeSection with name {request.Name} already exist");
            var reportTypeSection = new ReportTypeSection
            {
                Name = request.Name,
                Description = request.Description,
                ReportTypeId = request.ReportTypeId
            };
            //var reportTypeSection = request.Adapt<ReportTypeSection>();
            await _reportTypeSectionRepository.CreateReportTypeSection(reportTypeSection);
            return await Result<bool>.SuccessAsync("Successfully created");
            //return null;
        }

        public async Task<Result<ReportTypeSectionDto>> GetReportTypeSection(Guid reportTypeSectionId)
        {
            var reportTypeSection = await _reportTypeSectionRepository.GetReportTypeSection(x => x.Id == reportTypeSectionId);

            if (reportTypeSection is null) return await Result<ReportTypeSectionDto>.FailAsync("ReportTypeSection with Id not found");

            var reportTypeSectionDto = reportTypeSection.Adapt<ReportTypeSectionDto>();
            reportTypeSectionDto.Name = reportTypeSection.Name;
            reportTypeSectionDto.Description = reportTypeSection.Description;
            return await Result<ReportTypeSectionDto>.SuccessAsync(reportTypeSectionDto, "Successfully retrieved reportTypeSection");

        }

        public async Task<Result<IEnumerable<ReportTypeSectionDto>>> GetReportTypeSections(Guid reportTypeId)
        {
            var reportTypeSections = await _reportTypeSectionRepository.GetReportTypeSections(x => x.ReportTypeId == reportTypeId);

            if (reportTypeSections.Count is 0) return await Result<IEnumerable<ReportTypeSectionDto>>.FailAsync("There is no reportTypeSection for the section Id");

            var reportTypeSectionDtos = new List<ReportTypeSectionDto>();
            for (int i = 0; i < reportTypeSections.Count(); i++)
            {
                var reportTypeSectionDto = new ReportTypeSectionDto
                {
                    ReportTypeId = reportTypeId,
                    Name = reportTypeSections[i].Name,
                    Description = reportTypeSections[i].Description,

                };
                reportTypeSectionDtos.Add(reportTypeSectionDto);

            }

            return await Result<IEnumerable<ReportTypeSectionDto>>.SuccessAsync(reportTypeSectionDtos, "Successfully retrieved reportTypeSection");
        }

        public async Task<Result<bool>> SetReportTypeSectionActiveness(Guid reportTypeSectionId, bool state)
        {
            var reportTypeSection = await _reportTypeSectionRepository.GetReportTypeSectionById(reportTypeSectionId);
            if (reportTypeSection is null)
                return await Result<bool>.FailAsync("Report Type Id input not found");
            reportTypeSection.isActive = state;
            await _reportTypeSectionRepository.UpdateReportTypeSection(reportTypeSection);
            return await Result<bool>.SuccessAsync("Report-Type Activeness state updated");
        }

        public async Task<Result<bool>> UpdateReportTypeSection(Guid reportTypeSectionId, UpdateReportTypeSectionRequest request)
        {
            var reportTypeSection = await _reportTypeSectionRepository.GetReportTypeSection(x => x.Id == reportTypeSectionId);

            if (reportTypeSection is null) return await Result<bool>.FailAsync("ReportTypeSection with Id not found");
            reportTypeSection.Name = request.Name;
            reportTypeSection.Description = request.Description;
            await _reportTypeSectionRepository.UpdateReportTypeSection(reportTypeSection);
            return await Result<bool>.SuccessAsync("Report Type Section successfully update");
        }
    }
}
