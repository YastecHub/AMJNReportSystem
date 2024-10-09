using AMJNReportSystem.Application.Abstractions.Repositories;
using AMJNReportSystem.Application.Abstractions.Services;
using AMJNReportSystem.Application.Models.DTOs;
using AMJNReportSystem.Application.Models.RequestModels;
using AMJNReportSystem.Application.Models.ResponseModels;
using AMJNReportSystem.Application.Wrapper;
using AMJNReportSystem.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace AMJNReportSystem.Application.Services
{
    public class ReportSectionService : IReportSectionService
    {
        private readonly IReportSectionRepository _reportSectionRepository;
        private readonly IReportTypeRepository _reportTypeRepository;
        private readonly ILogger<ReportSectionService> _logger;

        public ReportSectionService(IReportSectionRepository reportSectionRepository, IReportTypeRepository reportTypeRepository, ILogger<ReportSectionService> logger)
        {
            _reportSectionRepository = reportSectionRepository;
            _reportTypeRepository = reportTypeRepository;
            _logger = logger;
        }

        public async Task<Result<CreateReportSectionResponse>> CreateReportSection(CreateReportSectionRequest request)
        {
            try
            {
                if (request == null)
                {
                    _logger.LogWarning("CreateReportSection was called with a null request.");
                    return Result<CreateReportSectionResponse>.Fail("Request cannot be null.");
                }

                var reportTypeExists = await _reportTypeRepository.GetReportTypeById(request.ReportTypeId);
                if (reportTypeExists == null)
                {
                    _logger.LogWarning($"Report type Id {request.ReportTypeId} not found.");
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
                    _logger.LogInformation($"Report section created successfully with Id: {reportSection.Id}");
                    var response = new CreateReportSectionResponse
                    {
                        Id = reportSection.Id,
                        Message = "Report section created successfully."
                    };

                    return Result<CreateReportSectionResponse>.Success(response);
                }
                else
                {
                    _logger.LogError("Failed to create report section.");
                    return Result<CreateReportSectionResponse>.Fail("Failed to create report section.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a report section.");
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
                    _logger.LogWarning($"Report section with Id {reportSectionId} not found.");
                    return Result<bool>.Fail("Report section not found.");
                }

                existingSection.ReportSectionName = request.ReportSectionName;
                existingSection.ReportSectionValue = request.ReportSectionValue;
                existingSection.Description = request.Description;
                existingSection.ReportTypeId = request.ReportTypeId;

                var isUpdated = await _reportSectionRepository.UpdateReportSection(existingSection);

                if (isUpdated)
                {
                    _logger.LogInformation($"Report section with Id {reportSectionId} updated successfully.");
                    return Result<bool>.Success(true, "Report section updated successfully.");
                }
                else
                {
                    _logger.LogError($"Failed to update report section with Id {reportSectionId}.");
                    return Result<bool>.Fail("Failed to update report section.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating report section with Id {reportSectionId}.", reportSectionId);
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
                    _logger.LogWarning($"Report section with Id {reportSectionId} not found.");
                    return Result<ReportSectionDto>.Fail("Report section not found.");
                }

                var reportSectionDto = new ReportSectionDto
                {
                    Id = reportSection.Id,
                    ReportSectionName = reportSection.ReportSectionName,
                    ReportSectionValue = reportSection.ReportSectionValue,
                    Description = reportSection.Description,
                    ReportTypeId = reportSection.ReportTypeId,
                    IsActive = reportSection.IsActive
                };

                _logger.LogInformation($"Report section with Id {reportSectionId} retrieved successfully.");
                return Result<ReportSectionDto>.Success(reportSectionDto, "Report section retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving report section with Id {reportSectionId}.", reportSectionId);
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
                    _logger.LogInformation($"No report sections found for ReportTypeId {reportTypeId}.");
                    return Result<IEnumerable<ReportSectionDto>>.Fail("No report sections found.");
                }

                var reportSectionDtos = reportSections.Select(rs => new ReportSectionDto
                {
                    Id = rs.Id,
                    ReportSectionName = rs.ReportSectionName,
                    ReportSectionValue = rs.ReportSectionValue,
                    Description = rs.Description,
                    ReportTypeId = rs.ReportTypeId,
                    ReportTypeName = rs.ReportType.Name,
                    IsActive = rs.IsActive
                }).ToList();

                _logger.LogInformation($"{reportSectionDtos.Count} report sections retrieved successfully for ReportTypeId {reportTypeId}.");
                return Result<IEnumerable<ReportSectionDto>>.Success(reportSectionDtos, "Report sections retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving report sections for ReportTypeId {reportTypeId}.", reportTypeId);
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
                    _logger.LogWarning($"Report section with Id {reportSectionId} not found.");
                    return Result<bool>.Fail("Report section not found.");
                }

                reportSection.IsActive = state;
                var isUpdated = await _reportSectionRepository.UpdateReportSection(reportSection);

                if (isUpdated)
                {
                    _logger.LogInformation($"Report section with Id {reportSectionId} activeness updated to {(state ? "active" : "inactive")}");
                    return Result<bool>.Success(true, $"Report section activeness updated to {(state ? "active" : "inactive")}");
                }
                else
                {
                    _logger.LogError($"Failed to update activeness for report section with Id {reportSectionId}.");
                    return Result<bool>.Fail("Failed to update report section activeness.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating activeness for report section with Id {reportSectionId}.", reportSectionId);
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
                    _logger.LogWarning($"Report section with Id {reportSectionId} not found.");
                    return Result<bool>.Fail("Report section not found.");
                }

                reportSection.IsActive = false;
                reportSection.IsDeleted = true;

                var result = await _reportSectionRepository.UpdateReportSection(reportSection);

                if (result)
                {
                    _logger.LogInformation($"Report section with Id {reportSectionId} deleted successfully.");
                    return Result<bool>.Success(true, "Report section deleted successfully.");
                }
                else
                {
                    _logger.LogError($"Failed to delete report section with Id {reportSectionId}.");
                    return Result<bool>.Fail("Failed to delete report section.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting report section with Id {reportSectionId}.", reportSectionId);
                return Result<bool>.Fail($"An error occurred: {ex.Message}");
            }
        }
    }
}
