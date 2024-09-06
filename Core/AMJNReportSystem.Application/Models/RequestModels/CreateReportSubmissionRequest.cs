using AMJNReportSystem.Domain.Entities;
using AMJNReportSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMJNReportSystem.Application.Models.RequestModels
{
    public class CreateReportSubmissionRequest
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public Guid ReportTypeId { get; set; }
        public Guid JamaatId { get; set; }
        public string JammatEmailAddress { get; set; }
        public ReportSubmissionStatus ReportSubmissionStatus { get; set; }
        public ReportTag ReportTag { get; set; }
        public string CreatedBy { get; set; }
        public Guid SubmissionWindowId { get; set; }
    }
}
