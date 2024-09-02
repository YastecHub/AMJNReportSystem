using AMJNReportSystem.Application.Abstractions.Repositories;
using AMJNReportSystem.Application.Abstractions.Services;
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

        public ReportSubmissionService(IReportSubmissionRepository reportSubmission, IReportTypeRepository reportTypeRepository, ISubmissionWindowRepository submissionWindowRepository)
        {
            _reportSubmissionRepository = reportSubmission;
            _reportTypeRepository = reportTypeRepository;
            _submissionWindowRepository = submissionWindowRepository;
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
                        Message = "Submission date already exists.",
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
                    DeletedOn = DateTime.Now,
                };
                submission.ReportType.Title = reportSubmissionName;

                await _reportSubmissionRepository.CreateReportSubmissionAsync(submission);

                return new BaseResponse<bool>
                {
                    Message = "Report submission successfully added.",
                    Status = true
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
                    JamaatId = reportSubmission.JamaatId,
                    ReportTypeId = reportSubmission.ReportTypeId,
                    JammatEmailAddress = reportSubmission.JammatEmailAddress,
                    ReportType = reportSubmission.ReportType,
                    ReportSubmissionStatus = reportSubmission.ReportSubmissionStatus,
                    ReportTag = reportSubmission.ReportTag,
                    SubmissionWindowId = reportSubmission.SubmissionWindowId,
                    SubmissionWindow = reportSubmission.SubmissionWindow,
                    Answers = reportSubmission.Answers.Select(x => new ReportResponse
                    {
                        QuestionId = x.QuestionId,
                        Question = x.Question,
                        TextAnswer = x.TextAnswer,
                        QuestionOptionId = x.QuestionOptionId,
                        QuestionOption = x.QuestionOption,
                        Report = x.Report
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

        public async Task<BaseResponse<PaginatedResult<ReportSubmissionResponseDto>>> GetReportTypeSubmissionsAsync(PaginationFilter filter)
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

                var reportSubmission = await _reportSubmissionRepository.GetReportTypeSubmissionsAsync(filter);

                if (reportSubmission == null || reportSubmission.Data.Count == 0)
                {
                    return new BaseResponse<PaginatedResult<ReportSubmissionResponseDto>>
                    {
                        Status = false,
                        Message = "No report type submissions found."
                    };
                }

                return new BaseResponse<PaginatedResult<ReportSubmissionResponseDto>>
                {
                    Status = true,
                    Message = $"{reportSubmission.TotalCount} report type submissions retrieved successfully.",
                    Data = reportSubmission
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<PaginatedResult<ReportSubmissionResponseDto>>
                {
                    Status = false,
                    Message = $"An error occurred while retrieving report type submissions: {ex.Message}",
                };
            }
        }

        public async Task<BaseResponse<PaginatedResult<ReportSubmissionDto>>> GetAllReportTypeSubmissionsAsync(PaginationFilter filter)
        {
            try
            {
                if (filter == null)
                {
                    return new BaseResponse<PaginatedResult<ReportSubmissionDto>>
                    {
                        Status = false,
                        Message = "Pagination filter is required."
                    };
                }

                var paginatedResult = await _reportSubmissionRepository.GetAllReportTypeSubmissionsAsync(filter);
                var dtos = paginatedResult.Data.Select(submission => new ReportSubmissionDto
                {
                    JamaatId = submission.JamaatId,
                    ReportTypeId = submission.ReportTypeId,
                    JammatEmailAddress = submission.JammatEmailAddress,
                    ReportType = submission.ReportType,
                    ReportSubmissionStatus = submission.ReportSubmissionStatus,
                    ReportTag = submission.ReportTag,
                    SubmissionWindowId = submission.SubmissionWindowId,
                    SubmissionWindow = submission.SubmissionWindow,
                    Answers = string.Join(", ", submission.Answers.Select(a => a.TextAnswer.ToString())) 
                }).ToList();

                return new BaseResponse<PaginatedResult<ReportSubmissionDto>>
                {
                    Status = true,
                    Message = $"{paginatedResult.TotalCount} report type submissions retrieved successfully.",
                    Data = new PaginatedResult<ReportSubmissionDto>
                    {
                        TotalCount = paginatedResult.TotalCount,
                        Data = dtos
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<PaginatedResult<ReportSubmissionDto>>
                {
                    Status = false,
                    Message = $"An error occurred while retrieving report type submissions: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponse<ReportSubmissionDto>> UpdateReportSubmission(Guid id, UpdateReportSubmission request)
        {
            // Fetch the existing report submission by ID
            var existingReportSubmission = await _reportSubmissionRepository.GetReportTypeSubmissionByIdAsync(id);

            if (existingReportSubmission == null)
            {
                return new BaseResponse<ReportSubmissionDto>
                {
                    Status = false,
                    Message = "Report submission not found."
                };
            }

            // Update the properties with the values from the request DTO
            existingReportSubmission.JammatEmailAddress = request.JammatEmailAddress;
            existingReportSubmission.ReportType = request.ReportType;
            existingReportSubmission.ReportSubmissionStatus = request.ReportSubmissionStatus;
            existingReportSubmission.ReportTag = request.ReportTag;
            existingReportSubmission.SubmissionWindowId = request.SubmissionWindowId;
            existingReportSubmission.SubmissionWindow = request.SubmissionWindow;

            // Update specific answers (if needed)
            // Uncomment and fix the logic if you need to update answers
            //foreach (var answerDto in request.Answers)
            //{
            //    var existingAnswer = existingReportSubmission.Answers
            //        .FirstOrDefault(a => a.Id == answerDto.Id);

            //    if (existingAnswer != null)
            //    {
            //        existingAnswer.Question = answerDto.Question;
            //        existingAnswer.Answer = answerDto.Answer;
            //    }
            //    else
            //    {
            //        existingReportSubmission.Answers.Add(new ReportResponse
            //        {
            //            Id = Guid.NewGuid(),
            //            Question = answerDto.Question,
            //            Answer = answerDto.Answer
            //        });
            //    }
            //}
            await _reportSubmissionRepository.UpdateReportSubmission(existingReportSubmission);
            var reportSubmissionDto = new ReportSubmissionDto
            {
                JammatEmailAddress = existingReportSubmission.JammatEmailAddress,
                ReportType = existingReportSubmission.ReportType,
                ReportSubmissionStatus = existingReportSubmission.ReportSubmissionStatus,
                ReportTag = existingReportSubmission.ReportTag,
                SubmissionWindowId = existingReportSubmission.SubmissionWindowId,
                SubmissionWindow = existingReportSubmission.SubmissionWindow,
                //Answers = existingReportSubmission.Answers.Select(a => new ReportResponseDto
                //{
                //    Id = a.Id,
                //    Question = a.Question,
                //    Answer = a.Answer
                //}).ToList()
            };

            return new BaseResponse<ReportSubmissionDto>
            {
                Status = true,
                Data = reportSubmissionDto,
                Message = "Report submission updated successfully."
            };
        }

    }
}
