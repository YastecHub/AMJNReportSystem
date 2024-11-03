using AMJNReportSystem.Application.Abstractions.Services;
using AMJNReportSystem.Application.Models.DTOs;
using AMJNReportSystem.Application.Models.RequestModels;
using AMJNReportSystem.Application.Models.ResponseModels;
using AMJNReportSystem.Application.Wrapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace AMJNReportSystem.WebApi.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportSectionController : ControllerBase
    {
        private readonly IReportSectionService _reportSectionService;

        public ReportSectionController(IReportSectionService reportSectionService)
        {
            _reportSectionService = reportSectionService;
        }


        [ProducesResponseType(typeof(BaseResponse<ReportSectionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<ReportSectionDto>), StatusCodes.Status500InternalServerError)]
        [HttpPost("create-new-report-section")]
        [OpenApiOperation("Create a new report section.", "Creates a new Report Section")]
        public async Task<IActionResult> CreateReportSection( 
             [FromBody] CreateReportSectionRequest model,
             [FromServices] IValidator<CreateReportSectionRequest> validator)
        {

            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }

            var result = await _reportSectionService.CreateReportSection(model);
            if (!result.Status)
            {
                return Conflict(result);
            }

            return Ok(result);
        }


        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
        [HttpPut("update-report-section/{reportSectionId}")]
        [OpenApiOperation("Update the details of a report section.", "Updates an existing Report Section")]
        public async Task<IActionResult> UpdateReportSection([FromRoute] Guid reportSectionId, [FromBody] UpdateReportSectionRequest model, [FromServices] IValidator<UpdateReportSectionRequest> validator)
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.ToDictionary());

            if (model == null || reportSectionId == Guid.Empty)
                return BadRequest(new { message = "Invalid request or Report Section ID" });

            var result = await _reportSectionService.UpdateReportSection(reportSectionId, model);

            if (!result.Status)
                return BadRequest(result);

            return Ok(result);
        }

        [ProducesResponseType(typeof(BaseResponse<ReportSectionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<ReportSectionDto>), StatusCodes.Status500InternalServerError)]
        [HttpGet("get-report-section-by-id/{reportSectionId}")]
        [OpenApiOperation("Get a report section by ID.", "Retrieves a specific Report Section by its ID")]
        public async Task<IActionResult> GetReportSection([FromRoute] Guid reportSectionId)
        {
            var result = await _reportSectionService.GetReportSection(reportSectionId);
            if (result.Status)
            {
                return Ok(result); 
            }
            return NotFound(result);
        }

        [ProducesResponseType(typeof(BaseResponse<ReportSectionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<ReportSectionDto>), StatusCodes.Status500InternalServerError)]
        [HttpGet("get-report-section-by-report-type/{reportTypeId}")]
        [OpenApiOperation("Get all report sections for a specific report type.", "")]
        public async Task<IActionResult> GetReportSectionsByReportType([FromRoute] Guid reportTypeId)
        {
            var result = await _reportSectionService.GetReportSections(reportTypeId);
            if (result.Status)
            {
                return Ok(result);
            }
            return NotFound(result);
        }


        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status500InternalServerError)]
        [HttpPut("update-reportsection-activeness/{reportSectionId}/{state}")]
        [OpenApiOperation("Update the activeness state of a report section.", "Sets the activeness state of a specific Report Section")]
        public async Task<IActionResult> SetReportSectionActiveness([FromRoute] Guid reportSectionId, [FromRoute] bool state)
        {
            if (reportSectionId == Guid.Empty)
                return BadRequest(new { message = "ID cannot be empty" });

            var result = await _reportSectionService.SetReportSectionActiveness(reportSectionId, state);
            if (result.Status)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status500InternalServerError)]
        [HttpDelete("delete-report-section/{reportSectionId}")]
        [OpenApiOperation("Delete a report section.", "Deletes a specific Report Section")]
        public async Task<IActionResult> DeleteReportSection([FromRoute] Guid reportSectionId)
        {
            if (reportSectionId == Guid.Empty)
                return BadRequest(new { message = "ID cannot be empty" });

            var result = await _reportSectionService.DeleteReportSection(reportSectionId);

            if (result.Status)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
