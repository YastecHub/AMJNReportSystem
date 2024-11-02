namespace AMJNReportSystem.Domain.Entities
{
    public class PdfReportSection
    {
        public Guid SectionId { get; set; }
        public string SectionName { get; set; }
        public List<PdfQuestionAnswer> Questions { get; set; }

    }
}
