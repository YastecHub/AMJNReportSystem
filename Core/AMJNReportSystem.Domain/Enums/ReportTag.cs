using System.ComponentModel;

namespace AMJNReportSystem.Domain.Enums
{
    public enum ReportTag
    {
        [Description("Muqam Report Type")]
        MuqamReportType = 1,

        [Description("Dila Report Type")]
        DilaReportType,

        [Description("Zone Report Type")]
        ZoneReportType,

        [Description("Qaid Report Type")]
        QaidReportType
    }
}
