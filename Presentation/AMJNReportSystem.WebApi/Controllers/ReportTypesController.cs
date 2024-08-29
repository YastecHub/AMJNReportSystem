﻿using AMJNReportSystem.Application.Abstractions.Services;
using AMJNReportSystem.Application.Models.RequestModels;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace AMJNReportSystem.WebApi.Controllers
{
    public class ReportTypesController : BaseSecuredController
    {
        private readonly IReportTypeService _reportTypeService;

        public ReportTypesController(IReportTypeService reportTypeService)
        {
            _reportTypeService = reportTypeService;
        }

        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost]
        [OpenApiOperation("Create new report type.", "")]
        public async Task<IActionResult> CreateReportType([FromBody] CreateReportTypeRequest model)
        {
            var a = UserContext;
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var reportTypeRequest = await _reportTypeService.CreateReportType(model);
            return !reportTypeRequest.Succeeded ? Conflict(reportTypeRequest) : Ok(reportTypeRequest);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("qaidReportTypes")]
        [OpenApiOperation("Get list of Qaid report types.", "")]
        public async Task<IActionResult> GetQaidReportTypes()
        {
            var qaidReportType = await _reportTypeService.GetQaidReportTypes();
            return Ok(qaidReportType);
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        [OpenApiOperation("Get a specific report type by id.", "")]
        public async Task<IActionResult> GetReportType(Guid id)
        {
            if (id == Guid.Empty) return BadRequest("id can not be empty");
            var response = await _reportTypeService.GetReportType(id);
            return !response.Succeeded ? NotFound(response) : Ok(response);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        [OpenApiOperation("Get list of all report types.", "")]
        public async Task<IActionResult> GetReportTypes()
        {
            var response = await _reportTypeService.GetReportTypes();
            return Ok(response);
        }


        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut("{id}")]
        [OpenApiOperation("update a specific report type.", "")]
        public async Task<IActionResult> UpdateReportType(Guid id, [FromBody] UpdateReportTypeRequest request)
        {
            if (id == Guid.Empty) return BadRequest("id can not be empty");
            var response = await _reportTypeService.UpdateReportType(id, request);
            return Ok(response);
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut("{id}/state")]
        [OpenApiOperation("update a Report Type Activeness State", "")]
        public async Task<IActionResult> SetReportTypeState([FromRoute] Guid reportTypeId, bool state)
        {
            if (reportTypeId == Guid.Empty) return BadRequest("id can not be empty");
            var response = await _reportTypeService.SetReportTypeActiveness(reportTypeId, state);
            return Ok(response);
        }

    }
}