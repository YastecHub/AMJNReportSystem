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

        public async Task<BaseResponse<ReportSectionDto>> CreateReportSection(CreateReportSectionRequest request)
        {
            try
            {
                if (request == null)
                {
                    _logger.LogWarning("CreateReportSection was called with a null request.");
                    return new BaseResponse<ReportSectionDto>
                    {
                        Status = false,
                        Message = "Request cannot be null"
                    };
                }


                var reportTypeExists = await _reportTypeRepository.GetReportTypeById(request.ReportTypeId);
                if (reportTypeExists == null)
                {
                    _logger.LogWarning($"Report type Id {request.ReportTypeId} not found.");
                    return new BaseResponse<ReportSectionDto>
                    {
                        Status = false,
                        Message = $"Report type Id {request.ReportTypeId} not found."
                    };
                }
                _logger.LogInformation("Checking if report section value {ReportSectionValue} exists.", request.ReportSectionValue);
                var sectionValueExists = await _reportSectionRepository.ExistByValueAsync(request.ReportTypeId, request.ReportSectionValue);

                if (sectionValueExists)
                {
                    _logger.LogWarning("Report section value {ReportSectionValue} already exists.", request.ReportSectionValue);
                    return new BaseResponse<ReportSectionDto>
                    {
                        Status = false,
                        Message = "Report Section Value already exists"
                    };
                }

                var reportSectionExist = await _reportSectionRepository.ReportSectionExist(request.ReportSectionName);
                if (reportSectionExist)
                {
                    _logger.LogWarning($"Report section with name '{request.ReportSectionName} already exists.");
                    return new BaseResponse<ReportSectionDto>
                    {
                        Status = false,
                        Message = $"Report section with name {request.ReportSectionName} already exists."
                    };
                }

                var reportSection = new ReportSection
                {
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

                    return new BaseResponse<ReportSectionDto>
                    {
                        Status = true,
                        Message = "Report section created successfully",
                        Data = new ReportSectionDto
                        {
                            Id = reportSection.Id,
                            ReportSectionName = reportSection.ReportSectionName,
                            ReportSectionValue = reportSection.ReportSectionValue,
                            Description = reportSection.Description,
                            ReportTypeId = reportSection.ReportTypeId,
                            IsActive = reportSection.IsActive
                        }
                    };
                }
                else
                {
                    _logger.LogError("Failed to create report section.");
                    return new BaseResponse<ReportSectionDto>
                    {
                        Status = false,
                        Message = "Failed to create report section."
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a report section.");
                return new BaseResponse<ReportSectionDto>
                {
                    Status = false,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponse<bool>> UpdateReportSection(Guid reportSectionId, UpdateReportSectionRequest request)
        {
            try
            {
                var existingSection = await _reportSectionRepository.GetReportSectionById(reportSectionId);
                if (existingSection == null)
                {
                    _logger.LogWarning($"Report section with Id {reportSectionId} not found.");
                    return new BaseResponse<bool>
                    {
                        Status = false,
                        Message = $"Report section with Id {reportSectionId} not found."
                    };
                }

                existingSection.ReportSectionName = request.ReportSectionName;
                existingSection.ReportSectionValue = request.ReportSectionValue;
                existingSection.Description = request.Description;
                existingSection.ReportTypeId = request.ReportTypeId;

                var isUpdated = await _reportSectionRepository.UpdateReportSection(existingSection);

                if (isUpdated)
                {
                    _logger.LogInformation($"Report section with Id {reportSectionId} updated successfully.");
                    return new BaseResponse<bool>
                    {
                        Status = true,
                        Message = "Report section updated successfully."
                    };
                }
                else
                {
                    _logger.LogError($"Failed to update report section with Id {reportSectionId}.");
                    return new BaseResponse<bool>
                    {
                        Status = false,
                        Message = "Failed to update report section."
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating report section with Id {reportSectionId}.", reportSectionId);
                return new BaseResponse<bool>
                {
                    Status = false,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponse<ReportSectionDto>> GetReportSection(Guid reportSectionId)
        {
            try
            {
                var reportSection = await _reportSectionRepository.GetReportSectionById(reportSectionId);
                if (reportSection == null)
                {
                    _logger.LogWarning($"Report section with Id {reportSectionId} not found.");
                    return new BaseResponse<ReportSectionDto>
                    {
                        Status = false,
                        Message = "Report section not found."
                    };
                }

                var reportSectionDto = new ReportSectionDto
                {
                    Id = reportSection.Id,
                    ReportTypeName = reportSection.ReportType.Name,
                    ReportSectionName = reportSection.ReportSectionName,
                    ReportSectionValue = reportSection.ReportSectionValue,
                    Description = reportSection.Description,
                    ReportTypeId = reportSection.ReportTypeId,
                    IsActive = reportSection.IsActive,
                };

                _logger.LogInformation($"Report section with Id {reportSectionId} retrieved successfully.");
                return new BaseResponse<ReportSectionDto>
                {
                    Status = true,
                    Message = "Report section retrieved successfully.",
                    Data = reportSectionDto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving report section with Id {reportSectionId}.", reportSectionId);
                return new BaseResponse<ReportSectionDto>
                {
                    Status = false,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<ReportSectionDto>>> GetReportSections(Guid reportTypeId)
        {
            try
            {
                var reportSections = await _reportSectionRepository.GetReportSections(rs => rs.ReportTypeId == reportTypeId);
                if (!reportSections.Any())
                {
                    _logger.LogInformation($"No report sections found for ReportTypeId {reportTypeId}.");
                    return new BaseResponse<IEnumerable<ReportSectionDto>>
                    {
                        Status = false,
                        Message = "No report sections found.",
                        Data = Enumerable.Empty<ReportSectionDto>()
                    };
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
                return new BaseResponse<IEnumerable<ReportSectionDto>>
                {
                    Status = true,
                    Message = "Report sections retrieved successfully.",
                    Data = reportSectionDtos
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving report sections for ReportTypeId {reportTypeId}.", reportTypeId);
                return new BaseResponse<IEnumerable<ReportSectionDto>>
                {
                    Status = false,
                    Message = $"An error occurred: {ex.Message}",
                    Data = Enumerable.Empty<ReportSectionDto>()
                };
            }
        }

        public async Task<BaseResponse<bool>> SetReportSectionActiveness(Guid reportSectionId, bool state)
        {
            try
            {
                var reportSection = await _reportSectionRepository.GetReportSectionById(reportSectionId);
                if (reportSection == null)
                {
                    _logger.LogWarning($"Report section with Id {reportSectionId} not found.");
                    return new BaseResponse<bool>
                    {
                        Status = false,
                        Message = "Report section not found.",
                    };
                }

                reportSection.IsActive = state;
                var isUpdated = await _reportSectionRepository.UpdateReportSection(reportSection);

                if (isUpdated)
                {
                    _logger.LogInformation($"Report section with Id {reportSectionId} activeness updated to {(state ? "active" : "inactive")}");
                    return new BaseResponse<bool>
                    {
                        Status = true,
                        Message = $"Report section activeness updated to {(state ? "active" : "inactive")}.",
                    };
                }
                else
                {
                    _logger.LogError($"Failed to update activeness for report section with Id {reportSectionId}.");
                    return new BaseResponse<bool>
                    {
                        Status = false,
                        Message = "Failed to update report section activeness.",
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating activeness for report section with Id {reportSectionId}.", reportSectionId);
                return new BaseResponse<bool>
                {
                    Status = false,
                    Message = $"An error occurred: {ex.Message}",
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteReportSection(Guid reportSectionId)
        {
            try
            {
                var reportSection = await _reportSectionRepository.GetReportSectionById(reportSectionId);
                if (reportSection == null)
                {
                    _logger.LogWarning($"Report section with Id {reportSectionId} not found.");
                    return new BaseResponse<bool>
                    {
                        Status = false,
                        Message = "Report section not found.",
                    };
                }

                reportSection.IsActive = false;
                reportSection.IsDeleted = true;

                var result = await _reportSectionRepository.UpdateReportSection(reportSection);

                if (result)
                {
                    _logger.LogInformation($"Report section with Id {reportSectionId} deleted successfully.");
                    return new BaseResponse<bool>
                    {
                        Status = false,
                        Message = "Report section deleted successfully."
                    };
                }
                else
                {
                    _logger.LogError($"Failed to delete report section with Id {reportSectionId}.");
                    return new BaseResponse<bool>
                    {
                        Status = false,
                        Message = "Failed to delete report section."
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting report section with Id {reportSectionId}.", reportSectionId);
                return new BaseResponse<bool>
                {
                    Status = false,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }
    }
}
