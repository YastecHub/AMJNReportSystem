using AMJNReportSystem.Application.Abstractions.Services;
using AMJNReportSystem.Application.Models.RequestModels;
using AMJNReportSystem.Application.Models.ResponseModels;
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


        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(object))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(object))]
        [HttpPost]
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
            if (!result.Succeeded)
            {
                return Conflict(new { message = result.Messages });
            }

            return CreatedAtAction(nameof(GetReportSection),
                new { reportSectionId = result.Data.Id },
                new { id = result.Data.Id, message = "Report section created successfully" });
        }


        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(object))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(object))]
        [HttpPut("{reportSectionId}")]
        [OpenApiOperation("Update the details of a report section.", "Updates an existing Report Section")]
        public async Task<IActionResult> UpdateReportSection([FromRoute] Guid reportSectionId, [FromBody] UpdateReportSectionRequest model, [FromServices] IValidator<UpdateReportSectionRequest> validator)
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.ToDictionary());

            if (model == null || reportSectionId == Guid.Empty)
                return BadRequest(new { message = "Invalid request or Report Section ID" });

            var result = await _reportSectionService.UpdateReportSection(reportSectionId, model);

            if (!result.Succeeded)
                return BadRequest(new { message = result.Messages });

            return Ok(new { message = "Report section updated successfully" });
        }


        [HttpGet("{reportSectionId}")]
        [OpenApiOperation("Get a report section by ID.", "Retrieves a specific Report Section by its ID")]
        public async Task<IActionResult> GetReportSection([FromRoute] Guid reportSectionId)
        {
            var result = await _reportSectionService.GetReportSection(reportSectionId);
            if (result.Succeeded)
                return Ok(result.Data);

            return NotFound(result.Messages);
        }

        [HttpGet("{reportTypeId}/reportSections")]
        [OpenApiOperation("Get all report sections for a specific report type.", "Retrieves all Report Sections associated with a Report Type")]
        public async Task<IActionResult> GetReportSectionsByReportType([FromRoute] Guid reportTypeId)
        {
            var result = await _reportSectionService.GetReportSections(reportTypeId);
            if (result.Succeeded)
                return Ok(result.Data);

            return NotFound(result.Messages);
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(object))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(object))]
        [HttpPut("{reportSectionId}/activeness/{state}")]
        [OpenApiOperation("Update the activeness state of a report section.", "Sets the activeness state of a specific Report Section")]
        public async Task<IActionResult> SetReportSectionActiveness([FromRoute] Guid reportSectionId, [FromRoute] bool state)
        {
            if (reportSectionId == Guid.Empty)
                return BadRequest(new { message = "ID cannot be empty" });

            var result = await _reportSectionService.SetReportSectionActiveness(reportSectionId, state);
            if (result.Succeeded)
            {
                return Ok(new { message = result.Messages });
            }

            return BadRequest(new { message = result.Messages });
        }

        [HttpDelete("{reportSectionId}")]
        [OpenApiOperation("Delete a report section.", "Deletes a specific Report Section")]
        public async Task<IActionResult> DeleteReportSection([FromRoute] Guid reportSectionId)
        {
            if (reportSectionId == Guid.Empty)
                return BadRequest(new { message = "ID cannot be empty" });

            var result = await _reportSectionService.DeleteReportSection(reportSectionId);

            if (result.Succeeded)
            {
                return Ok(new { message = result.Messages });
            }

            return BadRequest(new { message = result.Messages });
        }
    }
}
