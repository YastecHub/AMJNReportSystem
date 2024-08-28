using AMJNReportSystem.Domain.Enums;

namespace AMJNReportSystem.Application.Models.RequestModels
{
    public class ReportQuestionRequest
    {
        public string Text { get; set; } = null!;
        public ResponseType ResponseType { get; set; }
        public double Points { get; set; }
        public Guid SectionId { get; set; }
    }
}
