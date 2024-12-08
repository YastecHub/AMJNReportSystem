using AMJNReportSystem.Application.Abstractions.Services;
using AMJNReportSystem.Application.Models;
using AMJNReportSystem.Application.Models.DTOs;
using AMJNReportSystem.Application.Models.RequestModels;
using AMJNReportSystem.Application.Models.RequestModels.Reports;
using AMJNReportSystem.Application.Models.ResponseModels;
using AMJNReportSystem.Application.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace AMJNReportSystem.WebApi.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
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
        [ProducesResponseType(typeof(BaseResponse<ReportSubmissionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<ReportSubmissionDto>), StatusCodes.Status500InternalServerError)]
        [HttpPost("create-new-report-submission")]
        [OpenApiOperation("Create new report submission.", "")]
        public async Task<IActionResult> CreateReportSubmission([FromBody] CreateReportSubmissionRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var reportSubmission = await _reportSubmissionService.CreateAndUpdateReportSubmissionAsync(request);
            return !reportSubmission.Status ? Conflict(reportSubmission) : Ok(reportSubmission);
        }

        /// <summary>
        /// Get a specific report submission by id
        /// </summary>
        /// <param name="reportsubmissionid"></param>
        /// <param name="reportSectionId"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(BaseResponse<SubmittedReportDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<SubmittedReportDto>), StatusCodes.Status500InternalServerError)]
        [HttpGet("get-report-submission-by-id/{reportsubmissionid}/{reportSectionId}")]
        [OpenApiOperation("Get a specific report submission by id.", "")]
        public async Task<IActionResult> GetReportTypeSubmission([FromRoute]Guid reportsubmissionid, [FromRoute] Guid reportSectionId)
        {
            if (reportsubmissionid == Guid.Empty) return BadRequest("id can not be empty");
            var response = await _reportSubmissionService.GetSectionReportSubmissionAsync(reportsubmissionid,reportSectionId);
            return !response.Status ? NotFound(response) : Ok(response);
        }



        /// <summary>
        /// Get list of all report submission
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(BaseResponse<ReportSubmissionResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<ReportSubmissionResponseDto>), StatusCodes.Status500InternalServerError)]
        [HttpGet("get-all-report-type-submissions-paginated")]
        [OpenApiOperation("Get list of all report submission.", "")]
        public async Task<IActionResult> GeAlltReportTypeSubmissions(PaginationFilter filter)
        {
            var response = await _reportSubmissionService.GetAllReportTypeSubmissionsAsync(filter);
            return Ok(response);
        }

        /// <summary>
        /// Retrieves a list of all report submissions without pagination.
        /// This endpoint returns all report submissions, bypassing any pagination filters.
        /// </summary>
        /// <returns>Returns a list of all report submissions.</returns>
        [ProducesResponseType(typeof(BaseResponse<ReportSubmissionResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<ReportSubmissionResponseDto>), StatusCodes.Status500InternalServerError)]
        [HttpGet("get-all-report-type-submissions")]
        [OpenApiOperation("Get list of all report submissions without pagination.", "")]
        public async Task<IActionResult> GetAllReportTypeSubmissions()
        {
            var response = await _reportSubmissionService.GetAllReportTypeSubmissionsAsync();
            return Ok(response);
        }


        /// <summary>
        /// update a specific report submission
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="reportSubmission"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(BaseResponse<ReportSubmissionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<ReportSubmissionDto>), StatusCodes.Status500InternalServerError)]
        [HttpPut("update-report-submission{id}")]
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
        /// [ProducesResponseType(typeof(BaseResponse<ReportSubmissionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<Result<bool>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<Result<bool>>), StatusCodes.Status500InternalServerError)]
        [HttpDelete("delete-report-submission{reportSubmissionId}")]
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
        /// <returns></returns>
        [ProducesResponseType(typeof(BaseResponse<ReportSubmissionResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<ReportSubmissionResponseDto>), StatusCodes.Status500InternalServerError)]
        [HttpGet("get-report-submission-by-reportType/{reportTypeid}")]
        [OpenApiOperation("Get a specific report submission by reportType.", "")]
        [ProducesResponseType(typeof(BaseResponse<ReportSubmissionResponseDto>), StatusCodes.Status200OK)]
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
        [ProducesResponseType(typeof(BaseResponse<ReportSubmissionResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<ReportSubmissionResponseDto>), StatusCodes.Status500InternalServerError)]
        [HttpGet("get-report-submission-by-circuit-id")]
        [OpenApiOperation("Get all report submissions by  circuit ID.", "")]
        [ProducesResponseType(typeof(BaseResponse<ReportSubmissionResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReportSubmissionsByCircuitId()
        {
            var response = await _reportSubmissionService.GetReportSubmissionsByCircuitIdAsync();
            return !response.Status ? NotFound(response) : Ok(response);
        }


        /// <summary>
        /// Get all report submissions by  jammat ID
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(BaseResponse<ReportSubmissionResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<ReportSubmissionResponseDto>), StatusCodes.Status500InternalServerError)]
        [HttpGet("get-report-submission-by-jammat-id")]
        [OpenApiOperation("Get all report submissions by  jammat ID.", "")]
        [ProducesResponseType(typeof(BaseResponse<ReportSubmissionResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReportSubmissionsByJammatId()
        {
            var response = await _reportSubmissionService.GetReportSubmissionsByJamaatIdAsync();
            return !response.Status ? NotFound(response) : Ok(response);
        }


        /// <summary>
        /// Get a  report submission by submission window
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(BaseResponse<JamaatReport>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<JamaatReport>), StatusCodes.Status500InternalServerError)]
        [HttpGet("get-jamaat-report-submission-by-submission-window-id/{submissionWindowId}")]
        [OpenApiOperation("Get  jammat report submission by submissionWindowId.", "")]
        public async Task<IActionResult> GetJamaatReportsBySubmissionWindowIdAsync([FromRoute] Guid submissionWindowId)
        {
            if (submissionWindowId == Guid.Empty) return BadRequest("id cannot be empty");
            var response = await _reportSubmissionService.GetJamaatReportsBySubmissionWindowIdAsync(submissionWindowId);
            return !response.Status ? NotFound(response) : Ok(response);
        }


        /// <summary>
        /// Get a specific report submission by id
        /// </summary>
        /// <param name="reportsubmissionid"></param>
        /// <param name="reportSectionId"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
        [HttpGet("confirm-report-section-has-been-submitted/{reportsubmissionid}/{reportSectionId}")]
        [OpenApiOperation("Get a specific report submission by id.", "")]
        public async Task<IActionResult> ConfirmReportSectionHasBeenSubmittedAsync([FromRoute] Guid reportsubmissionid, [FromRoute] Guid reportSectionId)
        {
            if (reportsubmissionid == Guid.Empty) return BadRequest("id can not be empty");
            var response = await _reportSubmissionService.ConfirmReportSectionHasBeenSubmittedAsync(reportsubmissionid, reportSectionId);
            return !response.Status ? NotFound(response) : Ok(response);
        }
    }
}
