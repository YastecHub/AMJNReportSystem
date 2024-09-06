using AMJNReportSystem.Application.Abstractions.Services;
using AMJNReportSystem.Application.Models;
using AMJNReportSystem.Application.Models.RequestModels;
using AMJNReportSystem.Domain.Entities;
using FluentValidation;
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
		[ProducesResponseType(StatusCodes.Status409Conflict)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[HttpPost("Crewate SubmissionWindow")]
		[OpenApiOperation("Create new submission window.", "")]
		public async Task<IActionResult> AddSubmissionWindow([FromBody] CreateSubmissionWindowRequest model, [FromServices] IValidator<CreateSubmissionWindowRequest> validator)
		{
			var validationResult = await validator.ValidateAsync(model);
			if (!validationResult.IsValid)
				return BadRequest(validationResult.ToDictionary());
			var submissionWindow = await _submissionWindowService.CreateReportSubmissionWindow<SubmissionWindow>(model);
			return Ok(submissionWindow);

		}


		[ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPatch("Update SubmissionWindow/{id}")]
		[OpenApiOperation(" update submission window.", "")]
		public async Task<IActionResult> UpdateSubmissionWindow([FromBody] UpdateSubmissionWindowRequest updateSubmission, Guid updateId, [FromServices] IValidator<UpdateSubmissionWindowRequest> validator)
        {
			var validationResult = await validator.ValidateAsync(updateSubmission);
			if (!validationResult.IsValid)
				return BadRequest(validationResult.ToDictionary());
			var response = await _submissionWindowService.UpdateReportSubmissionWindow<SubmissionWindow>(id: updateId, request: updateSubmission);
            if (!response.Succeeded)
                return Conflict(response);
            return Ok(response);
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
