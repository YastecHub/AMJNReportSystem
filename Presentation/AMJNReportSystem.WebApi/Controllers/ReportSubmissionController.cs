using AMJNReportSystem.Application.Abstractions.Services;
using AMJNReportSystem.Application.Models;
using AMJNReportSystem.Application.Models.RequestModels;
using AMJNReportSystem.Application.Models.RequestModels.Reports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace AMJNReportSystem.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  //  [Authorize]
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
        [HttpGet("get-report-type-submission/{id}")]
        [OpenApiOperation("Get a specific report submission by id.", "")]
        public async Task<IActionResult> GetReportTypeSubmission(Guid reportTypeSubmissionId)
        {
            if (reportTypeSubmissionId == Guid.Empty) return BadRequest("id can not be empty");
            var response = await _reportSubmissionService.GetReportTypeSubmissionByIdAsync(reportTypeSubmissionId);
            return !response.Status ? NotFound(response) : Ok(response);
        }

        /// <summary>
        /// Get list of all report submission
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("get-report-type-submissions")]
        [OpenApiOperation("Get list of  report submission.", "")]
        public async Task<IActionResult> GetReportTypeSubmissions(PaginationFilter filter)
        {
            var response = await _reportSubmissionService.GetReportTypeSubmissionsAsync(filter);
            return Ok(response);
        }

        /// <summary>
        /// Get list of all report submission
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("get-all-report-type-submissions")]
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

    }
}
