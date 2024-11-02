using AMJNReportSystem.Application.Abstractions.Repositories;
using AMJNReportSystem.Application.Abstractions.Services;
using AMJNReportSystem.Application.Wrapper;
using AMJNReportSystem.Domain.Entities;
using Microsoft.Extensions.Logging;
using iTextSharp.text;
using iTextSharp.text.pdf;
using AMJNReportSystem.Application.Models.DTOs;

namespace AMJNReportSystem.Application.Services
{
    public class GenerateJamaatReportService : IGenerateReportService
    {
        private readonly IReportSubmissionRepository _reportSubmissionRepository;
        private readonly ILogger<GenerateJamaatReportService> _logger;

        public GenerateJamaatReportService(IReportSubmissionRepository reportSubmissionRepository, ILogger<GenerateJamaatReportService> logger)
        {
            _reportSubmissionRepository = reportSubmissionRepository;
            _logger = logger;
        }


        public async Task<BaseResponse<string>> GenerateJamaatReportSubmissionsAsync(Guid jamaatSubmissionId)
        {
            try
            {
                _logger.LogInformation("GenerateJamaatReportSubmissionsAsync called for JamaatId: {jamaatId}", jamaatSubmissionId);

                // Fetch report submissions from the repository
                var reportSubmissions = await _reportSubmissionRepository.GetReportSubmission(jamaatSubmissionId);

                if (reportSubmissions == null)
                {
                    return new BaseResponse<string>
                    {
                        Status = false,
                        Message = "No report submissions found for the given jamaat for the month."
                    };
                }

               

                // Ensure the Reports directory exists
                string projectFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "GeneratedReports");

                if (!Directory.Exists(projectFolderPath))
                {
                    Directory.CreateDirectory(projectFolderPath);
                }


                string filePath = Path.Combine(projectFolderPath, $"CircuitMonthlyReportForm_{reportSubmissions.Jamaat}_{reportSubmissions.Month}.pdf");


                 CreateMonthlyReportForm(filePath, reportSubmissions);


                _logger.LogInformation("PDF report generated successfully for JamaatId: {jamaatId}", jamaatSubmissionId);

                return new BaseResponse<string>
                {
                    Status = true,
                    Message = "Report submissions successfully retrieved and PDF generated.",
                    Data = filePath // Return the file path for reference
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving report submissions for JamaatId: {jamaatSubmissionId}");
                return new BaseResponse<string>
                {
                    Status = false,
                    Message = $"An error occurred while retrieving the report submissions: {ex.Message}"
                };
            }
        }



        private void CreateMonthlyReportForm(string filePath, PdfResponse sections)
        {
            Document document = new Document(PageSize.A4);
            PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
            document.Open();

            // Add title
            Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
            Font sectionTitleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
            Font normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);



            Paragraph title = new Paragraph($"{sections.ReportTypeName}\n{sections.ReportTypeDescription}\n\n", titleFont);
            title.Alignment = Element.ALIGN_CENTER;
            document.Add(title);

            // Add circuit and date information (static text, can be modified to accept dynamic inputs)
            Paragraph circuitInfo = new Paragraph($"Circuit: {sections.Circuit} Month : {sections.Month} Year: {sections.Year}\nEmail Address: {sections.EmailAddress}\n\n", normalFont);
            circuitInfo.Alignment = Element.ALIGN_CENTER;
            document.Add(circuitInfo);

            // Iterate over sections and add them to the document
            foreach (var section in sections.PdfReportSections)
            {
                // Add section title
                Paragraph sectionTitle = new Paragraph(section.SectionName, sectionTitleFont);
                document.Add(sectionTitle);

                // Create a table for each section
                PdfPTable table = new PdfPTable(2);
                table.WidthPercentage = 100;
                table.SpacingBefore = 10;
                table.SpacingAfter = 10;

                // Set the relative widths of columns (e.g., 1 for S/N. and 5 for ITEMS)
                float[] columnWidths = { 1f, 5f };
                table.SetWidths(columnWidths);

                // Table headers
                table.AddCell(new PdfPCell(new Phrase("S/N.", sectionTitleFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("ITEMS", sectionTitleFont)) { HorizontalAlignment = Element.ALIGN_CENTER });

                // Populate table with questions and answers
                int serialNumber = 1;
                foreach (var qa in section.Questions)
                {
                    table.AddCell(new PdfPCell(new Phrase(serialNumber.ToString(), normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell(new Phrase(qa.Question + "\n" + "Answer: " + (qa.Answer ?? ""), normalFont)));
                    serialNumber++;
                }

                document.Add(table);
            }

            // Close the document
            document.Close();
        }
    }

}
