using AMJNReportSystem.Application.Abstractions.Services;
using AMJNReportSystem.Application.Models;
using AMJNReportSystem.Application.Models.RequestModels;
using AMJNReportSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace AMJNReportSystem.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubmissionWindowController : ControllerBase
    {
        private readonly ISubmissionWindowService _submissionWindowService;
        public SubmissionWindowController(ISubmissionWindowService submissionWindowService)
        {
            _submissionWindowService = submissionWindowService;
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPatch("UpdateSubmissionWindow/{updateId}")]
        public async Task<IActionResult> UpdateSubmissionWindow([FromBody] UpdateSubmissionWindowRequest updateSubmission, Guid updateId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var startingDate = await _submissionWindowService.UpdateReportSubmissionWindow<SubmissionWindow>(id: updateId, request: updateSubmission);
            if (!startingDate.Succeeded)
                return Conflict(startingDate);
            return Ok(startingDate);
        }


        [HttpPost]
        public async Task<IActionResult> AddSubmissionWindow([FromBody] CreateSubmissionWindowRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var startingDate = await _submissionWindowService.CreateReportSubmissionWindow<SubmissionWindow>(model);
            return Ok(startingDate);

        }

        [HttpGet]
        public async Task<IActionResult> GetSubmissionWindowAsync(PaginationFilter filter)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _submissionWindowService.GetActiveSubmissionWindows<SubmissionWindow>(filter);
            return Ok(response);
        }

        [HttpGet("GetSubmissionWindow/{submissionWindowId}")]
        public async Task<IActionResult> GetSubmissionWindowAsync(Guid submissionWindowId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _submissionWindowService.GetSubmissionWindow(submissionWindowId);
            return Ok(response);
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{WindowSubmissions}")]
        [OpenApiOperation("Get all submission windows.")]
        public async Task<IActionResult> GetSubmissionWindows(Guid? reportTypeId, int? month, int? year, string? status, bool? isLocked, DateTime? startDate, DateTime? endDate)
        {
            var response = await _submissionWindowService.GetSubmissionWindows(reportTypeId, month, year, status, isLocked, startDate, endDate);
            return Ok(response);
        }
    }
}
