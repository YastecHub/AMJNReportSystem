using AMJNReportSystem.Application.Abstractions.Services;
using AMJNReportSystem.Application.Models;
using AMJNReportSystem.Application.Models.RequestModels;
using AMJNReportSystem.Application.Models.RequestModels.Reports;
using AMJNReportSystem.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace AMJNReportSystem.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportSubmissionController : ControllerBase
    {
        private readonly IReportSubmissionService _reportSubmissionService;
        public ReportSubmissionController(IReportSubmissionService reportSubmissionService)
        {
            _reportSubmissionService = reportSubmissionService;
        }

        /// <summary>
        /// Create new report submission
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("create-new-report-submission")]
        [OpenApiOperation("Create new report submission.", "")]
        public async Task<IActionResult> CreateReportSubmission([FromBody] CreateReportSubmissionRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var reportSubmission = await _reportSubmissionService.CreateReportTypeSubmissionAsync(request);
            return !reportSubmission.Status ? Conflict(reportSubmission) : Ok(reportSubmission);
        }

        /// <summary>
        /// Get a specific report submission by id
        /// </summary>
        /// <param name="reportTypeSubmissionId"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [OpenApiOperation("Get a specific report submission by id.", "")]
        public async Task<IActionResult> GetReportTypeSubmission(Guid reportsubmissionid)
        {
            if (reportsubmissionid == Guid.Empty) return BadRequest("id can not be empty");
            var response = await _reportSubmissionService.GetReportTypeSubmissionByIdAsync(reportsubmissionid);
            return !response.Status ? NotFound(response) : Ok(response);
        }



        /// <summary>
        /// Get list of all report submission
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("get-all-report-type-submissions")]
        [OpenApiOperation("Get list of all report submission.", "")]
        public async Task<IActionResult> GeAlltReportTypeSubmissions(PaginationFilter filter)
        {
            var response = await _reportSubmissionService.GetAllReportTypeSubmissionsAsync(filter);
            return Ok(response);
        }

        /// <summary>
        /// update a specific report submission
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="reportSubmission"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut("{id}")]
        [OpenApiOperation("update a specific report submission.", "")]
        public async Task<IActionResult> UpdateReportSubmission(Guid id, UpdateReportSubmission request)
        {
            if (id == Guid.Empty) return BadRequest("id can not be empty");
            var response = await _reportSubmissionService.UpdateReportSubmission(id, request);
            return Ok(response);
        }

        /// <summary>
        /// Delete a report submission
        /// </summary>
        /// <param name="reportSubmissionId"></param>
        /// <returns></returns>
        [HttpDelete("{reportSubmissionId}")]
        [OpenApiOperation("Delete a report submission.", "Deletes a specific Report Submission")]
        public async Task<IActionResult> DeleteReportSubmission([FromRoute] Guid reportSubmissionId)
        {
            if (reportSubmissionId == Guid.Empty)
                return BadRequest(new { message = "ID cannot be empty" });

            var result = await _reportSubmissionService.DeleteReportSubmission(reportSubmissionId);

            if (result.Succeeded)
            {
                return Ok(new { message = result.Messages });
            }

            return BadRequest(new { message = result.Messages });
        }

        /// <summary>
        /// Get a specific report submission by reportType
        /// </summary>
        /// <param name="reportTypeid"></param>
        /// <returns></returns>
        [HttpGet("{reportTypeid:guid}")]
        [OpenApiOperation("Get a specific report submission by reportType.", "")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetReportTypeSubmissionByRportType([FromRoute] Guid reportTypeid)
        {
            if (reportTypeid == Guid.Empty) return BadRequest("id cannot be empty");
            var response = await _reportSubmissionService.GetReportSubmissionsByReportTypeAsync(reportTypeid);
            return !response.Status ? NotFound(response) : Ok(response);
        }

        /// <summary>
        /// Get all report submissions by  circuit ID
        /// </summary>
        /// <returns></returns>
        [HttpGet("circuit")]
        [OpenApiOperation("Get all report submissions by  circuit ID.", "")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetReportSubmissionsByCircuitId()
        {
            var response = await _reportSubmissionService.GetReportSubmissionsByCircuitIdAsync();
            return !response.Status ? NotFound(response) : Ok(response);
        }

        /// <summary>
        /// Get all report submissions by  jammat ID
        /// </summary>
        /// <returns></returns>
        [HttpGet("jammat")]
        [OpenApiOperation("Get all report submissions by  jammat ID.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetReportSubmissionsByJammatId()
        {
            var response = await _reportSubmissionService.GetReportSubmissionsByJamaatIdAsync();
            return !response.Status ? NotFound(response) : Ok(response);
        }
    }
}
