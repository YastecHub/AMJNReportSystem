using AMJNReportSystem.Application.Abstractions.Repositories;
using AMJNReportSystem.Application.Abstractions.Services;
using AMJNReportSystem.Application.Interfaces;
using AMJNReportSystem.Application.Models;
using AMJNReportSystem.Application.Models.DTOs;
using AMJNReportSystem.Application.Models.RequestModels;
using AMJNReportSystem.Application.Models.RequestModels.Reports;
using AMJNReportSystem.Application.Models.ResponseModels;
using AMJNReportSystem.Application.Wrapper;
using AMJNReportSystem.Domain.Entities;
using Mapster;

namespace AMJNReportSystem.Application.Services
{
    public class ReportSubmissionService : IReportSubmissionService
    {
        private readonly IReportSubmissionRepository _reportSubmissionRepository;
        private readonly IReportTypeRepository _reportTypeRepository;
        private readonly ISubmissionWindowRepository _submissionWindowRepository;
        private readonly ICurrentUser _currentUser;

        public ReportSubmissionService(IReportSubmissionRepository reportSubmission, IReportTypeRepository reportTypeRepository, ISubmissionWindowRepository submissionWindowRepository, ICurrentUser currentUser)
        {
            _reportSubmissionRepository = reportSubmission;
            _reportTypeRepository = reportTypeRepository;
            _submissionWindowRepository = submissionWindowRepository;
            _currentUser = currentUser;
        }

