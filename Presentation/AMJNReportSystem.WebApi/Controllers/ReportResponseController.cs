using AMJNReportSystem.Domain.Entities;
using AMJNReportSystem.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

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

        [OpenApiOperation("Get all report responses.", "Retrieves a list of all report responses.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReportResponseDto>>> GetAllReportResponses()
        {
            var responses = await _reportResponseService.GetAllReportResponsesAsync();
            return Ok(responses);
        }

        [OpenApiOperation("Get a report response by ID.", "Retrieves a specific report response by its unique ID.")]
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

        [OpenApiOperation("Create a new report response.", "Creates a new response associated with a report.")]
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

            return CreatedAtAction(nameof(GetReportResponseById), new { id = createdResponse.Data }, createdResponse);
        }

        [OpenApiOperation("Update an existing report response.", "Updates a response based on the provided ID.")]
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

        [OpenApiOperation("Delete a report response.", "Deletes a response based on the provided ID.")]
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
