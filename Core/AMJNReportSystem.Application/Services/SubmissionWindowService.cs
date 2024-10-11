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
        public async Task<Result<bool>> CreateReportSubmissionWindow<T>(CreateSubmissionWindowRequest request)
        {
            if (request is null)
                return await Result<bool>.FailAsync("Question can't be null.");

            var reportTypeEsist = _submissionWindowRepository.GetReportTypeExist(request.ReportTypeId);
            if (reportTypeEsist != null)
            {
                return await Result<bool>.FailAsync("Report Type already exist");
            }

            var submissionWindow = new SubmissionWindow
            {
                Id = Guid.NewGuid(),
                EndingDate = request.EndingDate,
                StartingDate = request.StartingDate,
                IsLocked = false,
                IsDeleted = false,
                Month = request.Month,
                Year = request.Year,
                ReportTypeId = request.ReportTypeId
            };
            var result = await _submissionWindowRepository.AddSubmissionWindow(submissionWindow);

            return await Result<bool>.SuccessAsync(true, "Submission Window created successfully");

        }

        public async Task<Result<bool>> UpdateReportSubmissionWindow<T>(Guid id, UpdateSubmissionWindowRequest request)
        {
            if (request is null)
                return await Result<bool>.FailAsync("Submission Window can't be null.");
            var submissionWindow = await _submissionWindowRepository.GetSubmissionWindowsById(id);
            if (submissionWindow == null)
            {
                return Result<bool>.Fail("Submission Window not found.");
            }
            submissionWindow.StartingDate = request.StartingDate;
            submissionWindow.EndingDate = request.EndingDate;
            submissionWindow.LastModifiedBy = "Admin";
            submissionWindow.IsLocked = false;
            submissionWindow.LastModifiedOn = DateTime.Now;
            submissionWindow.Year = request.Year;
            submissionWindow.Month = request.Month;
            submissionWindow.ReportTypeId = request.ReportTypeId;


            var result = await _submissionWindowRepository.UpdateSubmissionWindow(submissionWindow);
            if (!result)
                return Result<bool>.Fail("Failed to update submission Window.");

            return await Result<bool>.SuccessAsync(true, "Submission Window Updated successfully");

        }

        public async Task<Result<bool>> DeleteSubmissionWindow(Guid subWindowId)
        {
            var subWindow = await _submissionWindowRepository.GetSubmissionWindowsById(subWindowId);
            if (subWindow == null)
            {
                return Result<bool>.Fail("Submission Window not found.");
            }

            subWindow.IsDeleted = true;
            subWindow.DeletedBy = "Admin";
            subWindow.DeletedOn = DateTime.Now;

            var result = await _submissionWindowRepository.UpdateSubmissionWindow(subWindow);
            if (!result)
                return Result<bool>.Fail("Failed to delete question.");

            return await Result<bool>.SuccessAsync(true, "Submission Window Deleted Successfully");
        }

        public async Task<Result<SubmissionWindowDto>> GetActiveSubmissionWindows(Guid SubmissionWindowId)
        {
            var currentDate = DateTime.Now;

            var subWindow = await _submissionWindowRepository.GetActiveSubmissionWindows(SubmissionWindowId);


            if (subWindow.EndingDate <= currentDate)
            {
                subWindow.IsLocked = true;
                await _submissionWindowRepository.UpdateSubmissionWindow(subWindow);
            }

            var subWindowDtos = new SubmissionWindowDto
            {
                SubmissionWindowId = subWindow.Id,
                ReportTypeId = subWindow.ReportTypeId,
                ReportTypeName = subWindow.ReportType.Name,
                Month = subWindow.Month,
                Year = subWindow.Year,
                IsLocked = subWindow.IsLocked,
                StartDate = subWindow.StartingDate,
                EndDate = subWindow.EndingDate,
            };

            return Result<SubmissionWindowDto>.Success(subWindowDtos, "Submission Windows retrieved and updated successfully");
        }



        public async Task<Result<IList<SubmissionWindowDto>>> GetSubmissionWindows()
        {
            var subWindow = await _submissionWindowRepository.GetAllSubmissionWindowsAsync(q => !q.IsDeleted);

            var subWindowDtos = subWindow.Select(q => new SubmissionWindowDto
            {
                SubmissionWindowId = q.Id,
                ReportTypeId = q.ReportTypeId,
                ReportTypeName = q.ReportType.Name,
                Month = q.Month,
                Year = q.Year,
                IsLocked = q.IsLocked,
                StartDate = q.StartingDate,
                EndDate = q.EndingDate,
            }).ToList();

            return Result<IList<SubmissionWindowDto>>.Success(subWindowDtos, "Submission Window retrieved successfully");
        }

        public async Task<Result<SubmissionWindowDto>> GetSubmissionWindow(Guid id)
        {
            var subWindow = await _submissionWindowRepository.GetSubmissionWindowsById(id);
            if (subWindow == null)
            {
                return Result<SubmissionWindowDto>.Fail("Question not found.");
            }

            var subWindowDto = new SubmissionWindowDto
            {
                SubmissionWindowId = subWindow.Id,
                EndDate = subWindow.EndingDate,
                ReportTypeId = subWindow.ReportTypeId,
                ReportTypeName = subWindow.ReportType.Name,
                Month = subWindow.Month,
                Year = subWindow.Year,
                StartDate = subWindow.StartingDate,
                IsLocked = subWindow.IsLocked,
            };

            return Result<SubmissionWindowDto>.Success(subWindowDto, "Submission Window retrieved successfully");
        }
    }
}
