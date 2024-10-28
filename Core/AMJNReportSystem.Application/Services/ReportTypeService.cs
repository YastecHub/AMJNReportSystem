using AMJNReportSystem.Application.Abstractions.Repositories;
using AMJNReportSystem.Application.Abstractions.Services;
using AMJNReportSystem.Application.Interfaces;
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
        private readonly ICurrentUser _currentUser;

        public ReportTypeService(IReportTypeRepository reportTypeRepository, ILogger<ReportTypeService> logger, ICurrentUser currentUser)
        {
            _reportTypeRepository = reportTypeRepository;
            _logger = logger;
            _currentUser = currentUser;
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
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Description = request.Description,
                    Year = request.Year,
                    CreatedBy = _currentUser.Name,
                    CreatedOn = DateTime.Now
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
                    Description = reportType.Description,
                    Year = reportType.Year,
                    CreatedBy = _currentUser.Name,
                    Id = reportType.Id,
                    LastModifiedBy = reportType.LastModifiedBy,
                    LastModifiedOn = reportType.LastModifiedOn
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

        public async Task<BaseResponse<IEnumerable<ReportTypeDto>>> GetReportTypes()
        {
            _logger.LogInformation("Starting GetReportTypes method.");
            try
            {
                var currentDate = DateTime.Today;
                var report = await _reportTypeRepository.GetAllReportTypes();
                var reportTypeDtos = report.Where(x => !x.IsDeleted).Select(r => new ReportTypeDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    Year = r.Year,
                    CreatedBy = _currentUser.Name,
                    LastModifiedBy = r.LastModifiedBy,
                    LastModifiedOn = r.LastModifiedOn,
                    SubmissionWindowId = r.SubmissionWindows.Count() > 0 ?
                    r.SubmissionWindows.Where(x => currentDate >= x.StartingDate && currentDate <= x.EndingDate).Select(x => x.Id)
                    .FirstOrDefault() : null,
                    SubmissionWindowIsActive = r.SubmissionWindows.Any(x => currentDate >= x.StartingDate && currentDate <= x.EndingDate),
                }).OrderByDescending(x => x.CreatedOn)
                  .ToList();

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

                existingReportType.Description = request.Description;
                existingReportType.Name = request.Name;
                existingReportType.Year = request.Year;

                await _reportTypeRepository.UpdateReportType(existingReportType);

                var reportType = new ReportType
                {
                    CreatedBy = _currentUser.Name,
                    LastModifiedBy = _currentUser.Name,
                    LastModifiedOn = existingReportType.LastModifiedOn,
                    Description = existingReportType.Description,
                    Year = existingReportType.Year,
                };
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



        public async Task<Result<bool>> DeleteReportType(Guid reportTypeId)
        {
            try
            {
                var reportSection = await _reportTypeRepository.GetReportTypeById(reportTypeId);
                if (reportSection == null)
                {
                    _logger.LogWarning($"Report Type with Id {reportTypeId} not found.");
                    return Result<bool>.Fail("Report Type not found.");
                }

                reportSection.IsDeleted = true;

                var result = await _reportTypeRepository.UpdateReportType(reportSection);

                if (result)
                {
                    _logger.LogInformation($"Report Type with Id {reportTypeId} deleted successfully.");
                    return Result<bool>.Success(true, "Report Type deleted successfully.");
                }
                else
                {
                    _logger.LogError($"Failed to delete report type with Id {reportTypeId}.");
                    return Result<bool>.Fail("Failed to delete report Type.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting report Type with Id {reportSectionId}.", reportTypeId);
                return Result<bool>.Fail($"An error occurred: {ex.Message}");
            }
        }
    }
}
