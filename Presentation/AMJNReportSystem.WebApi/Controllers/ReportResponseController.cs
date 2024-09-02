using AMJNReportSystem.Domain.Entities;
using AMJNReportSystem.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AMJNReportSystem.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportResponseController : ControllerBase
    {
        private readonly IReportResponseService _reportResponseService;

        public ReportResponseController(IReportResponseService reportResponseService)
        {
            _reportResponseService = reportResponseService;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReportResponseDto>>> GetAllReportResponses()
        {
            var responses = await _reportResponseService.GetAllReportResponsesAsync();
            return Ok(responses);
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ReportResponseDto>> GetReportResponseById(Guid id)
        {
            if (id == Guid.Empty) return BadRequest("ID cannot be empty.");

            var response = await _reportResponseService.GetReportResponseByIdAsync(id);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPost]
        public async Task<ActionResult<ReportResponseDto>> CreateReportResponse([FromBody] ReportResponseDto responseDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdResponse = await _reportResponseService.CreateReportResponseAsync(responseDto);

            return CreatedAtAction(nameof(GetReportResponseById), new { id = createdResponse.Id }, createdResponse);
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut("{id}")]
        public async Task<ActionResult<ReportResponseDto>> UpdateReportResponse(Guid id, [FromBody] ReportResponseDto responseDto)
        {
            if (id != responseDto.Id)
            {
                return BadRequest("ID in URL and request body do not match.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedResponse = await _reportResponseService.UpdateReportResponseAsync(responseDto);

            return Ok(updatedResponse);
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReportResponse(Guid id)
        {
            if (id == Guid.Empty) return BadRequest("ID cannot be empty.");

            var result = await _reportResponseService.DeleteReportResponseAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
