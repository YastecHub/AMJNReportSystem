using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMJNReportSystem.Application.Models.DTOs
{
    public class SubmissionWindowDto
    {
        public Guid SubmissionWindowId { get; set; }
        public string Name { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public Guid ReportTypeId { get; set; }
        public Guid ReportTypeName { get; set; }
        public bool IsLocked { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
