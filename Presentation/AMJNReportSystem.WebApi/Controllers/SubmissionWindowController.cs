using AMJNReportSystem.Application.Abstractions.Services;
using AMJNReportSystem.Application.Models.RequestModels;
using AMJNReportSystem.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace AMJNReportSystem.WebApi.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
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
		[HttpPost("create-submission-window")]
		[OpenApiOperation("create-submissionWindow", "Create new submission window.")]
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
        [HttpPatch("update-submission-window/{submissionWindowId}")]
		[OpenApiOperation("update-submissionWindow", " update submission window.")]
		public async Task<IActionResult> UpdateSubmissionWindow([FromBody] UpdateSubmissionWindowRequest updateSubmission,[FromRoute] Guid submissionWindowId, [FromServices] IValidator<UpdateSubmissionWindowRequest> validator)
        {
			var validationResult = await validator.ValidateAsync(updateSubmission);
			if (!validationResult.IsValid)
				return BadRequest(validationResult.ToDictionary());
			var response = await _submissionWindowService.UpdateReportSubmissionWindow<SubmissionWindow>(id: submissionWindowId, request: updateSubmission);
            if (!response.Succeeded)
                return Conflict(response);
            return Ok(response);
        }
        
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpDelete("delete-submission-window")] 
		[OpenApiOperation("delete-submissionWindow", "Delete submission window.")] 
		public async Task<IActionResult> DeleteSubmissionWindow(Guid subWindowId)
        {
			var response = await _submissionWindowService.DeleteSubmissionWindow(subWindowId);
            if (!response.Succeeded)
                return Conflict(response);
            return Ok(response);
        } 

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("get-submission-window/{submissionWindowId}")]
        [OpenApiOperation("Get submission window by id", "")]
        public async Task<IActionResult> GetSubmissionWindowAsync([FromRoute]Guid submissionWindowId)
        { 
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _submissionWindowService.GetSubmissionWindow(submissionWindowId);
            return Ok(response);
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("get-all-submission-window")]
        [OpenApiOperation("get-all-submissionwindow", "Get all submission windows.")]
        public async Task<IActionResult> GetSubmissionWindows()
        {
            var response = await _submissionWindowService.GetSubmissionWindows();
            return Ok(response);
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("get-all-active-submission-windows/{id}")] 
        [OpenApiOperation("Get all active submission windows.","")]
        public async Task<IActionResult> GetActiveSubmissionWindows([FromRoute]Guid id)
        {
            var response = await _submissionWindowService.GetActiveSubmissionWindows(id);
            return Ok(response);
        }

    }
}
