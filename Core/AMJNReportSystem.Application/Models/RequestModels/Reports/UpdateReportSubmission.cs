using AMJNReportSystem.Domain.Entities;
using AMJNReportSystem.Domain.Enums;

namespace AMJNReportSystem.Application.Models.RequestModels.Reports
{
    public class UpdateReportSubmission
    {
        public Guid ReportSubmissionId { get; set; } 
        public ReportSubmissionStatus ReportSubmissionStatus { get; set; }
        public string JammatEmailAddress { get; set; }
        public ReportTag ReportTag { get; set; }
        public string LastModifiedBy { get; set; } 
    }
}