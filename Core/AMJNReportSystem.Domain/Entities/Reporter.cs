using AMJNReportSystem.Domain.Common.Contracts;
using System.Xml;

namespace AMJNReportSystem.Domain.Entities
{
    public class Reporter : AuditableEntity
    {
        //(This Id here is not auto_generated but it is representing a unique Id of a specific reporter i.e muqam, dila, zone, qaid )
        public Guid ReportTypeId { get; set; }
    }
}
