using AMJNReportSystem.Application.Abstractions.Services;
using AMJNReportSystem.Application.Models.DTOs;
using AMJNReportSystem.Application.Models.RequestModels;
using AMJNReportSystem.Application.Wrapper;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace AMJNReportSystem.WebApi.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class SectionQuestionController : ControllerBase
    {
        private readonly ISectionQuestionService _sectionQuestionService;

        public SectionQuestionController(ISectionQuestionService sectionQuestionService)
        {
            _sectionQuestionService = sectionQuestionService;
        }

        [ProducesResponseType(typeof(BaseResponse<QuestionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<QuestionDto>), StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> AddQuestion([FromBody] ReportQuestionRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var reportQuestion = await _sectionQuestionService.AddQuestion(request);
            if (!reportQuestion.Succeeded) return Conflict(reportQuestion);
            return Ok(reportQuestion);
        }

        [ProducesResponseType(typeof(BaseResponse<QuestionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<QuestionDto>), StatusCodes.Status500InternalServerError)]
        [HttpGet("GetQuestion/{questionId}")]
        public async Task<IActionResult> GetQuestion(Guid questionId)
        {
            if (questionId == Guid.Empty) return BadRequest("id can not be empty");
            var question = await _sectionQuestionService.GetQuestion(questionId);
            return Ok(question);
        }

        [ProducesResponseType(typeof(Result<QuestionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<QuestionDto>), StatusCodes.Status500InternalServerError)]
        [HttpGet("GetSectionQuestionsBySectionId/{sectionId}")]
        public async Task<IActionResult> GetSectionQuestionsBySectionId(Guid sectionId)
        {
            if (sectionId == Guid.Empty) return BadRequest("id can not be empty");
            var response = await _sectionQuestionService.GetSectionQuestions(sectionId);
            return Ok(response);
        }

        [ProducesResponseType(typeof(Result<ReportQuestionOptionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<ReportQuestionOptionDto>), StatusCodes.Status500InternalServerError)]
        [HttpGet("{reportTypeId}/questions")]
        public async Task<IActionResult> GetReportTypeQuestions(Guid reportTypeId)
        {
            if (reportTypeId == Guid.Empty) return BadRequest("id can not be empty");
            var response = await _sectionQuestionService.GetReportTypeQuestions(reportTypeId);
            return Ok(response);
        }

        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status500InternalServerError)]
        [HttpPatch("UpdateQuestionPoint/{questionId}")]
        public async Task<IActionResult> UpdateQuestionPoint([FromRoute] Guid questionId, [FromQuery] double point)
        {
            if (questionId == Guid.Empty) return BadRequest("id can not be empty");
            var response = await _sectionQuestionService.UpdateQuestionPoint(questionId, point);
            return Ok(response);
        }

        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status500InternalServerError)]
        [HttpPatch("UpdateQuestionText/{questionId}")]
        public async Task<IActionResult> UpdateQuestionText([FromRoute] Guid questionId, [FromQuery] string text)
        {
            if (questionId == Guid.Empty) return BadRequest("id can not be empty");
            var response = await _sectionQuestionService.UpdateQuestionText(questionId, text);
            return Ok(response);
        }

        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status500InternalServerError)]
        [HttpPut("{questionId}/{state}")]
        [OpenApiOperation("update a question Activeness State.", "")]
        public async Task<IActionResult> SetReportTypeState([FromRoute] Guid questionId, bool state)
        {
            if (questionId == Guid.Empty) return BadRequest("id can not be empty");
            var response = await _sectionQuestionService.QuestionActivenessState(questionId, state);
            return Ok(response);
        }

    }
}
