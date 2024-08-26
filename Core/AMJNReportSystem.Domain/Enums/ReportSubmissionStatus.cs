using System.ComponentModel;

namespace AMJNReportSystem.Domain.Enums
{
    public enum ReportSubmissionStatus
    {
        [Description("In Progress")]
        Inprogress = 1,

        [Description("Pending")]
        Pending,

        [Description("Submitted")]
        Submitted

    }
}
