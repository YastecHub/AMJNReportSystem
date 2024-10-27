using AMJNReportSystem.Application.Abstractions.Services;
using AMJNReportSystem.Application.Models.DTOs;
using AMJNReportSystem.Application.Models.RequestModels;
using AMJNReportSystem.Application.Services;
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
    public class ReportTypeController : BaseSecuredController
    {
        private readonly IReportTypeService _reportTypeService;

        public ReportTypeController(IReportTypeService reportTypeService)
        {
            _reportTypeService = reportTypeService;
        }

        [ProducesResponseType(typeof(BaseResponse<ReportTypeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<ReportTypeDto>), StatusCodes.Status500InternalServerError)]
        [HttpPost("create-report-type")]
        [OpenApiOperation("Create new report type.", "")]
        public async Task<IActionResult> CreateReportType([FromBody] CreateReportTypeRequest request, [FromServices] IValidator<CreateReportTypeRequest> validator)

        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid) return BadRequest(validationResult.ToDictionary());
            var reportTypeRequest = await _reportTypeService.CreateReportType(request);
            return !reportTypeRequest.Status ? Conflict(reportTypeRequest) : Ok(reportTypeRequest);
        }


        [ProducesResponseType(typeof(BaseResponse<ReportTypeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<ReportTypeDto>), StatusCodes.Status500InternalServerError)]
        [HttpGet("get-report-type{id}")]
        [OpenApiOperation("Get a specific report type by id.", "")]
        public async Task<IActionResult> GetReportType(Guid id)
        {
            if (id == Guid.Empty) return BadRequest("id can not be empty");
            var response = await _reportTypeService.GetReportType(id);
            return !response.Status ? NotFound(response) : Ok(response);
        }

        [ProducesResponseType(typeof(BaseResponse<ReportTypeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<ReportTypeDto>), StatusCodes.Status500InternalServerError)]
        [HttpGet("get-report-types")]
        [OpenApiOperation("Get list of all report types.", "")]
        public async Task<IActionResult> GetReportTypes()
        {
            var response = await _reportTypeService.GetReportTypes();
            return Ok(response);
        }

        [ProducesResponseType(typeof(BaseResponse<ReportTypeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<ReportTypeDto>), StatusCodes.Status500InternalServerError)]
        [HttpPut("update-report-type{id}")]
        [OpenApiOperation("update a specific report type.", "")]
        public async Task<IActionResult> UpdateReportType(Guid id, [FromBody] UpdateReportTypeRequest request)
        {
            if (id == Guid.Empty) return BadRequest("id can not be empty");
            var response = await _reportTypeService.UpdateReportType(id, request);
            return Ok(response);
        }

        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status500InternalServerError)]
        [HttpDelete("delete-report-type/{reportSectionId}")]
        [OpenApiOperation("Delete a report type.", "Deletes a specific Report type")]
        public async Task<IActionResult> DeleteReportType([FromRoute] Guid reportSectionId)
        {
            if (reportSectionId == Guid.Empty)
                return BadRequest(new { message = "ID cannot be empty" });

            var result = await _reportTypeService.DeleteReportType(reportSectionId);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
