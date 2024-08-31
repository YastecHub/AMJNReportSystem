using AMJNReportSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMJNReportSystem.Application.Models.RequestModels
{
    public class CreateSubmissionWindowRequest
    {
        public Guid ReportSubmissionId { get; set; }
        public DateTime StartingDate { get; set; }
        public DateTime EndingDate { get; set; }
		public int Month { get; set; }
		public int Year { get; set; }
		public bool IsLocked { get; set; }
	}
}