        public async Task<BaseResponse<bool>> CreateReportTypeSubmissionAsync(CreateReportSubmissionRequest request)
        {
            try
            {
                if (request == null)
                {
                    return new BaseResponse<bool>
                    {
                        Message = "Report submission not found.",
                        Status = false
                    };
                }

                var reportType = await _reportTypeRepository.GetReportTypeById(request.ReportTypeId);
                if (reportType == null)
                {
                    return new BaseResponse<bool>
                    {
                        Message = "Report Type not found.",
                        Status = false
                    };
                }

                var reportSubmissionName = $"{reportType.Title}_{request.Year}_{request.Month}";
                var reportSubmissionCheckerExist = await _reportSubmissionRepository.Exist(reportSubmissionName);
                if (reportSubmissionCheckerExist)
                {
                    return new BaseResponse<bool>
                    {
                        Message = "Submission  already exists.",
                        Status = false
                    };
                }
                var submission = new ReportSubmission
                {
                    JamaatId = request.JamaatId,
                    ReportTypeId = request.ReportTypeId,
                    JammatEmailAddress = request.JammatEmailAddress,
                    ReportType = reportType,
                    ReportSubmissionStatus = request.ReportSubmissionStatus,
                    ReportTag = request.ReportTag,
                    SubmissionWindowId = request.SubmissionWindowId,
                    Answers = new List<ReportResponse>(),
                    CreatedBy = request.CreatedBy,
                    CreatedOn = DateTime.Now,
                };
                submission.ReportType.Title = reportSubmissionName;

                await _reportSubmissionRepository.CreateReportSubmissionAsync(submission);

                return new BaseResponse<bool>
                {
                    Message = "Report submission successfully added.",
                    Status = true,
                     Data = true,
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>
                {
                    Message = $"An error occurred: {ex.Message}",
                    Status = false
                };
            }
        }


        public async Task<BaseResponse<ReportSubmissionResponseDto>> GetReportTypeSubmissionByIdAsync(Guid reportTypeSubmissionId)
        {
            try
            {
                var reportSubmission = await _reportSubmissionRepository.GetReportTypeSubmissionByIdAsync(reportTypeSubmissionId);

                if (reportSubmission == null)
                {
                    return new BaseResponse<ReportSubmissionResponseDto>
                    {
                        Status = false,
                        Message = "Report submission Type not found"
                    };
                }
                var reportSubmissionResponse = new ReportSubmissionResponseDto
                {
                    JammatEmailAddress = reportSubmission.JammatEmailAddress,
                    ReportTypeName = reportSubmission.ReportType.Name,
                    ReportSubmissionStatus = reportSubmission.ReportSubmissionStatus,
                    ReportTag = reportSubmission.ReportTag,
                    SubmissionWindowMonth = reportSubmission.SubmissionWindow.Month,
                    SubmissionWindowYear = reportSubmission.SubmissionWindow.Year,
                    Answers = reportSubmission.Answers.Select(x => new ReportResponseDto
                    {
                        QuestionId = x.QuestionId,
                        TextAnswer = x.TextAnswer,
                        QuestionOptionId = x.QuestionOptionId,
                    }).ToList()
                };

                return new BaseResponse<ReportSubmissionResponseDto>
                {
                    Status = true,
                    Message = "Report submission successfully retrieved",
                    Data = reportSubmissionResponse
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ReportSubmissionResponseDto>
                {
                    Status = false,
                    Message = "An error occurred while retrieving the report submission."
                };
            }
        }

        public async Task<BaseResponse<PaginatedResult<ReportSubmissionResponseDto>>> GetAllReportTypeSubmissionsAsync(PaginationFilter filter)
        {
            try
            {
                if (filter == null)
                {
                    return new BaseResponse<PaginatedResult<ReportSubmissionResponseDto>>
                    {
                        Status = false,
                        Message = "Pagination filter is required."
                    };
                }

                var paginatedResult = await _reportSubmissionRepository.GetAllReportTypeSubmissionsAsync(filter);
                var dtos = paginatedResult.Data.Select(submission => new ReportSubmissionResponseDto
                {
                    JammatEmailAddress = submission.JammatEmailAddress,
                    ReportTypeName = submission.ReportType.Name,
                    ReportSubmissionStatus = submission.ReportSubmissionStatus,
                    ReportTag = submission.ReportTag,
                    SubmissionWindowMonth = submission.SubmissionWindow.Month,
                    SubmissionWindowYear = submission.SubmissionWindow.Year,
                    Answers = submission.Answers.Select(a => new ReportResponseDto
                    {
                        TextAnswer = a.TextAnswer,
                        Id = a.Id,
                        QuestionId = a.QuestionId,
                        QuestionOptionId = a.QuestionOptionId,
                        Report = a.Report
                    }).ToList()
                }).ToList();

                return new BaseResponse<PaginatedResult<ReportSubmissionResponseDto>>
                {
                    Status = true,
                    Message = $"{paginatedResult.TotalCount} report type submissions retrieved successfully.",
                    Data = new PaginatedResult<ReportSubmissionResponseDto>
                    {
                        TotalCount = paginatedResult.TotalCount,
                        Data = dtos
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<PaginatedResult<ReportSubmissionResponseDto>>
                {
                    Status = false,
                    Message = $"An error occurred while retrieving report type submissions: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponse<ReportSubmissionDto>> UpdateReportSubmission(Guid id, UpdateReportSubmission request)
        {
            try
            {
                var existingReportSubmission = await _reportSubmissionRepository.GetReportTypeSubmissionByIdAsync(id);

                if (existingReportSubmission == null)
                {
                    return new BaseResponse<ReportSubmissionDto>
                    {
                        Status = false,
                        Message = "Report submission not found."
                    };
                }

                id = request.ReportSubmissionId;
                existingReportSubmission.JammatEmailAddress = request.JammatEmailAddress;
                existingReportSubmission.ReportSubmissionStatus = request.ReportSubmissionStatus;
                existingReportSubmission.ReportTag = request.ReportTag;
                existingReportSubmission.LastModifiedOn = DateTime.Now;
                existingReportSubmission.LastModifiedBy = request.LastModifiedBy;
                await _reportSubmissionRepository.UpdateReportSubmission(existingReportSubmission);
                var reportSubmissionDto = new ReportSubmissionDto
                {
                    JamaatId = existingReportSubmission.JamaatId,
                    ReportTypeId = existingReportSubmission.ReportTypeId,
                    JammatEmailAddress = existingReportSubmission.JammatEmailAddress,
                    ReportType = existingReportSubmission.ReportType,
                    ReportSubmissionStatus = existingReportSubmission.ReportSubmissionStatus,
                    ReportTag = existingReportSubmission.ReportTag,
                    SubmissionWindowId = existingReportSubmission.SubmissionWindowId,
                    SubmissionWindow = existingReportSubmission.SubmissionWindow,
                    Answers = existingReportSubmission.Answers.Select(a => new ReportResponseDto
                    {
                        Id = a.Id,
                        QuestionId = a.QuestionId,
                        TextAnswer = a.TextAnswer,
                        QuestionOptionId = a.QuestionOptionId,
                        Report = a.Report
                    }).ToList()
                };

                return new BaseResponse<ReportSubmissionDto>
                {
                    Status = true,
                    Data = reportSubmissionDto,
                    Message = "Report submission updated successfully."
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ReportSubmissionDto>
                {
                    Status = false,
                    Message = $"An error occurred while updating the report submission: {ex.Message}"
                };
            }
        }

        public async Task<Result<bool>> DeleteReportSubmission(Guid reportSubmissionId)
        {
            try
            {
                var reportSubmission = await _reportSubmissionRepository.GetReportTypeSubmissionByIdAsync(reportSubmissionId);
                if (reportSubmission == null)
                {
                    return Result<bool>.Fail("Report section not found.");
                }
                reportSubmission.IsDeleted = true;
                reportSubmission.DeletedOn = DateTime.Now;
                reportSubmission.DeletedBy = _currentUser.Name;

                var result = await _reportSubmissionRepository.UpdateReportSubmission(reportSubmission);
                return new Result<bool>
                {
                    Succeeded = true,
                    Messages = new List<string> { "Report submission deleted successfully." }
                };
                
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail($"An error occurred: {ex.Message}");
            }
        }

    }
}
