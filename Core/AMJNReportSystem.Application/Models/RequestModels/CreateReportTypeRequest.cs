using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMJNReportSystem.Application.Models.RequestModels
{
    public class CreateReportTypeRequest
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public ReportTag ReportTag { get; set; }
    }
}
