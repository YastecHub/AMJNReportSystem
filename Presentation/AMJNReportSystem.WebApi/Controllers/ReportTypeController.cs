using AMJNReportSystem.Application.Abstractions.Services;
using AMJNReportSystem.Application.Models.RequestModels;
using AMJNReportSystem.Application.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace AMJNReportSystem.WebApi.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportTypeController : BaseSecuredController
    {
        private readonly IReportTypeService _reportTypeService;

        public ReportTypeController(IReportTypeService reportTypeService)
        {
            _reportTypeService = reportTypeService;
        }

        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("create-report-type")]
        [OpenApiOperation("Create new report type.", "")]
        public async Task<IActionResult> CreateReportType([FromBody] CreateReportTypeRequest request, [FromServices] IValidator<CreateReportTypeRequest> validator)

        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid) return BadRequest(validationResult.ToDictionary());
            var reportTypeRequest = await _reportTypeService.CreateReportType(request);
            return !reportTypeRequest.Status ? Conflict(reportTypeRequest) : Ok(reportTypeRequest);
        }


        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("get-report-type{id}")]
        [OpenApiOperation("Get a specific report type by id.", "")]
        public async Task<IActionResult> GetReportType(Guid id)
        {
            if (id == Guid.Empty) return BadRequest("id can not be empty");
            var response = await _reportTypeService.GetReportType(id);
            return !response.Status ? NotFound(response) : Ok(response);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("get-report-types")]
        [OpenApiOperation("Get list of all report types.", "")]
        public async Task<IActionResult> GetReportTypes()
        {
            var response = await _reportTypeService.GetReportTypes();
            return Ok(response);
        }


        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut("update-report-type{id}")]
        [OpenApiOperation("update a specific report type.", "")]
        public async Task<IActionResult> UpdateReportType(Guid id, [FromBody] UpdateReportTypeRequest request)
        {
            if (id == Guid.Empty) return BadRequest("id can not be empty");
            var response = await _reportTypeService.UpdateReportType(id, request);
            return Ok(response);
        }


        [HttpDelete("delete-report-type/{reportSectionId}")]
        [OpenApiOperation("Delete a report type.", "Deletes a specific Report type")]
        public async Task<IActionResult> DeleteReportSection([FromRoute] Guid reportSectionId)
        {
            if (reportSectionId == Guid.Empty)
                return BadRequest(new { message = "ID cannot be empty" });

            var result = await _reportTypeService.DeleteReportType(reportSectionId);

            if (result.Succeeded)
            {
                return Ok(new { message = result.Messages });
            }

            return BadRequest(new { message = result.Messages });
        }
    }
}
