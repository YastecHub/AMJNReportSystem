using Microsoft.AspNetCore.Mvc;
using AMJNReportSystem.Domain.Entities;
using AMJNReportSystem.Domain.Repositories;

namespace AMJNReportSystem.Application.Controllers
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

       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReportResponseDto>>> GetAllResponses()
        {
            var responses = await _reportResponseService.GetAllReportResponsesAsync();
            return Ok(responses);
        }

       
        [HttpGet("{id}")]
        public async Task<ActionResult<ReportResponseDto>> GetResponseById(Guid id)
        {
            var response = await _reportResponseService.GetReportResponseByIdAsync(id);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

    
        [HttpPost]
        public async Task<ActionResult<ReportResponseDto>> CreateResponse([FromBody] ReportResponseDto responseDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdResponse = await _reportResponseService.CreateReportResponseAsync(responseDto);

            return CreatedAtAction(nameof(GetResponseById), new { id = createdResponse.Id }, createdResponse);
        }

      
        [HttpPut("{id}")]
        public async Task<ActionResult<ReportResponseDto>> UpdateResponse(Guid id, [FromBody] ReportResponseDto responseDto)
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

      
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResponse(Guid id)
        {
            var result = await _reportResponseService.DeleteReportResponseAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
