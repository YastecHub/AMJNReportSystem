using Application.Abstractions.Services;
using Application.Models;
using Application.Models.RequestModels;
using Application.Models.RequestModels.Reports;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace AMJNReportSystem.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpPost]
        public async Task<IActionResult> SaveReport(ReportRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _reportService.SaveReport(request);
            return Ok(response);
        }
    }
}
