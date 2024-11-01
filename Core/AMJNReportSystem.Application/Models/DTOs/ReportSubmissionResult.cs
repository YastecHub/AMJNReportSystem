using AMJNReportSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMJNReportSystem.Application.Models.DTOs
{
    public class ReportSubmissionResult
    {
        public List<ReportSubmission> Submissions { get; set; }
        public int Count => Submissions?.Count ?? 0;
    }

}
