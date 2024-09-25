using AMJNReportSystem.Application.Abstractions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
