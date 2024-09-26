using AMJNReportSystem.Application.Abstractions.Repositories;
using AMJNReportSystem.Application.Abstractions.Services;
using AMJNReportSystem.Application.Models.DTOs;
using AMJNReportSystem.Application.Models.RequestModels;
using AMJNReportSystem.Application.Wrapper;
using AMJNReportSystem.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace AMJNReportSystem.Application.Services
{
    public class ReportTypeService : IReportTypeService
    {
        private readonly IReportTypeRepository _reportTypeRepository;
        private readonly ILogger<ReportTypeService> _logger;

        public ReportTypeService(IReportTypeRepository reportTypeRepository, ILogger<ReportTypeService> logger)
        {
            _reportTypeRepository = reportTypeRepository;
            _logger = logger;
        }

        public async Task<BaseResponse<Guid>> CreateReportType(CreateReportTypeRequest request)
        {
            _logger.LogInformation("Starting CreateReportType method.");
            try
            {
                if (request == null)
                {
                    _logger.LogWarning("CreateReportType request is null.");
                    return new BaseResponse<Guid>
                    {
                        Message = "Request cannot be null",
                        Status = false,
                    };
                }

                _logger.LogInformation("Checking if report type with name {ReportName} exists.", request.Name);
                var reportExists = await _reportTypeRepository.Exist(request.Name);

                if (reportExists)
                {
                    _logger.LogWarning("Report type with name {ReportName} already exists.", request.Name);
                    return new BaseResponse<Guid>
                    {
                        Status = false,
                        Message = "Report Name already exists"
                    };
                }

                var reportType = new ReportType
                {
                    Name = request.Name,
                    Title = request.Title,
                    Description = request.Description,
                    ReportTag = request.ReportTag,
                    Year = request.Year,
                };

                _logger.LogInformation("Creating new report type {ReportName}.", request.Name);
                var createdReportType = await _reportTypeRepository.CreateReportType(reportType);

                _logger.LogInformation("Report type created successfully with Id {ReportId}.", reportType.Id);
                return new BaseResponse<Guid>
                {
                    Message = "Report type created successfully",
                    Status = true,
                    Data = reportType.Id,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating report type.");
                return new BaseResponse<Guid>
                {
                    Message = $"Failed to create report type: {ex.Message}",
                    Status = false,
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<ReportTypeDto>>> GetQaidReportTypes()
        {
            _logger.LogInformation("Starting GetQaidReportTypes method.");
            try
            {
                var reportTypes = await _reportTypeRepository.GetAllReportTypes();
                var reportTypeDtos = reportTypes.Select(r => new ReportTypeDto
                {
                    Name = r.Name,
                    IsActive = r.IsActive,
                    Title = r.Title,
                    Description = r.Description,
                    Year = r.Year,
                }).ToList();

                _logger.LogInformation("Successfully retrieved {Count} report types.", reportTypeDtos.Count);
                return new BaseResponse<IEnumerable<ReportTypeDto>>
                {
                    Data = reportTypeDtos,
                    Status = true,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving report types.");
                return new BaseResponse<IEnumerable<ReportTypeDto>>
                {
                    Message = $"Failed to retrieve report types: {ex.Message}",
                    Status = false
                };
            }
        }

        public async Task<BaseResponse<ReportTypeDto>> GetReportType(Guid reportTypeId)
        {
            _logger.LogInformation("Starting GetReportType method for Id {ReportTypeId}.", reportTypeId);
            try
            {
                var reportType = await _reportTypeRepository.GetReportTypeById(reportTypeId);
                if (reportType == null)
                {
                    _logger.LogWarning("Report type with Id {ReportTypeId} not found.", reportTypeId);
                    return new BaseResponse<ReportTypeDto>
                    {
                        Message = "Report type not found",
                        Status = false
                    };
                }

                var reportTypeDto = new ReportTypeDto
                {
                    Name = reportType.Name,
                    Title = reportType.Title,
                    Description = reportType.Description,
                    Year = reportType.Year,
                    Questions = reportType.Questions,
                    ReportTag = reportType.ReportTag
                };

                _logger.LogInformation("Successfully retrieved report type with Id {ReportTypeId}.", reportTypeId);
                return new BaseResponse<ReportTypeDto>
                {
                    Data = reportTypeDto,
                    Status = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving report type with Id {ReportTypeId}.", reportTypeId);
                return new BaseResponse<ReportTypeDto>
                {
                    Message = $"Failed to retrieve report type: {ex.Message}",
                    Status = false
                };
            }
        }

        public async Task<BaseResponse<bool>> SetReportTypeActiveness(Guid reportTypeId, bool state)
        {
            _logger.LogInformation("Starting SetReportTypeActiveness method for Id {ReportTypeId} with state {State}.", reportTypeId, state);
            try
            {
                var reportType = await _reportTypeRepository.GetReportTypeById(reportTypeId);
                if (reportType == null)
                {
                    _logger.LogWarning("Report type with Id {ReportTypeId} not found.", reportTypeId);
                    return new BaseResponse<bool>
                    {
                        Message = "Report type not found",
                        Status = false
                    };
                }

                reportType.IsActive = state;
                await _reportTypeRepository.UpdateReportType(reportType);

                _logger.LogInformation("Successfully updated activeness of report type .");
                return new BaseResponse<bool>
                {
                    Data = true,
                    Status = true,
                    Message = "Report type activeness updated successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating report type activeness.");
                return new BaseResponse<bool>
                {
                    Message = $"Failed to update report type activeness: {ex.Message}",
                    Status = false
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<ReportTypeDto>>> GetReportTypes()
        {
            _logger.LogInformation("Starting GetReportTypes method.");
            try
            {
                var report = await _reportTypeRepository.GetAllReportTypes();
                var reportTypeDtos = report.Select(r => new ReportTypeDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Title = r.Title,
                    Description = r.Description,
                    Year = r.Year,
                    ReportTag = r.ReportTag
                }).ToList();

                _logger.LogInformation("Successfully retrieved report types.");
                return new BaseResponse<IEnumerable<ReportTypeDto>>
                {
                    Message = "Record Found Successfully",
                    Status = true,
                    Data = reportTypeDtos,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving report types.");
                return new BaseResponse<IEnumerable<ReportTypeDto>>
                {
                    Status = false,
                    Message = "Failed",
                };
            }
        }

        public async Task<BaseResponse<bool>> UpdateReportType(Guid reportTypeId, UpdateReportTypeRequest request)
        {
            _logger.LogInformation("Starting UpdateReportType method for Id {ReportTypeId}.", reportTypeId);
            try
            {
                var existingReportType = await _reportTypeRepository.GetReportTypeById(reportTypeId);
                if (existingReportType == null)
                {
                    _logger.LogWarning($"Report type with Id {reportTypeId} not found.");
                    return new BaseResponse<bool>
                    {
                        Message = "Report type not found",
                        Status = false
                    };
                }

                var reportExists = await _reportTypeRepository.Exist(request.Name);

                if (reportExists)
                {
                    _logger.LogWarning($"Report type with name {request.Name} already exists.");
                    return new BaseResponse<bool>
                    {
                        Status = false,
                        Message = "Report Name already exists"
                    };
                }

                existingReportType.Description = request.Description;
                existingReportType.Name = request.Name;
                existingReportType.Year = request.Year;
                existingReportType.Title = request.Title;
                existingReportType.ReportTag = request.ReportTag;

                await _reportTypeRepository.UpdateReportType(existingReportType);

                _logger.LogInformation("Successfully updated report type with Id {ReportTypeId}.", reportTypeId);
                return new BaseResponse<bool>
                {
                    Data = true,
                    Message = "Report type updated successfully",
                    Status = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating report type with Id {ReportTypeId}.", reportTypeId);
                return new BaseResponse<bool>
                {
                    Message = $"Failed to update report type: {ex.Message}",
                    Status = false
                };
            }
        }
    }
}
