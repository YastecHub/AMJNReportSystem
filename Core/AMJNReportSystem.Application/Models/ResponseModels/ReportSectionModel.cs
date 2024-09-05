using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMJNReportSystem.Application.Models.ResponseModels
{
    public class ReportSectionModel
    {
    }

    public class CreateReportSectionResponse
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
    }
}
