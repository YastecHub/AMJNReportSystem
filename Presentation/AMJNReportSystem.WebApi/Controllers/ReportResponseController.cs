using AMJNReportSystem.Application.Models.RequestModels;
using AMJNReportSystem.Domain.Entities;
using AMJNReportSystem.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace AMJNReportSystem.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        public async Task<IActionResult> CreateReportResponse([FromBody] CreateReportResponseRequest responseDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid request data", errors = ModelState });
            }

            var result = await _reportResponseService.CreateReportResponseAsync(responseDto);

            if (!result.Succeeded)
            {
                return BadRequest(new { message = "Failed to create report response", details = result.Messages });
            }

            return CreatedAtAction(
                nameof(GetReportResponseById), 
                new { id = result.Data.Id },
                new { id = result.Data.Id, message = "Report response created successfully" }
            );
        }

        [HttpPut("{id}")]
        [OpenApiOperation("Update an existing report response.", "Updates a response associated with a report.")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateReportResponse([FromRoute] Guid id, [FromBody] UpdateReportResponseRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid request data", errors = ModelState });
            }

            var result = await _reportResponseService.UpdateReportResponseAsync(id, request);

            if (!result.Succeeded)
            {
                return BadRequest(new { message = "Failed to update report response", details = result.Messages });
            }

            return Ok(new { id = result.Data.Id, message = "Report response updated successfully" });
        }

        [OpenApiOperation("Delete a report response.", "Deletes a response based on the provided ID.")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReportResponse([FromRoute] Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new { message = "ID cannot be empty." });

            var result = await _reportResponseService.DeleteReportResponseAsync(id);

            if (!result)
            {
                return NotFound(new { message = "Report response not found." });
            }

            return Ok(new { message = "Report response has been deleted successfully." });
        }
    }
}
