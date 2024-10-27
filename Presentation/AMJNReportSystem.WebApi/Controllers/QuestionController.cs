using AMJNReportSystem.Application.Abstractions.Services;
using AMJNReportSystem.Application.Models.DTOs;
using AMJNReportSystem.Application.Models.RequestModels;
using AMJNReportSystem.Application.Validation;
using AMJNReportSystem.Application.Wrapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace AMJNReportSystem.WebApi.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class QuestionController : BaseSecuredController
    {
        private readonly IQuestionService _questionService;
        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;

        }

        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status500InternalServerError)]
        [HttpPost("create-question")]
        [OpenApiOperation("create-question", "")] 
        public async Task<IActionResult> CreateQuestion([FromBody] CreateQuestionRequest model, [FromServices] IValidator<CreateQuestionRequest> validator)
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.ToDictionary());
            var questionRequest = await _questionService.CreateQuestion(model);
            return !questionRequest.Succeeded ? Conflict(questionRequest) : Ok(questionRequest);
        }

        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status500InternalServerError)]
        [HttpPut("update-question/{id}")]
        [OpenApiOperation("update-question", "")]
        public async Task<IActionResult> UpdateQuestion([FromBody] UpdateQuestionRequest model, [FromRoute] Guid id, [FromServices] IValidator<UpdateQuestionRequest> validator)
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.ToDictionary());
            var reportTypeRequest = await _questionService.UpdateQuestion(id, model);
            return !reportTypeRequest.Succeeded ? Conflict(reportTypeRequest) : Ok(reportTypeRequest);
        }

        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status500InternalServerError)]
        [HttpDelete("delete-question/{id}")]
        [OpenApiOperation("delete-question", "Delet question")]
        public async Task<IActionResult> DeleteQuestion([FromRoute] Guid id)
        {
            var reportTypeRequest = await _questionService.DeleteQuestion(id);
            return !reportTypeRequest.Succeeded ? Conflict(reportTypeRequest) : Ok(reportTypeRequest);
        }



        [ProducesResponseType(typeof(Result<QuestionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<QuestionDto>), StatusCodes.Status500InternalServerError)]
        [HttpGet("get-question-by-id/{id}")]
        [OpenApiOperation("get-question-by-id", "Get a specific question by id.")]
        public async Task<IActionResult> GetQuestion(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("id can not be empty");
            var response = await _questionService.GetQuestion(id);
            return !response.Succeeded ? NotFound(response) : Ok(response);
        }


        [ProducesResponseType(typeof(Result<QuestionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<QuestionDto>), StatusCodes.Status500InternalServerError)]
        [HttpGet("get-all-questions")]
        [OpenApiOperation("Get list of all question.", "")]
        public async Task<IActionResult> GetQuestions()
        {
            var response = await _questionService.GetQuestions();
            return Ok(response);
        }

        [ProducesResponseType(typeof(Result<QuestionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<QuestionDto>), StatusCodes.Status500InternalServerError)]
        [HttpGet("get-question-by-section-id/{sectionId}")]
        [OpenApiOperation("get-question-by-section-id", "Get list of all question by section")]
        public async Task<IActionResult> GetQuestionsBySection([FromRoute] Guid sectionId)
        {
            var questionSection = await _questionService.GetQuestionsBySection(sectionId);
            return Ok(questionSection);
        }


        [ProducesResponseType(typeof(Result<QuestionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<QuestionDto>), StatusCodes.Status500InternalServerError)]
        [HttpGet("get-question-options-by-question-id/{questionId}")]
        [OpenApiOperation("get-question-options-by-question-id", "Get list of all question option")]
        public async Task<IActionResult> GetQuestionOptions([FromRoute] Guid questionId)
        {
            var questionOption = await _questionService.GetQuestionOptions(questionId);
            return Ok(questionOption);
        }


        [ProducesResponseType(typeof(BaseResponse<List<ReportTypeSectionQuestion>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<List<ReportTypeSectionQuestion>>), StatusCodes.Status500InternalServerError)]
        [HttpGet("get-report-section-questions-by-report-id/{reportTypeId}")]
        [OpenApiOperation("get-report-section-questions-by-report-id", "Get list of all question in sections")]
        public async Task<IActionResult> GetQuestionReportSectionByReportTypeId([FromRoute] Guid reportTypeId)
        {
            var questionOption = await _questionService.GetQuestionReportSectionByReportTypeId(reportTypeId);
            return Ok(questionOption);
        }


        [ProducesResponseType(typeof(BaseResponse<List<ReportTypeSectionQuestionSlim>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<List<ReportTypeSectionQuestionSlim>>), StatusCodes.Status500InternalServerError)]
        [HttpGet("get-report-section-by-report-id/{reportTypeId}")]
        [OpenApiOperation("get-question-options-by-question-id", "Get list of all section in question")]
        public async Task<IActionResult> GetQuestionReportSectionByReportTypeIdSlim([FromRoute] Guid reportTypeId)
        {
            var questionOption = await _questionService.GetQuestionReportSectionByReportTypeIdSlim(reportTypeId);
            return Ok(questionOption);
        }
    }
}
