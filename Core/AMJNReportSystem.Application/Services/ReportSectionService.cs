using AMJNReportSystem.Application.Abstractions.Repositories;
using AMJNReportSystem.Application.Abstractions.Services;
using AMJNReportSystem.Application.Models.DTOs;
using AMJNReportSystem.Application.Models.RequestModels;
using AMJNReportSystem.Application.Wrapper;
using AMJNReportSystem.Domain.Entities;
using Mapster;

namespace AMJNReportSystem.Application.Services
{
    public class ReportSectionService : IReportSectionService
    {
        private readonly IReportSectionRepository _reportSectionRepository;

        public ReportSectionService(IReportSectionRepository reportSectionRepository)
        {
            _reportSectionRepository = reportSectionRepository;
        }

        public async Task<Result<bool>> CreateReportSection(CreateReportSectionRequest request)
        {
            try
            {
                var reportSection = new ReportSection
                {
                    ReportSectionName = request.Name,
                    ReportSectionValue = request.Value,
                    Description = request.Description,
                    ReportTypeId = request.ReportTypeId,
                    IsActive = true
                };

                var isCreated = await _reportSectionRepository.CreateReportSection(reportSection);
                return isCreated
                    ? Result<bool>.Success(true)
                    : Result<bool>.Fail("Failed to create report section.");
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail($"An error occurred: {ex.Message}");
            }
        }

        public async Task<Result<bool>> UpdateReportSection(Guid reportSectionId, UpdateReportSectionRequest request)
        {
            try
            {
                var existingSection = await _reportSectionRepository.GetReportSectionById(reportSectionId);
                if (existingSection == null)
                    return Result<bool>.Fail("Report section not found.");

                existingSection.ReportSectionName = request.Name;
                existingSection.ReportSectionValue = request.Value;
                existingSection.Description = request.Description;

                var isUpdated = await _reportSectionRepository.UpdateReportSection(existingSection);
                return isUpdated
                    ? Result<bool>.Success(true)
                    : Result<bool>.Fail("Failed to update report section.");
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail($"An error occurred: {ex.Message}");
            }
        }

        public async Task<Result<ReportSectionDto>> GetReportSection(Guid reportSectionId)
        {
            try
            {
                var reportSection = await _reportSectionRepository.GetReportSectionById(reportSectionId);
                if (reportSection == null)
                    return Result<ReportSectionDto>.Fail("Report section not found.");

                var reportSectionDto = reportSection.Adapt<ReportSectionDto>();
                return Result<ReportSectionDto>.Success(reportSectionDto);
            }
            catch (Exception ex)
            {
                return Result<ReportSectionDto>.Fail($"An error occurred: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<ReportSectionDto>>> GetReportSections(Guid reportTypeId)
        {
            try
            {
                var reportSections = await _reportSectionRepository.GetReportSections(rs => rs.ReportTypeId == reportTypeId);
                var reportSectionDtos = reportSections.Adapt<IEnumerable<ReportSectionDto>>();
                return Result<IEnumerable<ReportSectionDto>>.Success(reportSectionDtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<ReportSectionDto>>.Fail($"An error occurred: {ex.Message}");
            }
        }


        public async Task<Result<bool>> SetreportSectionActiveness(Guid reportSectionId, bool state)
        {
            try
            {
                var reportSection = await _reportSectionRepository.GetReportSectionById(reportSectionId);
                if (reportSection != null)
                {
                    reportSection.IsActive = state;
                    var isUpdated = await _reportSectionRepository.UpdateReportSection(reportSection);
                    return isUpdated
                        ? Result<bool>.Success(true)
                        : Result<bool>.Fail("Failed to update report section activeness.");
                }
                return Result<bool>.Fail("Report section not found.");
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail($"An error occurred: {ex.Message}");
            }
        }

        public async Task<Result<bool>> DeleteReportSection(Guid reportSectionId)
        {
            var reportSection = await _reportSectionRepository.GetReportSectionById(reportSectionId);
            if (reportSection == null)
            {
                return Result<bool>.Fail("Report section not found.");
            }

            reportSection.IsActive = false;
            reportSection.IsDeleted = true;

            var result = await _reportSectionRepository.UpdateReportSection(reportSection);

            return result ? Result<bool>.Success(true) : Result<bool>.Fail("Failed to delete report section.");
        }
    }
}
