using AMJNReportSystem.Application.Abstractions.Services;
using AMJNReportSystem.Application.Models.RequestModels;
using AMJNReportSystem.Application.Services;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace AMJNReportSystem.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionQuestionController : ControllerBase
    {
        private readonly ISectionQuestionService _sectionQuestionService;

        public SectionQuestionController(ISectionQuestionService sectionQuestionService)
        {
            _sectionQuestionService = sectionQuestionService;
        }

        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost]
        public async Task<IActionResult> AddQuestion([FromBody] ReportQuestionRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var reportQuestion = await _sectionQuestionService.AddQuestion(request);
            if (!reportQuestion.Succeeded) return Conflict(reportQuestion);
            return Ok(reportQuestion);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("GetQuestion/{questionId}")]
        public async Task<IActionResult> GetQuestion(Guid questionId)
        {
            if (questionId == Guid.Empty) return BadRequest("id can not be empty");
            var question = await _sectionQuestionService.GetQuestion(questionId);
            return Ok(question);
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("GetSectionQuestionsBySectionId/{sectionId}")]
        public async Task<IActionResult> GetSectionQuestionsBySectionId(Guid sectionId)
        {
            if (sectionId == Guid.Empty) return BadRequest("id can not be empty");
            var response = await _sectionQuestionService.GetSectionQuestions(sectionId);
            return Ok(response);
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{reportTypeId}/questions")]
        public async Task<IActionResult> GetReportTypeQuestions(Guid reportTypeId)
        {
            if (reportTypeId == Guid.Empty) return BadRequest("id can not be empty");
            var response = await _sectionQuestionService.GetReportTypeQuestions(reportTypeId);
            return Ok(response);
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPatch("UpdateQuestionPoint/{questionId}")]
        public async Task<IActionResult> UpdateQuestionPoint([FromRoute] Guid questionId, [FromQuery] double point)
        {
            if (questionId == Guid.Empty) return BadRequest("id can not be empty");
            var response = await _sectionQuestionService.UpdateQuestionPoint(questionId, point);
            return Ok(response);
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPatch("UpdateQuestionText/{questionId}")]
        public async Task<IActionResult> UpdateQuestionText([FromRoute] Guid questionId, [FromQuery] string text)
        {
            if (questionId == Guid.Empty) return BadRequest("id can not be empty");
            var response = await _sectionQuestionService.UpdateQuestionText(questionId, text);
            return Ok(response);
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
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
