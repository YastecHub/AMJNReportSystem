using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMJNReportSystem.Application.Models.RequestModels
{
    public class CreateReportSectionRequest
    {
        public string ReportSectionName { get; set; } = null!;
        public int ReportSectionValue { get; set; }
        public string Description { get; set; } = null!;
        public Guid ReportTypeId { get; set; }
    }
}
