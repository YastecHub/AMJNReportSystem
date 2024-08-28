using AMJNReportSystem.Domain.Enums;

namespace AMJNReportSystem.Application.Models.DTOs
{
    public class ReportQuestionsModel
    {
        public Guid ReportTypeId { get; set; }
        public string ReportTypeName { get; set; } = null!;
        public IList<Sections> Sections { get; set; }
    }

    public class Sections
    {
        public Guid SectionId { get; set; }
        public string Title { get; set; } = null!;
        public IList<ReportSectionQuestion> Questions { get; set; }
    }

    public class ReportSectionQuestion
    {
        public Guid Id { get; set; }
        public string Text { get; set; } = null!;
        public ResponseType ResponseType { get; set; }
        public double Points { get; set; }
    }
}
