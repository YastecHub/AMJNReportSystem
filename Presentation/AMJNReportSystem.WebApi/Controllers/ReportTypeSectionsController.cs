using Application.Abstractions.Services;
using Application.Models.RequestModels;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace AMJNReportSystem.WebApi.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ReportTypeSectionsController : ControllerBase
    {
        private readonly IReportTypeSectionService _reportTypeSectionService;

        public ReportTypeSectionsController(IReportTypeSectionService reportTypeSectionService)
        {
            _reportTypeSectionService = reportTypeSectionService;
        }

        [HttpPost]
        [OpenApiOperation("Create a new section in a report type .", "")]
        public async Task<IActionResult> CreateReportTypeSection([FromBody] CreateReportTypeSectionRequest model)
        {
            var response = await _reportTypeSectionService.CreateReportTypeSection(model);
            return Ok(response);
        }

        [HttpPut("{reportTypeSectionId}")]
        [OpenApiOperation("Update the name and description of a report type section.", "")]
        public async Task<IActionResult> UpdateReportTypeSection([FromRoute] Guid reportTypeSectionId, [FromBody] UpdateReportTypeSectionRequest model)
        {
            var response = await _reportTypeSectionService.UpdateReportTypeSection(reportTypeSectionId, model);
            return Ok(response);
        }

        [HttpGet("{reportTypeSectionId}")]
        [OpenApiOperation("Get a report type section by id.", "")]
        public async Task<IActionResult> GetReportTypeSection([FromRoute] Guid reportTypeSectionId)
        {
            var response = await _reportTypeSectionService.GetReportTypeSection(reportTypeSectionId);
            return Ok(response);
        }

        [HttpGet("{reportTypeId}/reportTypeSections")]
        [OpenApiOperation("Get list of all sections in a report type.", "")]
        public async Task<IActionResult> GetReportTypeSectionsByReportType([FromRoute] Guid reportTypeId)
        {
            var response = await _reportTypeSectionService.GetReportTypeSections(reportTypeId);
            return Ok(response);
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut("{reportTypeSectionId}/{state}")]
        [OpenApiOperation("update a Report-Type sction Activeness state.", "")]
        public async Task<IActionResult> SetReportTypeState([FromRoute] Guid reportTypeSectionId, bool state)
        {
            if (reportTypeSectionId == Guid.Empty) return BadRequest("id can not be empty");
            var response = await _reportTypeSectionService.SetReportTypeSectionActiveness(reportTypeSectionId, state);
            return Ok(response);
        }
    }
}
