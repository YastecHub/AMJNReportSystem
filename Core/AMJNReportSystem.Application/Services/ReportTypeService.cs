using AMJNReportSystem.Application.Abstractions.Repositories;
using AMJNReportSystem.Application.Abstractions.Services;
using AMJNReportSystem.Application.Models.DTOs;
using AMJNReportSystem.Application.Models.RequestModels;
using AMJNReportSystem.Application.Wrapper;
using AMJNReportSystem.Domain.Entities;

namespace AMJNReportSystem.Application.Services
{
    public class ReportTypeService : IReportTypeService
    {
        private readonly IReportTypeRepository _reportTypeRepository;

        public ReportTypeService(IReportTypeRepository reportTypeRepository)
        {
            _reportTypeRepository = reportTypeRepository;
        }

        public async Task<BaseResponse<Guid>> CreateReportType(CreateReportTypeRequest request)
        {
            try
            {
                if (request == null)
                {
                    return new BaseResponse<Guid>
                    {
                        Message = "Request cannot be null",
                        Status = false,
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

                var createdReportType = await _reportTypeRepository.CreateReportType(reportType);

                return new BaseResponse<Guid>
                {
                    Message = "Report type created successfully",
                    Status = true,  
                    Data = reportType.Id,
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Guid>
                {
                    Message = $"Failed to create report type: {ex.Message}",
                    Status = false,
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<ReportTypeDto>>>GetQaidReportTypes()
        {
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

                return new BaseResponse<IEnumerable<ReportTypeDto>>
                {
                    Data = reportTypeDtos,
                    Status = true,
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<ReportTypeDto>>
                {
                    Message = $"Failed to retrieve report types: {ex.Message}",
                    Status = false
                };
            }
        }

        public async Task<BaseResponse<ReportTypeDto>> GetReportType(Guid reportTypeId)
        {
            try
            {
                var reportType = await _reportTypeRepository.GetReportTypeById(reportTypeId);
                if (reportType == null)
                {
                    return new BaseResponse<ReportTypeDto>
                    {
                        Message = "Report type not found",
                        Status = false
                    };
                }

                var reportTypeDto = new ReportTypeDto
                {
                    Title = reportType.Title,
                    Description = reportType.Description,
                    Year = reportType.Year,
                    Questions = reportType.Questions
                };

                return new BaseResponse<ReportTypeDto>
                {
                    Data = reportTypeDto,
                    Status = true
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ReportTypeDto>
                {
                    Message = $"Failed to retrieve report type: {ex.Message}",
                    Status = false
                };
            }
        }

        public async Task<BaseResponse<bool>> SetReportTypeActiveness(Guid reportTypeId, bool state)
        {
            try
            {
                var reportType = await _reportTypeRepository.GetReportTypeById(reportTypeId);
                if (reportType == null)
                {
                    return new BaseResponse<bool>
                    {
                        Message = "Report type not found",
                        Status = false
                    };
                }

                reportType.IsActive = state;
                await _reportTypeRepository.UpdateReportType(reportType);

                return new BaseResponse<bool>
                {
                    Data = true,
                    Status = true,
                    Message = "Report type activeness updated successfully"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>
                {
                    Message = $"Failed to update report type activeness: {ex.Message}",
                    Status = false
                };
            }
        }


        public async Task<BaseResponse<IEnumerable<ReportTypeDto>>> GetReportTypes()
        {
            try
            {
                var report = await _reportTypeRepository.GetAllReportTypes();
                var reportTypeDtos = report.Select(r => new ReportTypeDto

                {
                    Title = r.Title,
                    Description = r.Description,
                    Year = r.Year,
                }).ToList();



                return new BaseResponse<IEnumerable<ReportTypeDto>>
                {
                    Message = "Record Found Successfully",
                    Status = true,
                    Data = reportTypeDtos,
                };
            }
            catch (Exception)
            {
                return new BaseResponse<IEnumerable<ReportTypeDto>>
                {
                    Status = false,
                    Message = "Failed",
                };

            }



        }


        public async Task<BaseResponse<bool>> UpdateReportType(Guid reportTypeId, UpdateReportTypeRequest request)
        {
            try
            {
                var existingReportType = await _reportTypeRepository.GetReportTypeById(reportTypeId);
                if (existingReportType == null)
                {
                    return new BaseResponse<bool>
                    {
                        Message = "Report type not found",
                        Status = false
                    };
                }

                existingReportType.Description = request.Description;
                existingReportType.Name = request.Name;
                existingReportType.Year = request.Year;
                existingReportType.Title = request.Title;
                existingReportType.ReportTag = request.ReportTag;

                await _reportTypeRepository.UpdateReportType(existingReportType);

                return new BaseResponse<bool>
                {
                    Data = true,
                    Message = "Report type updated successfully",
                    Status = true
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>
                {
                    Message = $"Failed to update report type: {ex.Message}",
                    Status = false
                };
            }
        }
    }
}
