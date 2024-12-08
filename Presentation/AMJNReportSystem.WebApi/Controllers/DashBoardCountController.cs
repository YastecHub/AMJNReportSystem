using AMJNReportSystem.Application.Abstractions.Services;
using AMJNReportSystem.Application.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
namespace AMJNReportSystem.WebApi.Controllers
{
    [Authorize]
    [Route("api/v{version:apiVersion}/[controller]")]
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
        [ProducesResponseType(typeof(DashboardCountDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DashboardCountDto), StatusCodes.Status500InternalServerError)]
        [HttpGet("get-dash-board-count")]
        [OpenApiOperation("Get list of all dash board count", "")]
        public async Task<ActionResult<DashboardCountDto>> GetDashboardCountsAsync()
        {
            var result = await _dashboardService.DashBoardCountAsync();
            return Ok(result);
        }
    }
}
