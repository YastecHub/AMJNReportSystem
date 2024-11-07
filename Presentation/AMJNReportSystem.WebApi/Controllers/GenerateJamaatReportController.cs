using AMJNReportSystem.Application.Abstractions.Services;
using AMJNReportSystem.Application.Wrapper;
using AMJNReportSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace AMJNReportSystem.WebApi.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class JamaatReportController : ControllerBase
    {
        private readonly IGenerateReportService _generateReportService;

        public JamaatReportController(IGenerateReportService generateReportService)
        {
            _generateReportService = generateReportService;
        }

        
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status500InternalServerError)]
        [HttpGet("generate/{jamaatSubmissionId}")]
        public async Task<IActionResult> GenerateJamaatReport(Guid jamaatSubmissionId)
        {

            var response = await _generateReportService.GenerateJamaatReportSubmissionsAsync(jamaatSubmissionId);

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


       
        [ProducesResponseType(typeof(BaseResponse<PdfResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<PdfResponse>), StatusCodes.Status500InternalServerError)]
        [HttpGet("generate-report/{jamaatSubmissionId}")]
        [OpenApiOperation("create-question", "")]
        public async Task<IActionResult> GenerateJamaatReportSubmission(Guid jamaatSubmissionId)
        {

            var response = await _generateReportService.ReportSubmissionsAsync(jamaatSubmissionId);

            if (!response.Status)
            {
                return StatusCode(500, response.Message);
            }

            return Ok(response);
        }
    }
}