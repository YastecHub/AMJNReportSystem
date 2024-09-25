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
        private readonly IQuestionRepository _questionRepository;

        public ReportSubmissionService(IReportSubmissionRepository reportSubmission,
            IReportTypeRepository reportTypeRepository, ISubmissionWindowRepository
            submissionWindowRepository, ICurrentUser currentUser, IQuestionRepository questionRepository)
        {
            _reportSubmissionRepository = reportSubmission;
            _reportTypeRepository = reportTypeRepository;
            _submissionWindowRepository = submissionWindowRepository;
            _currentUser = currentUser;
            _questionRepository = questionRepository;
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
                    JamaatId = _currentUser.GetJamaatId(),
                    CircuitId = _currentUser.GetCircuit(),
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
                if (request.ReportResponses != null && request.ReportResponses.Count > 0)
                {
                    foreach (var response in request.ReportResponses)
                    {
                        var question = await _questionRepository.GetQuestionById(response.QuestionId);
                        if (question == null)
                        {
                            return new BaseResponse<bool>
                            {
                                Message = $"Question with ID {response.QuestionId} not found.",
                                Status = false
                            };
                        }
                        var reportResponse = new ReportResponse
                        {

                            QuestionId = response.QuestionId,
                            Question = question,
                            TextAnswer = response.TextAnswer,
                            QuestionOptionId = response.QuestionOptionId,
                            CreatedBy = request.CreatedBy,
                            CreatedOn = DateTime.Now,
                            Report = response.Report
                        };

                        submission.Answers.Add(reportResponse);
                    }
                }

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
                    JamaatId = _currentUser.GetJamaatId(),
                    CircuitId = _currentUser.GetCircuit(),
                    JammatEmailAddress = reportSubmission.JammatEmailAddress,
                    ReportTypeName = reportSubmission.ReportType.Name,
                    ReportSubmissionStatus = reportSubmission.ReportSubmissionStatus,
                    ReportTag = reportSubmission.ReportTag,
                    SubmissionWindowMonth = reportSubmission.SubmissionWindow.Month,
                    SubmissionWindowYear = reportSubmission.SubmissionWindow.Year,
                    Answers = reportSubmission.Answers.Select(x => new ReportResponseDto
                    {
                        Id = x.Id,
                        QuestionId = x.QuestionId,
                        TextAnswer = x.TextAnswer,
                        QuestionOptionId = x.QuestionOptionId,
                        Report = x.Report,
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

                existingReportSubmission.JammatEmailAddress = request.JammatEmailAddress;
                existingReportSubmission.ReportSubmissionStatus = request.ReportSubmissionStatus;
                existingReportSubmission.SubmissionWindow.Year = request.Year;
                existingReportSubmission.SubmissionWindow.Month = request.Month;    
                existingReportSubmission.ReportTag = request.ReportTag;
                existingReportSubmission.LastModifiedOn = DateTime.Now;
                existingReportSubmission.LastModifiedBy = request.LastModifiedBy;

                await _reportSubmissionRepository.UpdateReportSubmission(existingReportSubmission);

                
                var reportSubmissionDto = new ReportSubmissionDto
                {
                    JamaatId = existingReportSubmission.JamaatId,
                    CircuitId = existingReportSubmission.CircuitId,
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


        public async Task<BaseResponse<List<ReportSubmissionResponseDto>>> GetReportSubmissionsByReportTypeAsync(Guid reportTypeId)
        {
            try
            {
                var reportSubmissions = await _reportSubmissionRepository.GetReportSubmissionsByReportTypeAsync(reportTypeId);

                if (reportSubmissions == null )
                {
                    return new BaseResponse<List<ReportSubmissionResponseDto>>
                    {
                        Status = false,
                        Message = "No report submissions found for the given report type."
                    };
                }

                var reportSubmissionResponses = reportSubmissions.Select(reportSubmission => new ReportSubmissionResponseDto
                {
                    JamaatId = _currentUser.GetJamaatId(),
                    CircuitId = _currentUser.GetCircuit(),
                    JammatEmailAddress = reportSubmission.JammatEmailAddress,
                    ReportTypeName = reportSubmission.ReportType.Name,
                    ReportSubmissionStatus = reportSubmission.ReportSubmissionStatus,
                    ReportTag = reportSubmission.ReportTag,
                    SubmissionWindowMonth = reportSubmission.SubmissionWindow.Month,
                    SubmissionWindowYear = reportSubmission.SubmissionWindow.Year,
                    Answers = reportSubmission.Answers.Select(x => new ReportResponseDto
                    {
                        Id = x.Id,
                        QuestionId = x.QuestionId,
                        TextAnswer = x.TextAnswer,
                        QuestionOptionId = x.QuestionOptionId,
                        Report = x.Report,
                    }).ToList()
                }).ToList();

                return new BaseResponse<List<ReportSubmissionResponseDto>>
                {
                    Status = true,
                    Message = "Report submissions successfully retrieved",
                    Data = reportSubmissionResponses
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<ReportSubmissionResponseDto>>
                {
                    Status = false,
                    Message = $"An error occurred while retrieving the report submissions.{ex.Message}"
                };
            }
        }

        public async Task<BaseResponse<List<ReportSubmissionResponseDto>>> GetReportSubmissionsByCircuitIdAsync()
        {
            try
            {
                var circuitId = _currentUser.GetCircuit(); 

                if (circuitId <= 0)
                {
                    return new BaseResponse<List<ReportSubmissionResponseDto>>
                    {
                        Status = false,
                        Message = "Unable to retrieve the circuit ID."
                    };
                }
                var reportSubmissions = await _reportSubmissionRepository.GetReportSubmissionsByCircuitIdAsync(circuitId);

                if (reportSubmissions == null || !reportSubmissions.Any())
                {
                    return new BaseResponse<List<ReportSubmissionResponseDto>>
                    {
                        Status = false,
                        Message = "No report submissions found for the given circuit ID."
                    };
                }

                var reportSubmissionResponses = reportSubmissions.Select(reportSubmission => new ReportSubmissionResponseDto
                {
                    JamaatId = _currentUser.GetJamaatId(),
                    CircuitId = _currentUser.GetCircuit(),
                    JammatEmailAddress = reportSubmission.JammatEmailAddress,
                    ReportTypeName = reportSubmission.ReportType.Name,
                    ReportSubmissionStatus = reportSubmission.ReportSubmissionStatus,
                    ReportTag = reportSubmission.ReportTag,
                    SubmissionWindowMonth = reportSubmission.SubmissionWindow.Month,
                    SubmissionWindowYear = reportSubmission.SubmissionWindow.Year,
                    Answers = reportSubmission.Answers.Select(x => new ReportResponseDto
                    {
                        Id = x.Id,
                        QuestionId = x.QuestionId,
                        TextAnswer = x.TextAnswer,
                        QuestionOptionId = x.QuestionOptionId,
                        Report = x.Report,
                    }).ToList()
                }).ToList();

               
                return new BaseResponse<List<ReportSubmissionResponseDto>>
                {
                    Status = true,
                    Message = "Report submissions successfully retrieved",
                    Data = reportSubmissionResponses
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<ReportSubmissionResponseDto>>
                {
                    Status = false,
                    Message = $"An error occurred while retrieving the report submissions by circuit ID.{ex.Message}"
                };
            }
        }

        public async Task<BaseResponse<List<ReportSubmissionResponseDto>>> GetReportSubmissionsByJamaatIdAsync()
        {
            try
            {
                var jamaatId = _currentUser.GetJamaatId();

                if (jamaatId <= 0)
                {
                    return new BaseResponse<List<ReportSubmissionResponseDto>>
                    {
                        Status = false,
                        Message = "Unable to retrieve the Jamaat ID."
                    };
                }

                var reportSubmissions = await _reportSubmissionRepository.GetReportSubmissionsByJamaatIdAsync(jamaatId);

                if (reportSubmissions == null)
                {
                    return new BaseResponse<List<ReportSubmissionResponseDto>>
                    {
                        Status = false,
                        Message = "No report submissions found for the given Jamaat ID."
                    };
                }
                var reportSubmissionResponses = reportSubmissions.Select(reportSubmission => new ReportSubmissionResponseDto
                {
                    JamaatId = _currentUser.GetJamaatId(),
                    CircuitId = _currentUser.GetCircuit(),
                    JammatEmailAddress = reportSubmission.JammatEmailAddress,
                    ReportTypeName = reportSubmission.ReportType.Name,
                    ReportSubmissionStatus = reportSubmission.ReportSubmissionStatus,
                    ReportTag = reportSubmission.ReportTag,
                    SubmissionWindowMonth = reportSubmission.SubmissionWindow.Month,
                    SubmissionWindowYear = reportSubmission.SubmissionWindow.Year,
                    Answers = reportSubmission.Answers.Select(x => new ReportResponseDto
                    {
                        Id = x.Id,
                        QuestionId = x.QuestionId,
                        TextAnswer = x.TextAnswer,
                        QuestionOptionId = x.QuestionOptionId,
                        Report = x.Report,
                    }).ToList()
                }).ToList();

                return new BaseResponse<List<ReportSubmissionResponseDto>>
                {
                    Status = true,
                    Message = "Report submissions successfully retrieved",
                    Data = reportSubmissionResponses
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<ReportSubmissionResponseDto>>
                {
                    Status = false,
                    Message = $"An error occurred while retrieving the report submissions by Jamaat ID.{ex.Message}"
                };
            }
        }
    }
}
