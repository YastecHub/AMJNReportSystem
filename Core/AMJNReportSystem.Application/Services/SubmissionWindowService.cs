using AMJNReportSystem.Application.Abstractions.Repositories;
using AMJNReportSystem.Application.Abstractions.Services;
using AMJNReportSystem.Application.Models;
using AMJNReportSystem.Application.Models.DTOs;
using AMJNReportSystem.Application.Models.RequestModels;
using AMJNReportSystem.Application.Models.ResponseModels;
using AMJNReportSystem.Application.Wrapper;
using AMJNReportSystem.Domain.Entities;
using Mapster;

namespace AMJNReportSystem.Application.Services
{
    public class SubmissionWindowService : ISubmissionWindowService
    {
        private readonly ISubmissionWindowRepository _submissionWindowRepository; 
        public SubmissionWindowService(ISubmissionWindowRepository submissionWindowRepository) 
        {
            _submissionWindowRepository = submissionWindowRepository; 
        }

        public async Task<Result<bool>> UpdateReportSubmissionWindow<T>(Guid id, UpdateSubmissionWindowRequest request)
        {
			if (request is null)
				return await Result<bool>.FailAsync("Submission Window can't be null.");
			var submissionWindow = await _submissionWindowRepository.GetSubmissionWindowAsync(x => x.Id == id);
			if (submissionWindow == null)
			{
				return Result<bool>.Fail("Submission Window not found.");
			}
			submissionWindow.StartingDate = request.StartingDate;
			submissionWindow.EndingDate = request.EndingDate;
			submissionWindow.LastModifiedBy = "Admin";
			submissionWindow.LastModifiedOn = DateTime.Now;
			submissionWindow.Year = request.Year;
			submissionWindow.Month = request.Month;
			submissionWindow.ReportTypeId = request.ReportTypeId;


			var result = await _submissionWindowRepository.UpdateSubmissionWindow(submissionWindow);

			return result ? Result<bool>.Success(true) : Result<bool>.Fail("Failed to update submission window.");
		} 

		public async Task<Result<bool>> CreateReportSubmissionWindow<T>(CreateSubmissionWindowRequest request)
		{
			if (request is null)
				return await Result<bool>.FailAsync("Question can't be null.");
			var submissionWindow = new SubmissionWindow
			{
				Id = Guid.NewGuid(),
				EndingDate = request.EndingDate,
				StartingDate = request.StartingDate,
				IsDeleted = false,
				Month = request.Month,
				Year = request.Year,
				ReportTypeId = request.ReportSubmissionId 
			};
			var result = await _submissionWindowRepository.AddSubmissionWindow(submissionWindow);

			return result ? Result<bool>.Success(true) : Result<bool>.Fail("Failed to create submission window.");

		}

		public async Task<Result<bool>> DeleteSubmissionWindow(Guid subWindowId)
		{
			var subWindow = await _submissionWindowRepository.GetSubmissionWindowAsync(x => x.Id == subWindowId);
			if (subWindow == null)
			{
				return Result<bool>.Fail("Submission Window not found.");
			}

			subWindow.IsDeleted = true;

			var result = await _submissionWindowRepository.UpdateSubmissionWindow(subWindow);

			return result ? Result<bool>.Success(true) : Result<bool>.Fail("Failed to delete question.");
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
						ReportTypeName = submissionWindow.ReportType.Description,
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
						ReportTypeName = submissionWindow.ReportType.Description,
						ReportTypeId = submissionWindow.ReportType.Id
					};
					listOfCurrentSubmissionWindow.Add(previousSubmissionWindow);
				}
			}
			return listOfCurrentSubmissionWindow;
		}
	}
}
