using AMJNReportSystem.Application.Abstractions.Services;
using AMJNReportSystem.Application.Models;
using AMJNReportSystem.Application.Models.RequestModels;
using AMJNReportSystem.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace AMJNReportSystem.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
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
		[HttpPost("Create SubmissionWindow")]
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
        [HttpPatch("Update SubmissionWindo")]
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
        
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpDelete("Delete SubmissionWindow")] 
		[OpenApiOperation(" Delete submission window.", "")] 
		public async Task<IActionResult> DeleteSubmissionWindow(Guid subWindowId)
        {
			var response = await _submissionWindowService.DeleteSubmissionWindow(subWindowId);
            if (!response.Succeeded)
                return Conflict(response);
            return Ok(response);
        } 

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("GetSubmissionWindow/{id}")]
        [OpenApiOperation("Get submission window by id", "")]
        public async Task<IActionResult> GetSubmissionWindowAsync(Guid submissionWindowId)
        { 
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _submissionWindowService.GetSubmissionWindow(submissionWindowId);
            return Ok(response);
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("GetAllSubmissionWindow")]
        [OpenApiOperation("Get all submission windows.","")]
        public async Task<IActionResult> GetSubmissionWindows()
        {
            var response = await _submissionWindowService.GetSubmissionWindows();
            return Ok(response);
        }
    }
}
