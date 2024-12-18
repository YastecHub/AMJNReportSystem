﻿ using AMJNReportSystem.Domain.Common.Contracts;

namespace AMJNReportSystem.Domain.Entities
{
    public class ReportSection : AuditableEntity
    {
        public string ReportSectionName { get; set; }
        public int ReportSectionValue { get; set; }
        public string? Description { get; set; } 
        public Guid ReportTypeId { get; set; } 
        public bool IsActive { get; set; } = true; 
    }
}
