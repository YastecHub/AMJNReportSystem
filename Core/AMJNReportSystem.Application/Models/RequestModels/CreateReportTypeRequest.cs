using AMJNReportSystem.Domain.Enums;

namespace AMJNReportSystem.Application.Models.RequestModels
{
    public class CreateReportSubmission
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public ReportTag ReportTag { get; set; }
    }
}
