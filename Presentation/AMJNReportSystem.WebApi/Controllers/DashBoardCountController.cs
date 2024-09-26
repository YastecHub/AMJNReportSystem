﻿using AMJNReportSystem.Application.Abstractions.Services;
using AMJNReportSystem.Application.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace AMJNReportSystem.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashBoardCountController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
       
        
        public DashBoardCountController(IDashboardService dashboardService)
        {
          _dashboardService = dashboardService;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("Get dash board count")]
        [OpenApiOperation("Get list of all dash board count", "")]
        public ActionResult<DashboardCountDto> GetDashboardCounts()
        {
            var result = _dashboardService.DashBoardCount();
            return Ok(result);
        }


    }
}
