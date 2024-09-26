﻿using AMJNReportSystem.Application.Abstractions.Repositories;
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
using Microsoft.Extensions.Logging;

namespace AMJNReportSystem.Application.Services
{
    public class ReportSubmissionService : IReportSubmissionService
    {
        private readonly IReportSubmissionRepository _reportSubmissionRepository;
        private readonly IReportTypeRepository _reportTypeRepository;
        private readonly ISubmissionWindowRepository _submissionWindowRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IQuestionRepository _questionRepository;
        private readonly ILogger<ReportSubmissionService> _logger;

        public ReportSubmissionService(IReportSubmissionRepository reportSubmission,
            IReportTypeRepository reportTypeRepository, ISubmissionWindowRepository
            submissionWindowRepository, ICurrentUser currentUser, IQuestionRepository questionRepository, ILogger<ReportSubmissionService> logger)
        {
            _reportSubmissionRepository = reportSubmission;
            _reportTypeRepository = reportTypeRepository;
            _submissionWindowRepository = submissionWindowRepository;
            _currentUser = currentUser;
            _questionRepository = questionRepository;
            _logger = logger;
        }

        public async Task<BaseResponse<bool>> CreateReportTypeSubmissionAsync(CreateReportSubmissionRequest request)
        {
            try
            {
                _logger.LogInformation("CreateReportTypeSubmissionAsync called with request {@Request}", request);

                if (request == null)
                {
                    _logger.LogWarning("Request is null.");
                    return new BaseResponse<bool>
                    {
                        Message = "Report submission not found.",
                        Status = false
                    };
                }

                var reportType = await _reportTypeRepository.GetReportTypeById(request.ReportTypeId);
                if (reportType == null)
                {
                    _logger.LogWarning("ReportType with ID {ReportTypeId} not found.", request.ReportTypeId);
                    return new BaseResponse<bool>
                    {
                        Message = "Report Type not found.",
                        Status = false
                    };
                }

                var reportSubmissionName = $"{reportType.Title}_{request.Year}_{request.Month}";
                _logger.LogInformation("Generated report submission name: {ReportSubmissionName}", reportSubmissionName);

                var reportSubmissionCheckerExist = await _reportSubmissionRepository.Exist(reportSubmissionName);
                if (reportSubmissionCheckerExist)
                {
                    _logger.LogWarning("Report submission with name {ReportSubmissionName} already exists.", reportSubmissionName);
                    return new BaseResponse<bool>
                    {
                        Message = "Submission already exists.",
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
                    _logger.LogInformation("Processing {Count} report responses.", request.ReportResponses.Count);
                    foreach (var response in request.ReportResponses)
                    {
                        var question = await _questionRepository.GetQuestionById(response.QuestionId);
                        if (question == null)
                        {
                            _logger.LogWarning("Question with ID {QuestionId} not found.", response.QuestionId);
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

                _logger.LogInformation("Saving report submission to the database.");
                await _reportSubmissionRepository.CreateReportSubmissionAsync(submission);

                _logger.LogInformation("Report submission successfully added.");
                return new BaseResponse<bool>
                {
                    Message = "Report submission successfully added.",
                    Status = true,
                    Data = true,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the report submission.");
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
                _logger.LogInformation("GetReportTypeSubmissionByIdAsync called with ID: {ReportTypeSubmissionId}", reportTypeSubmissionId);

                var reportSubmission = await _reportSubmissionRepository.GetReportTypeSubmissionByIdAsync(reportTypeSubmissionId);

                if (reportSubmission == null)
                {
                    _logger.LogWarning("Report submission with ID {ReportTypeSubmissionId} not found.", reportTypeSubmissionId);
                    return new BaseResponse<ReportSubmissionResponseDto>
                    {
                        Status = false,
                        Message = "Report submission Type not found"
                    };
                }

                _logger.LogInformation("Report submission with ID {ReportTypeSubmissionId} found. Mapping data to DTO.", reportTypeSubmissionId);

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

                _logger.LogInformation("Successfully retrieved and mapped report submission with ID {ReportTypeSubmissionId}.", reportTypeSubmissionId);

                return new BaseResponse<ReportSubmissionResponseDto>
                {
                    Status = true,
                    Message = "Report submission successfully retrieved",
                    Data = reportSubmissionResponse
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving report submission with ID {ReportTypeSubmissionId}.", reportTypeSubmissionId);
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
                _logger.LogInformation("GetAllReportTypeSubmissionsAsync called with pagination filter: {PageNumber}, {PageSize}", filter?.PageNumber, filter?.PageSize);

                if (filter == null)
                {
                    _logger.LogWarning("Pagination filter is null.");
                    return new BaseResponse<PaginatedResult<ReportSubmissionResponseDto>>
                    {
                        Status = false,
                        Message = "Pagination filter is required."
                    };
                }

                var paginatedResult = await _reportSubmissionRepository.GetAllReportTypeSubmissionsAsync(filter);

                _logger.LogInformation("Successfully retrieved {TotalCount} report type submissions.", paginatedResult.TotalCount);

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

                _logger.LogInformation("Mapped report type submissions to DTO successfully.");

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
                _logger.LogError(ex, "An error occurred while retrieving report type submissions.");
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
                _logger.LogInformation("UpdateReportSubmission called with submission ID: {Id}", id);

                var existingReportSubmission = await _reportSubmissionRepository.GetReportTypeSubmissionByIdAsync(id);

                if (existingReportSubmission == null)
                {
                    _logger.LogWarning("Report submission with ID {Id} not found.", id);
                    return new BaseResponse<ReportSubmissionDto>
                    {
                        Status = false,
                        Message = "Report submission not found."
                    };
                }

                _logger.LogInformation("Updating report submission with ID: {Id}", id);
                existingReportSubmission.JammatEmailAddress = request.JammatEmailAddress;
                existingReportSubmission.ReportSubmissionStatus = request.ReportSubmissionStatus;
                existingReportSubmission.SubmissionWindow.Year = request.Year;
                existingReportSubmission.SubmissionWindow.Month = request.Month;
                existingReportSubmission.ReportTag = request.ReportTag;
                existingReportSubmission.LastModifiedOn = DateTime.Now;
                existingReportSubmission.LastModifiedBy = request.LastModifiedBy;

                await _reportSubmissionRepository.UpdateReportSubmission(existingReportSubmission);

                _logger.LogInformation("Successfully updated report submission with ID: {Id}", id);
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

                _logger.LogInformation("Report submission DTO mapped successfully for submission ID: {Id}", id);

                return new BaseResponse<ReportSubmissionDto>
                {
                    Status = true,
                    Data = reportSubmissionDto,
                    Message = "Report submission updated successfully."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the report submission with ID: {Id}", id);
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
                _logger.LogInformation("DeleteReportSubmission called for submission ID: {ReportSubmissionId}", reportSubmissionId);

                var reportSubmission = await _reportSubmissionRepository.GetReportTypeSubmissionByIdAsync(reportSubmissionId);
                if (reportSubmission == null)
                {
                    _logger.LogWarning("Report submission with ID {ReportSubmissionId} not found.", reportSubmissionId);
                    return Result<bool>.Fail("Report section not found.");
                }

                _logger.LogInformation("Marking report submission with ID {ReportSubmissionId} as deleted.", reportSubmissionId);
                reportSubmission.IsDeleted = true;
                reportSubmission.DeletedOn = DateTime.Now;
                reportSubmission.DeletedBy = _currentUser.Name;

                var result = await _reportSubmissionRepository.UpdateReportSubmission(reportSubmission);

                _logger.LogInformation("Report submission with ID {ReportSubmissionId} deleted successfully.", reportSubmissionId);
                return new Result<bool>
                {
                    Succeeded = true,
                    Messages = new List<string> { "Report submission deleted successfully." }
                };


                _logger.LogWarning("Failed to delete report submission with ID {ReportSubmissionId}.", reportSubmissionId);
                return Result<bool>.Fail("Failed to delete report submission.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting report submission with ID: {ReportSubmissionId}", reportSubmissionId);
                return Result<bool>.Fail($"An error occurred: {ex.Message}");
            }
        }


        public async Task<BaseResponse<List<ReportSubmissionResponseDto>>> GetReportSubmissionsByReportTypeAsync(Guid reportTypeId)
        {
            try
            {
                _logger.LogInformation("GetReportSubmissionsByReportTypeAsync called for ReportTypeId: {ReportTypeId}", reportTypeId);

                var reportSubmissions = await _reportSubmissionRepository.GetReportSubmissionsByReportTypeAsync(reportTypeId);

                if (reportSubmissions == null || !reportSubmissions.Any())
                {
                    _logger.LogWarning("No report submissions found for ReportTypeId: {ReportTypeId}", reportTypeId);
                    return new BaseResponse<List<ReportSubmissionResponseDto>>
                    {
                        Status = false,
                        Message = "No report submissions found for the given report type."
                    };
                }

                _logger.LogInformation("{Count} report submissions found for ReportTypeId: {ReportTypeId}", reportSubmissions.Count, reportTypeId);

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

                _logger.LogInformation("Successfully retrieved report submissions for ReportTypeId: {ReportTypeId}", reportTypeId);

                return new BaseResponse<List<ReportSubmissionResponseDto>>
                {
                    Status = true,
                    Message = "Report submissions successfully retrieved",
                    Data = reportSubmissionResponses
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving report submissions for ReportTypeId: {ReportTypeId}", reportTypeId);
                return new BaseResponse<List<ReportSubmissionResponseDto>>
                {
                    Status = false,
                    Message = $"An error occurred while retrieving the report submissions: {ex.Message}"
                };
            }
        }


        public async Task<BaseResponse<List<ReportSubmissionResponseDto>>> GetReportSubmissionsByCircuitIdAsync()
        {
            try
            {
                _logger.LogInformation("GetReportSubmissionsByCircuitIdAsync called for user: {UserId}", _currentUser.GetUserId());

                var circuitId = _currentUser.GetCircuit();
                _logger.LogInformation("Retrieved Circuit ID: {CircuitId} for user: {UserId}", circuitId, _currentUser.GetUserId());

                if (circuitId <= 0)
                {
                    _logger.LogWarning("Unable to retrieve a valid circuit ID for user: {UserId}", _currentUser.GetUserId());
                    return new BaseResponse<List<ReportSubmissionResponseDto>>
                    {
                        Status = false,
                        Message = "Unable to retrieve the circuit ID."
                    };
                }

                var reportSubmissions = await _reportSubmissionRepository.GetReportSubmissionsByCircuitIdAsync(circuitId);
                _logger.LogInformation("{Count} report submissions retrieved for Circuit ID: {CircuitId}", reportSubmissions?.Count() ?? 0, circuitId);

                if (reportSubmissions == null || !reportSubmissions.Any())
                {
                    _logger.LogWarning("No report submissions found for Circuit ID: {CircuitId}", circuitId);
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

                _logger.LogInformation("Successfully retrieved report submissions for Circuit ID: {CircuitId}", circuitId);

                return new BaseResponse<List<ReportSubmissionResponseDto>>
                {
                    Status = true,
                    Message = "Report submissions successfully retrieved",
                    Data = reportSubmissionResponses
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving report submissions for Circuit ID: {CircuitId}", _currentUser.GetCircuit());
                return new BaseResponse<List<ReportSubmissionResponseDto>>
                {
                    Status = false,
                    Message = $"An error occurred while retrieving the report submissions by circuit ID: {ex.Message}"
                };
            }
        }


        public async Task<BaseResponse<List<ReportSubmissionResponseDto>>> GetReportSubmissionsByJamaatIdAsync()
        {
            try
            {
                _logger.LogInformation("GetReportSubmissionsByJamaatIdAsync called for user: {UserId}", _currentUser.GetUserId());

                var jamaatId = _currentUser.GetJamaatId();
                _logger.LogInformation("Retrieved Jamaat ID: {JamaatId} for user: {UserId}", jamaatId, _currentUser.GetUserId());

                if (jamaatId <= 0)
                {
                    _logger.LogWarning("Unable to retrieve a valid Jamaat ID for user: {UserId}", _currentUser.GetUserId());
                    return new BaseResponse<List<ReportSubmissionResponseDto>>
                    {
                        Status = false,
                        Message = "Unable to retrieve the Jamaat ID."
                    };
                }

                var reportSubmissions = await _reportSubmissionRepository.GetReportSubmissionsByJamaatIdAsync(jamaatId);
                _logger.LogInformation("{Count} report submissions retrieved for Jamaat ID: {JamaatId}", reportSubmissions?.Count() ?? 0, jamaatId);

                if (reportSubmissions == null || !reportSubmissions.Any())
                {
                    _logger.LogWarning("No report submissions found for Jamaat ID: {JamaatId}", jamaatId);
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

                _logger.LogInformation("Successfully retrieved report submissions for Jamaat ID: {JamaatId}", jamaatId);

                return new BaseResponse<List<ReportSubmissionResponseDto>>
                {
                    Status = true,
                    Message = "Report submissions successfully retrieved",
                    Data = reportSubmissionResponses
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving report submissions for Jamaat ID: {JamaatId}");
                return new BaseResponse<List<ReportSubmissionResponseDto>>
                {
                    Status = false,
                    Message = $"An error occurred while retrieving the report submissions by Jamaat ID: {ex.Message}"
                };
            }
        }

    }
}
