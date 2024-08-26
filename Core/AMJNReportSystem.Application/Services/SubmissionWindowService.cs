using AMJNReportSystem.Application.Abstractions.Repositories;
using AMJNReportSystem.Application.Abstractions.Services;
using AMJNReportSystem.Application.Models;
using AMJNReportSystem.Application.Models.DTOs;
using AMJNReportSystem.Application.Models.RequestModels;
using AMJNReportSystem.Application.Models.ResponseModels;
using AMJNReportSystem.Application.Wrapper;
using Domain.Entities;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMJNReportSystem.Application.Services
{
    public class SubmissionWindowService : ISubmissionWindowService
    {
        private readonly ISubmissionWindowRepository _submissionWindowRepository;
        public SubmissionWindowService(ISubmissionWindowRepository submissionWindowRepository)
        {
            _submissionWindowRepository = submissionWindowRepository;
        }

        public Task<Result<T>> CreateReportSubmissionWindow<T>(CreateSubmissionWindowRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<Result<PaginatedResult<T>>> GetActiveSubmissionWindows<T>(PaginationFilter filter)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<SubmissionWindowDto>> GetSubmissionWindow(Guid Id)
        {
            var submissionWindow = await _submissionWindowRepository.GetSubmissionWindowAsync(x => x.Id == Id);
            if (submissionWindow is null) return await Result<SubmissionWindowDto>.FailAsync("Submission Window not found");

            var submissionWindowDto = submissionWindow.Adapt<SubmissionWindowDto>();
            return await Result<SubmissionWindowDto>.SuccessAsync(submissionWindowDto, "Successfully retrieved Submission Window");
        }

        public async Task<IEnumerable<SubmissionWindowResponseModel>> GetSubmissionWindows(Guid? reportTypeId, int? month, int? year, string? status, bool? isLocked, DateTime? startDate, DateTime? endDate)
        {
            var listOfCurrentSubmissionWindow = new List<SubmissionWindowResponseModel>();

            var getAllSubmissionWindows = await _submissionWindowRepository.GetAllSubmissionWindowsAsync(reportTypeId, month, year, isLocked, endDate, startDate);
            foreach (var submissionWindow in getAllSubmissionWindows)
            {
                if (DateTime.Now >= submissionWindow.StartingDate && DateTime.Now <= submissionWindow.EndingDate)
                {
                    var currentSubmissionWindow = new SubmissionWindowResponseModel
                    {
                        Status = "IsCurrent",
                        Month = submissionWindow.Month,
                        Year = submissionWindow.Year,
                        EndDate = submissionWindow.EndingDate,
                        StartDate = submissionWindow.StartingDate,
                        Id = submissionWindow.Id,
                        ReportTypeName = submissionWindow.ReportType.Name,
                        ReportTypeId = submissionWindow.ReportType.Id
                    };
                    listOfCurrentSubmissionWindow.Add(currentSubmissionWindow);
                }
                else
                {
                    var previousSubmissionWindow = new SubmissionWindowResponseModel
                    {
                        Status = "IsPrevious",
                        Month = submissionWindow.Month,
                        Year = submissionWindow.Year,
                        EndDate = submissionWindow.EndingDate,
                        StartDate = submissionWindow.StartingDate,
                        Id = submissionWindow.Id,
                        ReportTypeName = submissionWindow.ReportType.Name,
                        ReportTypeId = submissionWindow.ReportType.Id
                    };
                    listOfCurrentSubmissionWindow.Add(previousSubmissionWindow);
                }
            }
            return listOfCurrentSubmissionWindow;
        }

        public Task<Result<T>> UpdateReportSubmissionWindow<T>(Guid id, UpdateSubmissionWindowRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
