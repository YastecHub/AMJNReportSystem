using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMJNReportSystem.Application.Models.DTOs
{
    public class DashboardCountDto
    {
        public int? MembersCounts { get; set; } 
        public int? ReportTypeCounts { get; set; }
        public int? ReportSectionCounts { get; set; }  
        public int? QuestionCounts { get; set; }
        public int? ReportSubmittedByCircuitCounts { get; set; } 
        public int? ReportSubmittedByJamaatCounts { get; set; } 
        public int? ReportSubmittedByJamaatCount{ get; set; } 
    }
}
