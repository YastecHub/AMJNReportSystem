using AMJNReportSystem.Application.Abstractions.Repositories;
using AMJNReportSystem.Application.Abstractions.Services;
using AMJNReportSystem.Application.Models.DTOs;
using AMJNReportSystem.Application.Models.RequestModels;
using AMJNReportSystem.Application.Models.ResponseModels;
using AMJNReportSystem.Application.Wrapper;
using AMJNReportSystem.Domain.Entities;
using Mapster;

namespace AMJNReportSystem.Application.Services
{
    public class ReportSectionService : IReportSectionService
    {
        private readonly IReportSectionRepository _reportSectionRepository;
        private readonly IReportTypeRepository _reportTypeRepository;

        public ReportSectionService(IReportSectionRepository reportSectionRepository, IReportTypeRepository reportTypeRepository)
        {
            _reportSectionRepository = reportSectionRepository;
            _reportTypeRepository = reportTypeRepository;
        }

        public async Task<Result<CreateReportSectionResponse>> CreateReportSection(CreateReportSectionRequest request)
        {
            try
            {
                if (request == null)
                {
                    return Result<CreateReportSectionResponse>.Fail("Request cannot be null.");
                }

                var reportTypeExists = await _reportTypeRepository.GetReportTypeById(request.ReportTypeId);
                if (reportTypeExists == null)
                {
                    return Result<CreateReportSectionResponse>.Fail("Report type Id not found.");
                }
                var id = Guid.NewGuid();
                var reportSection = new ReportSection
                {
                    Id = id,
                    ReportSectionName = request.ReportSectionName,
                    ReportSectionValue = request.ReportSectionValue,
                    Description = request.Description,
                    ReportTypeId = request.ReportTypeId,
                    IsActive = true
                };

                var isCreated = await _reportSectionRepository.CreateReportSection(reportSection);

                if (isCreated)
                {
                    var response = new CreateReportSectionResponse
                    {
                        Id = reportSection.Id, 
                        Message = "Report section created successfully."
                    };

                    return Result<CreateReportSectionResponse>.Success(response);
                }
                else
                {
                    return Result<CreateReportSectionResponse>.Fail("Failed to create report section.");
                }
            }
            catch (Exception ex)
            {
                return Result<CreateReportSectionResponse>.Fail($"An error occurred: {ex.Message}");
            }
        }

        public async Task<Result<bool>> UpdateReportSection(Guid reportSectionId, UpdateReportSectionRequest request)
        {
            try
            {
                var existingSection = await _reportSectionRepository.GetReportSectionById(reportSectionId);
                if (existingSection == null)
                {
                    return Result<bool>.Fail("Report section not found.");
                }

                existingSection.ReportSectionName = request.ReportSectionName;
                existingSection.ReportSectionValue = request.ReportSectionValue;
                existingSection.Description = request.Description;

                var isUpdated = await _reportSectionRepository.UpdateReportSection(existingSection);

                return isUpdated
                    ? Result<bool>.Success(true, "Report section updated successfully.")
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
                {
                    return Result<ReportSectionDto>.Fail("Report section not found.");
                }

                var reportSectionDto = reportSection.Adapt<ReportSectionDto>();
                return Result<ReportSectionDto>.Success(reportSectionDto, "Report section retrieved successfully.");
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
                if (!reportSections.Any())
                {
                    return Result<IEnumerable<ReportSectionDto>>.Fail("No report sections found.");
                }

                var reportSectionDtos = reportSections.Adapt<IEnumerable<ReportSectionDto>>();
                return Result<IEnumerable<ReportSectionDto>>.Success(reportSectionDtos, "Report sections retrieved successfully.");
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<ReportSectionDto>>.Fail($"An error occurred: {ex.Message}");
            }
        }

        public async Task<Result<bool>> SetReportSectionActiveness(Guid reportSectionId, bool state)
        {
            try
            {
                var reportSection = await _reportSectionRepository.GetReportSectionById(reportSectionId);
                if (reportSection == null)
                {
                    return Result<bool>.Fail("Report section not found.");
                }

                reportSection.IsActive = state;
                var isUpdated = await _reportSectionRepository.UpdateReportSection(reportSection);

                return isUpdated
                    ? Result<bool>.Success(true, $"Report section activeness updated to {(state ? "active" : "inactive")}.")
                    : Result<bool>.Fail("Failed to update report section activeness.");
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail($"An error occurred: {ex.Message}");
            }
        }

        public async Task<Result<bool>> DeleteReportSection(Guid reportSectionId)
        {
            try
            {
                var reportSection = await _reportSectionRepository.GetReportSectionById(reportSectionId);
                if (reportSection == null)
                {
                    return Result<bool>.Fail("Report section not found.");
                }

                reportSection.IsActive = false;
                reportSection.IsDeleted = true;

                var result = await _reportSectionRepository.UpdateReportSection(reportSection);

                return result
                    ? Result<bool>.Success(true, "Report section deleted successfully.")
                    : Result<bool>.Fail("Failed to delete report section.");
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail($"An error occurred: {ex.Message}");
            }
        }
    }
}
