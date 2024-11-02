namespace AMJNReportSystem.Domain.Entities
{
    public class PdfResponse
    {
        public string? ReportTypeDescription { get; set; }
        public string? ReportTypeName { get; set; }
        public string? EmailAddress { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }
        public string? Jamaat { get; set; }
        public string? Circuit { get; set; }
        public List<PdfReportSection> PdfReportSections { get; set; }
    }
}
