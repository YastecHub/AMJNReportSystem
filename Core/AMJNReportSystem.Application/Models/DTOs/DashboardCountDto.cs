﻿namespace AMJNReportSystem.Application.Models.DTOs
{
    public class DashboardCountDto
    {
        public int? UserCounts { get; set; }
        public int? ReportTypeCounts { get; set; }
        public int? ReportSectionCounts { get; set; }  
        public int? QuestionCounts { get; set; }
        public int? ReportSubmittedByCircuitCounts { get; set; } 
        public int? ReportSubmittedByJamaatCounts { get; set; } 
        public int TotalReportSubmittedForTheWholeMonth { get; set; }
    }
}
