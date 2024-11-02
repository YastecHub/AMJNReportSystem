using AMJNReportSystem.Application.Abstractions.Repositories;
using AMJNReportSystem.Application.Abstractions.Services;
using AMJNReportSystem.Application.Wrapper;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.Extensions.Logging;

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

        //public async Task<BaseResponse<string>> GenerateJamaatReportSubmissionsAsync(int jamaatId, int month)
        //{
        //    try
        //    {
        //        _logger.LogInformation("GenerateJamaatReportSubmissionsAsync called for JamaatId: {jamaatId}", jamaatId);

        //        // Fetch report submissions from the repository
        //        var reportSubmissions = await _reportSubmissionRepository.GetJamaatMonthlyReport(jamaatId, month);

        //        if (reportSubmissions == null || !reportSubmissions.Any())
        //        {
        //            return new BaseResponse<string>
        //            {
        //                Status = false,
        //                Message = "No report submissions found for the given jamaat for the month."
        //            };
        //        }

        //        // Define the output directory and file path
        //        string reportsDirectory = System.IO.Path.Combine("Reports");
        //        string outputPath = System.IO.Path.Combine(reportsDirectory, $"JamaatReport_{jamaatId}_{month}.pdf");

        //        // Ensure the Reports directory exists
        //        if (!Directory.Exists(reportsDirectory))
        //        {
        //            Directory.CreateDirectory(reportsDirectory);
        //        }

        //        // Generate PDF report
        //        using (var stream = new FileStream(outputPath, FileMode.Create))
        //        using (var writer = new PdfWriter(stream))
        //        using (var pdf = new PdfDocument(writer))
        //        {
        //            pdf.SetDefaultPageSize(PageSize.A4); // Set page size here
        //            var document = new Document(pdf);
        //            document.SetMargins(20, 20, 20, 20); // Set margins

        //            foreach (var report in reportSubmissions)
        //            {
        //                // Add Header with dynamic ReportTypeName
        //                document.Add(new Paragraph(report.SubmissionWindow.ReportType.Name)
        //                    .SetFontSize(18)
        //                    .SetBold()
        //                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

        //                document.Add(new Paragraph($"Circuit Monthly Report for Jamaat ID: {report.JamaatId}")
        //                    .SetFontSize(12)
        //                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
        //                document.Add(new Paragraph($"Month: {report.SubmissionWindow.Month}, Year: {report.SubmissionWindow.Year}")
        //                    .SetFontSize(12)
        //                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

        //                document.Add(new Paragraph($"Email Address: {report.JammatEmailAddress}")
        //                    .SetFontSize(10)
        //                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT));
        //                document.Add(new Paragraph(" "));

        //                // Add each section with question and answer structure
        //                foreach (var answer in report.Answers)
        //                {
        //                    // Format question and answer
        //                    document.Add(new Paragraph($"Question ID: {answer.QuestionId}")
        //                        .SetFontSize(10)
        //                        .SetBold());
        //                    document.Add(new Paragraph($"Answer: {answer.TextAnswer ?? "No Answer"}")
        //                        .SetFontSize(10));
        //                    document.Add(new Paragraph(" "));  // Add spacing between questions
        //                }

        //                // Add a new page for each Jamaat report if necessary
        //                document.Add(new AreaBreak(iText.Layout.Properties.AreaBreakType.NEXT_PAGE));
        //            }

        //            // Add Footer for Signatures
        //            document.Add(new Paragraph("President: ______________        Missionary: ______________")
        //                .SetFontSize(12)
        //                .SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT));
        //            document.Add(new Paragraph("General Secretary: ______________")
        //                .SetFontSize(12)
        //                .SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT));

        //            document.Close();
        //        }

        //        _logger.LogInformation("PDF report generated successfully for JamaatId: {jamaatId}", jamaatId);

        //        return new BaseResponse<string>
        //        {
        //            Status = true,
        //            Message = "Report submissions successfully retrieved and PDF generated.",
        //            Data = outputPath // Return the file path for reference
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"An error occurred while retrieving report submissions for JamaatId: {jamaatId}");
        //        return new BaseResponse<string>
        //        {
        //            Status = false,
        //            Message = $"An error occurred while retrieving the report submissions: {ex.Message}"
        //        };
        //    }
        //}



        public async Task<BaseResponse<string>> GenerateJamaatReportSubmissionsAsync(int jamaatId, int month)
        {
            try
            {
                _logger.LogInformation("GenerateJamaatReportSubmissionsAsync called for JamaatId: {jamaatId}", jamaatId);

                // Fetch report submissions from the repository
                var reportSubmissions = await _reportSubmissionRepository.GetJamaatMonthlyReport(jamaatId, month);

                if (reportSubmissions == null || !reportSubmissions.Any())
                {
                    return new BaseResponse<string>
                    {
                        Status = false,
                        Message = "No report submissions found for the given jamaat for the month."
                    };
                }

                // Define the output directory and file path
                string reportsDirectory = System.IO.Path.Combine("Reports");
                string outputPath = System.IO.Path.Combine(reportsDirectory, $"JamaatReport_{jamaatId}_{month}.pdf");

                // Ensure the Reports directory exists
                if (!Directory.Exists(reportsDirectory))
                {
                    Directory.CreateDirectory(reportsDirectory);
                }

                // Generate PDF report
                using (var stream = new FileStream(outputPath, FileMode.Create))
                using (var writer = new PdfWriter(stream))
                using (var pdf = new PdfDocument(writer))
                {
                    pdf.SetDefaultPageSize(PageSize.A4); // Set page size here
                    var document = new Document(pdf);
                    document.SetMargins(20, 20, 20, 20); // Set margins

                    foreach (var report in reportSubmissions)
                    {
                        // Add Header with dynamic ReportTypeName
                        document.Add(new Paragraph(report.SubmissionWindow.ReportType.Name)
                            .SetFontSize(18)
                            .SetBold()
                            .SetTextAlignment(TextAlignment.CENTER));

                        document.Add(new Paragraph($"Circuit Monthly Report for Jamaat ID: {report.JamaatId}")
                            .SetFontSize(12)
                            .SetTextAlignment(TextAlignment.CENTER));
                        document.Add(new Paragraph($"Month: {report.SubmissionWindow.Month}, Year: {report.SubmissionWindow.Year}")
                            .SetFontSize(12)
                            .SetTextAlignment(TextAlignment.CENTER));

                        document.Add(new Paragraph($"Email Address: {report.JammatEmailAddress}")
                            .SetFontSize(10)
                            .SetTextAlignment(TextAlignment.LEFT));
                        document.Add(new Paragraph(" "));

                        // Create a table to display questions and answers
                        Table table = new Table(UnitValue.CreatePercentArray(new float[] { 1, 2 })).UseAllAvailableWidth();
                        table.SetMarginBottom(20);
                        table.AddHeaderCell("Question");
                        table.AddHeaderCell("Answer");

                        foreach (var answer in report.Answers)
                        {
                            table.AddCell(answer.Question.QuestionName);
                            table.AddCell(answer.TextAnswer ?? "No Answer");
                        }

                        // Add the table to the document
                        document.Add(table);

                        // Add a new page for each Jamaat report if necessary
                        document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                    }

                    // Add Footer for Signatures
                    document.Add(new Paragraph("President: ______________        Missionary: ______________")
                        .SetFontSize(12)
                        .SetTextAlignment(TextAlignment.LEFT));
                    document.Add(new Paragraph("General Secretary: ______________")
                        .SetFontSize(12)
                        .SetTextAlignment(TextAlignment.LEFT));

                    document.Close();
                }

                _logger.LogInformation("PDF report generated successfully for JamaatId: {jamaatId}", jamaatId);

                return new BaseResponse<string>
                {
                    Status = true,
                    Message = "Report submissions successfully retrieved and PDF generated.",
                    Data = outputPath // Return the file path for reference
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving report submissions for JamaatId: {jamaatId}");
                return new BaseResponse<string>
                {
                    Status = false,
                    Message = $"An error occurred while retrieving the report submissions: {ex.Message}"
                };
            }
        }

    }
}
