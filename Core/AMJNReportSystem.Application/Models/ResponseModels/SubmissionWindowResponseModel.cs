using AMJNReportSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMJNReportSystem.Application.Models.ResponseModels
{
    public class SubmissionWindowResponseModel
    {
        public Guid Id { get; set; }
        public Guid ReportTypeId { get; set; }
        public string ReportTypeName { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Islocked { get; set; }
        public string Status { get; set; }

    }
}
