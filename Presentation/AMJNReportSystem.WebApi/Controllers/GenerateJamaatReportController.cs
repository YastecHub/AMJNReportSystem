using AMJNReportSystem.Application.Abstractions.Services;
using AMJNReportSystem.Application.Wrapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AMJNReportSystem.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JamaatReportController : ControllerBase
    {
        private readonly IGenerateReportService _generateReportService;

        public JamaatReportController(IGenerateReportService generateReportService)
        {
            _generateReportService = generateReportService;
        }

        [HttpGet("generate/{jamaatId}/{month}")]
        public async Task<IActionResult> GenerateJamaatReport(int jamaatId, int month)
        {
            if (jamaatId <= 0 || month < 1 || month > 12)
            {
                return BadRequest("Invalid Jamaat ID or month.");
            }

            BaseResponse<string> response = await _generateReportService.GenerateJamaatReportSubmissionsAsync(jamaatId, month);

            if (!response.Status)
            {
                return StatusCode(500, response.Message);
            }

            // Now return the file as a downloadable file
            var filePath = response.Data;

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("Report not found.");
            }

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            var fileName = Path.GetFileName(filePath);

            return File(fileBytes, "application/pdf", fileName);
        }
    }
}
