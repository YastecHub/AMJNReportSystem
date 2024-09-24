using AMJNReportSystem.Application.Abstractions.Services;
using AMJNReportSystem.Application.Models.RequestModels;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace AMJNReportSystem.WebApi.Controllers
{
    [Route("api/[controller]")]
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

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[HttpGet("report-types")]
        //[OpenApiOperation("Get list of report types.", "")]
        //public async Task<IActionResult> GetReportTypes()
        //{
        //    var qaidReportType = await _reportTypeService.GetQaidReportTypes();
        //    return Ok(qaidReportType);
        //}

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

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut("set-report-type-state{id}/state")]
        [OpenApiOperation("update a Report Type Activeness State", "")]
        public async Task<IActionResult> SetReportTypeState([FromRoute] Guid reportTypeId, bool state)
        {
            if (reportTypeId == Guid.Empty) return BadRequest("id can not be empty");
            var response = await _reportTypeService.SetReportTypeActiveness(reportTypeId, state);
            return Ok(response);
        }

    }
}
