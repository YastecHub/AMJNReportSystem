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
    }
}
