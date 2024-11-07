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
using AMJNReportSystem.Domain.Enums;
using AMJNReportSystem.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Globalization;

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
        private readonly ISubmissionWindowService _submissionWindowService;
        private readonly IGatewayHandler _gatewayHandler;
        private readonly IReportResponseRepository _reportResponseRepository;

        public ReportSubmissionService(IReportSubmissionRepository reportSubmission,
            IReportTypeRepository reportTypeRepository, ISubmissionWindowRepository
            submissionWindowRepository, ICurrentUser currentUser, IQuestionRepository questionRepository,
            ILogger<ReportSubmissionService> logger, ISubmissionWindowService submissionWindowService,
            IGatewayHandler gatewayHandler, IReportResponseRepository reportResponseRepository)
        {
            _reportSubmissionRepository = reportSubmission;
            _reportTypeRepository = reportTypeRepository;
            _submissionWindowRepository = submissionWindowRepository;
            _currentUser = currentUser;
            _questionRepository = questionRepository;
            _logger = logger;
            _submissionWindowService = submissionWindowService;
            _gatewayHandler = gatewayHandler;
            _reportResponseRepository = reportResponseRepository;
        }

        public async Task<BaseResponse<bool>> CreateAndUpdateReportSubmissionAsync(CreateReportSubmissionRequest request)
        {
            try
            {
                _logger.LogInformation("Called {MethodName} with request: {Request}",
                    nameof(CreateAndUpdateReportSubmissionAsync), JsonConvert.SerializeObject(request));


                if (request == null)
                {
                    _logger.LogWarning("Request is null.");
                    return new BaseResponse<bool>
                    {
                        Message = "Report submission not found.",
                        Status = false
                    };
                }


                var submissionWindow = await _submissionWindowRepository.GetSubmissionWindowsById(request.SubmissionWindowId);
                if (submissionWindow == null)
                {
                    _logger.LogWarning($"Submission window with ID {request.SubmissionWindowId} not found.");
                    return new BaseResponse<bool>
                    {
                        Message = "Submission window not found.",
                        Status = false
                    };
                }


                var reportType = await _reportTypeRepository.GetReportTypeById(submissionWindow.ReportTypeId);
                if (reportType == null)
                {
                    _logger.LogWarning($"ReportType with ID {submissionWindow.ReportTypeId} not found.", submissionWindow.ReportTypeId);
                    return new BaseResponse<bool>
                    {
                        Message = "Report Type not found.",
                        Status = false
                    };
                }



                var getsubwinactiveness = await _submissionWindowService
                    .GetActiveSubmissionWindows(request.SubmissionWindowId);
                if (getsubwinactiveness.Data.IsLocked)
                {
                    _logger.LogWarning($"Submission window with ID {request.SubmissionWindowId} is locked.");
                    return new BaseResponse<bool>
                    {
                        Message = "Submission window is locked. No further submissions are allowed.",
                        Status = false
                    };
                }

                var jammatId = _currentUser.GetJamaatId();

                var reportSubmissionCheckerExist = await _submissionWindowRepository
                    .CheckIfReportHasBeenSubmittedByJammatPresident(submissionWindow.Id, jammatId);

                if (reportSubmissionCheckerExist != null)
                {
                    var checkReportSection = await _submissionWindowRepository
                        .CheckIfReportSectionHasBeenSubmitted(submissionWindow.Id, jammatId, request.reportSectionId);

                    if (checkReportSection == null)
                    {

                        foreach (var item in request.ReportResponses)
                        {

                            var newAnswer = new ReportResponse
                            {
                                QuestionId = item.QuestionId,
                                TextAnswer = item.TextAnswer,
                                Id = Guid.NewGuid(),
                                QuestionOptionId = item.QuestionOptionId,
                                Report = item.Report,
                                CreatedOn = DateTime.Now,
                                ReportSubmissionId = reportSubmissionCheckerExist.Id,
                                ReportSubmissionSectionId = request.reportSectionId
                            };

                            await _reportResponseRepository.AddReportResponseAsync(newAnswer);
                        }

                        return new BaseResponse<bool>
                        {
                            Status = true,
                            Data = false,
                            Message = $"Report Section Submitted successfully "

                        };
                    }
                    else
                    {

                        return new BaseResponse<bool>
                        {
                            Status = true,
                            Data = false,
                            Message = $"Report Section has already been submitted by {reportSubmissionCheckerExist.CreatedBy} " +
                            $"on {reportSubmissionCheckerExist.CreatedOn.ToString("dd-MM-yyyy")}"
                        };
                    }
                }
                else
                {

                    var submission = new ReportSubmission
                    {
                        JamaatId = _currentUser.GetJamaatId(),
                        CircuitId = _currentUser.GetCircuit(),
                        JammatEmailAddress = _currentUser.GetUserEmail(),
                        ReportSubmissionStatus = ReportSubmissionStatus.Pending,
                        ReportTag = ReportTag.MuqamReportType,
                        SubmissionWindowId = request.SubmissionWindowId,
                        CreatedBy = _currentUser.Name,
                        CreatedOn = DateTime.Now,
                    };

                    await _reportSubmissionRepository.CreateReportSubmissionAsync(submission);

                    foreach (var item in request.ReportResponses)
                    {

                        var newAnswer = new ReportResponse
                        {
                            QuestionId = item.QuestionId,
                            TextAnswer = item.TextAnswer,
                            Id = Guid.NewGuid(),
                            QuestionOptionId = item.QuestionOptionId,
                            Report = item.Report,
                            CreatedOn = DateTime.Now,
                            ReportSubmissionId = submission.Id,
                            ReportSubmissionSectionId = request.reportSectionId
                        };

                        await _reportResponseRepository.AddReportResponseAsync(newAnswer);
                    }

                    _logger.LogInformation($"Saving report submission to the database.");
                    await _reportSubmissionRepository.CreateReportSubmissionAsync(submission);

                    _logger.LogInformation("Report submission successfully added.");
                    return new BaseResponse<bool>
                    {
                        Message = "Report submission successfully added.",
                        Status = true,
                        Data = true,
                    };
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while creating the report submission.");
                return new BaseResponse<bool>
                {
                    Message = $"An error occurred: {ex.Message}",
                    Status = false
                };
            }
        }

        public async Task<BaseResponse<SubmittedReportDto>> GetSectionReportSubmissionAsync(Guid reportTypeSubmissionId, Guid reportSectionId)
        {
            try
            {
                _logger.LogInformation($"GetReportTypeSubmissionByIdAsync called with ID: {reportTypeSubmissionId}", reportTypeSubmissionId);

                var jamaatId = _currentUser.GetJamaatId();

                var reportSubmission = await _reportSubmissionRepository
                    .GetReportSubmissionSectionAsync(reportTypeSubmissionId, reportSectionId, jamaatId);

                if (reportSubmission == null)
                {
                    _logger.LogWarning($"Report submission with ID {reportTypeSubmissionId} not found.");
                    return new BaseResponse<SubmittedReportDto>
                    {
                        Status = false,
                        Message = "Report submission Type not found"
                    };
                }

                _logger.LogInformation($"Report submission with ID {reportTypeSubmissionId} found.");

                var reportSubmissionResponse = new SubmittedReportDto
                {
                    Id = reportSubmission.Id,
                    JamaatId = _currentUser.GetJamaatId(),
                    CircuitId = _currentUser.GetCircuit(),
                    JammatEmailAddress = reportSubmission.JammatEmailAddress,
                    ReportTypeName = reportSubmission.SubmissionWindow.ReportType.Name,
                    Answers = reportSubmission.Answers.Select(x => new SubmittedReportResponseDto
                    {
                        Id = x.Id,
                        QuestionName = x.Question.QuestionName,
                        QuestionType = x.Question.QuestionType,
                        ReportSectionId = x.Question.ReportSectionId,
                        IsActive = x.Question.IsActive,
                        IsRequired = x.Question.IsRequired,
                        QuestionId = x.QuestionId,
                        TextAnswer = x.TextAnswer,
                        ResponseType = x.Question.ResponseType,
                        QuestionOptionId = x.QuestionOptionId,
                        Report = x.Report,
                        SubmittedQuestionOptions = x.Question.Options.Select(x => new SubmittedQuestionOption
                        {
                            Id = x.Id,
                            QuestionId = x.QuestionId,
                            Text = x.Text,
                        }).ToList(),

                    }).ToList()
                };

                var result = reportSubmissionResponse.Answers.Where(x => x.ReportSectionId == reportSectionId).ToList();
                reportSubmissionResponse.Answers = result;


                _logger.LogInformation($"Successfully retrieved and mapped report submission with ID {reportTypeSubmissionId}.");

                return new BaseResponse<SubmittedReportDto>
                {
                    Status = true,
                    Message = "Report submission successfully retrieved",
                    Data = reportSubmissionResponse
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving report submission with ID {reportTypeSubmissionId}.");
                return new BaseResponse<SubmittedReportDto>
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
                _logger.LogInformation($"GetAllReportTypeSubmissionsAsync called with pagination filter: {filter?.PageNumber}, {filter?.PageSize}", filter?.PageNumber, filter?.PageSize);

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

                _logger.LogInformation($"Successfully retrieved {paginatedResult.TotalCount} report type submissions.");

                var dtos = paginatedResult.Data.Select(submission => new ReportSubmissionResponseDto
                {
                    JamaatId = _currentUser.GetJamaatId(),
                    CircuitId = _currentUser.GetCircuit(),
                    JammatEmailAddress = submission.JammatEmailAddress,
                    ReportTypeName = submission.SubmissionWindow.ReportType.Name,
                    ReportSubmissionStatus = submission.ReportSubmissionStatus,
                    ReportTag = (Domain.Enums.ReportTag)submission.ReportTag,
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

        public async Task<BaseResponse<List<ReportSubmissionResponseDto>>> GetAllReportTypeSubmissionsAsync()
        {
            try
            {
                _logger.LogInformation("GetAllReportTypeSubmissionsAsync called without pagination.");

                var submissions = await _reportSubmissionRepository.GetAllReportTypeSubmissionsAsync();

                _logger.LogInformation($"Successfully retrieved {submissions.Count} report type submissions.");

                var dtos = submissions.Select(submission => new ReportSubmissionResponseDto
                {
                    Id = submission.Id,
                    JamaatId = _currentUser.GetJamaatId(),
                    CircuitId = _currentUser.GetCircuit(),
                    JammatEmailAddress = submission.JammatEmailAddress,
                    ReportTypeName = submission.SubmissionWindow.ReportType.Name,
                    ReportSubmissionStatus = submission.ReportSubmissionStatus,
                    ReportTag = (Domain.Enums.ReportTag)submission.ReportTag,
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
                }).OrderByDescending(x => x.CreatedOn).ToList();

                _logger.LogInformation("Mapped report type submissions to DTO successfully.");

                return new BaseResponse<List<ReportSubmissionResponseDto>>
                {
                    Status = true,
                    Message = $"{submissions.Count} report type submissions retrieved successfully.",
                    Data = dtos
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving report type submissions.");
                return new BaseResponse<List<ReportSubmissionResponseDto>>
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
                    _logger.LogWarning($"Report submission with ID {id} not found.");
                    return new BaseResponse<ReportSubmissionDto>
                    {
                        Status = false,
                        Message = "Report submission not found."
                    };
                }

                _logger.LogInformation($"Updating report submission with ID: {id}");
                existingReportSubmission.JammatEmailAddress = request.JammatEmailAddress;
                existingReportSubmission.SubmissionWindow.Year = request.Year;
                existingReportSubmission.SubmissionWindow.Month = request.Month;
                existingReportSubmission.ReportTag = request.ReportTag;
                existingReportSubmission.LastModifiedOn = DateTime.Now;
                existingReportSubmission.LastModifiedBy = request.LastModifiedBy;

                await _reportSubmissionRepository.UpdateReportSubmission(existingReportSubmission);

                _logger.LogInformation($"Successfully updated report submission with ID: {id}");
                var reportSubmissionDto = new ReportSubmissionDto
                {
                    JamaatId = _currentUser.GetJamaatId(),
                    CircuitId = _currentUser.GetCircuit(),
                    ReportTypeId = existingReportSubmission.SubmissionWindow.ReportTypeId,
                    JammatEmailAddress = existingReportSubmission.JammatEmailAddress,
                    ReportType = existingReportSubmission.SubmissionWindow.ReportType,
                    ReportSubmissionStatus = existingReportSubmission.ReportSubmissionStatus,
                    ReportTag = (Domain.Enums.ReportTag)existingReportSubmission.ReportTag,
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

                _logger.LogInformation($"Report submission DTO mapped successfully for submission ID: {id}");

                return new BaseResponse<ReportSubmissionDto>
                {
                    Status = true,
                    Data = reportSubmissionDto,
                    Message = "Report submission updated successfully."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the report submission with ID: {id}");
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
                _logger.LogInformation($"DeleteReportSubmission called for submission ID: {reportSubmissionId}", reportSubmissionId);

                var reportSubmission = await _reportSubmissionRepository.GetReportTypeSubmissionByIdAsync(reportSubmissionId);
                if (reportSubmission == null)
                {
                    _logger.LogWarning($"Report submission with ID {reportSubmissionId} not found.");
                    return Result<bool>.Fail("Report section not found.");
                }

                _logger.LogInformation($"Marking report submission with ID {reportSubmissionId} as deleted.", reportSubmissionId);
                reportSubmission.IsDeleted = true;
                reportSubmission.DeletedOn = DateTime.Now;
                reportSubmission.DeletedBy = _currentUser.Name;

                var result = await _reportSubmissionRepository.UpdateReportSubmission(reportSubmission);

                _logger.LogInformation($"Report submission with ID {reportSubmissionId} deleted successfully.", reportSubmissionId);
                return new Result<bool>
                {
                    Succeeded = true,
                    Messages = new List<string> { "Report submission deleted successfully." }
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting report submission with ID: {reportSubmissionId}");
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
                    _logger.LogWarning($"No report submissions found for ReportTypeId: {reportTypeId}");
                    return new BaseResponse<List<ReportSubmissionResponseDto>>
                    {
                        Status = false,
                        Message = "No report submissions found for the given report type."
                    };
                }

                _logger.LogInformation($"{reportSubmissions.Count} report submissions found for ReportTypeId: {reportTypeId}");

                var reportSubmissionResponses = reportSubmissions.Select(reportSubmission => new ReportSubmissionResponseDto
                {
                    JamaatId = _currentUser.GetJamaatId(),
                    CircuitId = _currentUser.GetCircuit(),
                    JammatEmailAddress = reportSubmission.JammatEmailAddress,
                    ReportTypeName = reportSubmission.SubmissionWindow.ReportType.Name,
                    ReportSubmissionStatus = reportSubmission.ReportSubmissionStatus,
                    ReportTag = (Domain.Enums.ReportTag)reportSubmission.ReportTag,
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
                _logger.LogError(ex, $"An error occurred while retrieving report submissions for ReportTypeId: {reportTypeId}");
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
                _logger.LogInformation($"GetReportSubmissionsByCircuitIdAsync called for user: {_currentUser.GetUserId()}");

                var circuitId = _currentUser.GetCircuit();
                _logger.LogInformation($"Retrieved Circuit ID: {circuitId} for user: {_currentUser.GetUserId()}");

                if (circuitId <= 0)
                {
                    _logger.LogWarning($"Unable to retrieve a valid circuit ID for user: {_currentUser.GetUserId()}", _currentUser.GetUserId());
                    return new BaseResponse<List<ReportSubmissionResponseDto>>
                    {
                        Status = false,
                        Message = "Unable to retrieve the circuit ID."
                    };
                }

                var reportSubmissions = await _reportSubmissionRepository.GetReportSubmissionsByCircuitIdAsync(circuitId);
                _logger.LogInformation($"report submissions retrieved for Circuit ID: {circuitId}");

                if (reportSubmissions == null || !reportSubmissions.Any())
                {
                    _logger.LogWarning($"No report submissions found for Circuit ID: {circuitId}");
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
                    ReportTypeName = reportSubmission.SubmissionWindow.ReportType.Name,
                    ReportSubmissionStatus = reportSubmission.ReportSubmissionStatus,
                    ReportTag = (Domain.Enums.ReportTag)reportSubmission.ReportTag,
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

                _logger.LogInformation($"Successfully retrieved report submissions for Circuit ID: {circuitId}");

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
                var getJamaat = await _gatewayHandler.GetListOfMuqamAsync();

                if (getJamaat == null)
                {
                    return new BaseResponse<List<ReportSubmissionResponseDto>>
                    {
                        Status = true,
                        Message = "Unable to retive Muqam",
                        Data = new List<ReportSubmissionResponseDto>()
                    };
                }


                _logger.LogInformation($"GetReportSubmissionsByJamaatIdAsync called for user: {_currentUser.Name}");

                var jamaatId = _currentUser.GetJamaatId();
                _logger.LogInformation($"Retrieved Jamaat ID: {jamaatId} for user: {_currentUser.Name}");

                if (jamaatId <= 0)
                {
                    _logger.LogWarning($"Unable to retrieve a valid Jamaat ID for user: {_currentUser.Name}");
                    return new BaseResponse<List<ReportSubmissionResponseDto>>
                    {
                        Status = false,
                        Message = "Unable to retrieve the Jamaat ID."
                    };
                }

                var reportSubmissions = await _reportSubmissionRepository.GetReportSubmissionsByJamaatIdAsync(jamaatId);
                _logger.LogInformation($"{reportSubmissions.Count()} report submissions retrieved for Jamaat ID: {jamaatId}");

                if (reportSubmissions == null || !reportSubmissions.Any())
                {
                    _logger.LogWarning($"No report submissions found for Jamaat ID: {jamaatId}");
                    return new BaseResponse<List<ReportSubmissionResponseDto>>
                    {
                        Status = false,
                        Message = "No report submissions found for the given Jamaat ID."
                    };
                }
                var currentDate = DateTime.Today;

                var reportSubmissionResponses = reportSubmissions.Select(reportSubmission => new ReportSubmissionResponseDto
                {
                    JamaatId = reportSubmission.JamaatId,
                    CircuitId = reportSubmission.CircuitId,
                    JammatEmailAddress = reportSubmission.JammatEmailAddress,
                    ReportTypeName = reportSubmission.SubmissionWindow.ReportType.Name,
                    ReportSubmissionStatus = reportSubmission.ReportSubmissionStatus,
                    ReportTag = reportSubmission.ReportTag,
                    SubmissionWindowMonth = reportSubmission.SubmissionWindow.Month,
                    SubmissionWindowYear = reportSubmission.SubmissionWindow.Year,
                    JamaatName = GetMuqamiDetailByJamaatId(getJamaat, reportSubmission.JamaatId).JamaatName,
                    CircuitName = GetMuqamiDetailByJamaatId(getJamaat, reportSubmission.JamaatId).CircuitName,
                    Month = GetMonthName(reportSubmission.SubmissionWindow.Month),
                    Year = reportSubmission.SubmissionWindow.Year,
                    StartDate = reportSubmission.SubmissionWindow.StartingDate.ToString("dd-MMM-yyyy"),
                    EndDate = reportSubmission.SubmissionWindow.EndingDate.ToString("dd-MMM-yyyy"),
                    Status = (currentDate >= reportSubmission.SubmissionWindow.StartingDate && currentDate <= reportSubmission.SubmissionWindow.EndingDate) ? true : false,
                    CreatedOn = reportSubmission.CreatedOn,
                    NumberOfQuestion = reportSubmission.Answers.Count(),
                    NumberOfQuestionSections = reportSubmission.SubmissionWindow.ReportType.ReportSections.Count()
                }).ToList();

                _logger.LogInformation($"Successfully retrieved report submissions for Jamaat ID: {jamaatId}");

                return new BaseResponse<List<ReportSubmissionResponseDto>>
                {
                    Status = true,
                    Message = "Report submissions successfully retrieved",
                    Data = reportSubmissionResponses
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving report submissions for Jamaat");
                return new BaseResponse<List<ReportSubmissionResponseDto>>
                {
                    Status = false,
                    Message = $"An error occurred while retrieving the report submissions by Jamaat ID: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponse<List<JamaatReport>>> GetJamaatReportsBySubmissionWindowIdAsync(Guid submissionWindowId)
        {
            try
            {
                _logger.LogInformation($"GetJamaatReportsBySubmissionWindowIdAsync called with Submission Window ID: {submissionWindowId}");

                var getJamaat = await _gatewayHandler.GetListOfMuqamAsync();

                var jamaatReports = await _reportSubmissionRepository.GetJamaatReportsBySubmissionWindowIdAsync(submissionWindowId);

                if (jamaatReports == null || !jamaatReports.Any())
                {
                    _logger.LogWarning($"No Jamaat reports found for Submission Window ID: {submissionWindowId}");
                    return new BaseResponse<List<JamaatReport>>
                    {
                        Status = false,
                        Message = "No Jamaat reports found for the given Submission Window ID."
                    };
                }

                var jamaatReportResponses = jamaatReports.Select(report => new JamaatReport
                {
                    Id = report.Id,
                    JamaatId = report.JamaatId,
                    SubmissionWindowId = report.SubmissionWindowId,
                    SubmissionWindowName = report.SubmissionWindow.ReportType.Name,
                    JamaatName = GetMuqamiDetailByJamaatId(getJamaat, report.JamaatId).JamaatName,
                }).ToList();

                _logger.LogInformation($"Successfully retrieved Jamaat reports for Submission Window ID: {submissionWindowId}");

                return new BaseResponse<List<JamaatReport>>
                {
                    Status = true,
                    Message = "Jamaat reports successfully retrieved",
                    Data = jamaatReportResponses
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving Jamaat reports for Submission Window ID: {submissionWindowId}");
                return new BaseResponse<List<JamaatReport>>
                {
                    Status = false,
                    Message = $"An error occurred while retrieving Jamaat reports: {ex.Message}"
                };
            }
        }

        private static (string? JamaatName, string? CircuitName) GetMuqamiDetailByJamaatId(List<Muqam> getJamaat, int jamaatId)
        {

            if (getJamaat == null || !getJamaat.Any())
            {
                return (null, null);
            }

            var detail = getJamaat.FirstOrDefault(x => x.JamaatId == jamaatId);

            return detail != null
                ? (detail.JamaatName, detail.CircuitName)
                : (null, null);
        }

        public static string GetMonthName(int monthNumber)
        {
            // Validate the month number
            if (monthNumber < 1 || monthNumber > 12)
            {
                throw new ArgumentOutOfRangeException(nameof(monthNumber), "Month must be between 1 and 12.");
            }

            // Use DateTime to get the month name
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monthNumber);
        }


    }
}
