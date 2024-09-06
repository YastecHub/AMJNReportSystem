using AMJNReportSystem.Application.Models.RequestModels;
using AMJNReportSystem.Application.Wrapper;
using AMJNReportSystem.Domain.Entities;
using AMJNReportSystem.Domain.Repositories;

namespace AMJNReportSystem.Application.Services
{
    public class ReportResponseService : IReportResponseService
    {
        private readonly IReportResponseRepository _repository;

        public ReportResponseService(IReportResponseRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ReportResponseDto>> GetAllReportResponsesAsync()
        {
            var responses = await _repository.GetAllReportResponseAsync();
            return responses.Select(r => new ReportResponseDto
            {
                Id = r.Id,
                QuestionId = r.QuestionId,
                TextAnswer = r.TextAnswer,
                QuestionOptionId = r.QuestionOptionId,
                Report = r.Report
            });
        }

        public async Task<ReportResponseDto?> GetReportResponseByIdAsync(Guid responseId)
        {
            var response = await _repository.GetReportResponseByIdAsync(responseId);
            if (response == null)
            {
                return null;
            }

            return new ReportResponseDto
            {
                Id = response.Id,
                QuestionId = response.QuestionId,
                TextAnswer = response.TextAnswer,
                QuestionOptionId = response.QuestionOptionId,
                Report = response.Report
            };
        }

        public async Task<Result<ReportResponseDto>> CreateReportResponseAsync(CreateReportResponseRequest responseDto)
        {
            if (!await _repository.QuestionExistsAsync(responseDto.QuestionId))
            {
                return Result<ReportResponseDto>.Fail("The provided QuestionId does not exist.");
            }

            if (responseDto.QuestionOptionId.HasValue && !await _repository.QuestionOptionExistsAsync(responseDto.QuestionOptionId.Value))
            {
                return Result<ReportResponseDto>.Fail("The provided QuestionOptionId does not exist.");
            }

            var response = new ReportResponse
            {
                Id = Guid.NewGuid(),
                QuestionId = responseDto.QuestionId,
                TextAnswer = responseDto.TextAnswer,
                QuestionOptionId = responseDto.QuestionOptionId,
                Report = responseDto.Report
            };

            var createdResponse = await _repository.AddReportResponseAsync(response);

            var createdResponseDto = new ReportResponseDto
            {
                Id = createdResponse.Id,
                QuestionId = createdResponse.QuestionId,
                TextAnswer = createdResponse.TextAnswer,
                QuestionOptionId = createdResponse.QuestionOptionId,
                Report = createdResponse.Report
            };

            return Result<ReportResponseDto>.Success(createdResponseDto);
        }


        public async Task<Result<ReportResponseDto>> UpdateReportResponseAsync(Guid reportResponseId, UpdateReportResponseRequest responseDto)
        {
            var existingResponse = await _repository.GetReportResponseByIdAsync(reportResponseId);
            if (existingResponse == null)
            {
                return Result<ReportResponseDto>.Fail("The report response with the provided Id does not exist.");
            }

            existingResponse.TextAnswer = responseDto.TextAnswer;
            existingResponse.Report = responseDto.Report;
            
            var updatedResponse = await _repository.UpdateReportResponseAsync(existingResponse);

            var updatedResponseDto = new ReportResponseDto
            {
                Id = updatedResponse.Id,
                QuestionId = updatedResponse.QuestionId,
                TextAnswer = updatedResponse.TextAnswer,
                QuestionOptionId = updatedResponse.QuestionOptionId, 
                Report = updatedResponse.Report
            };

            return Result<ReportResponseDto>.Success(updatedResponseDto);
        }

        public async Task<bool> DeleteReportResponseAsync(Guid responseId)
        {
            var responseExists = await _repository.GetReportResponseByIdAsync(responseId);
            if (responseExists == null)
            {
                return false;
            }

            return await _repository.DeleteReportResponseAsync(responseId);
        }
    }
}
