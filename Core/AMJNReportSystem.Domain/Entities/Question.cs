using AMJNReportSystem.Domain.Common.Contracts;
using AMJNReportSystem.Domain.Enums;

namespace AMJNReportSystem.Domain.Entities
{
    public class Question : AuditableEntity
    {

        public Guid SectionId { get; set; }
        public ReportTypeSection Section { get; set; }
        public string Text { get; set; }
        public ResponseType ResponseType { get; set; }
        public double Points { get; set; }
        public bool isActive { get; set; }
    }
}