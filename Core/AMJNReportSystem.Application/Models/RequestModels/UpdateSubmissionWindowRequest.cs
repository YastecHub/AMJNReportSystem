using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMJNReportSystem.Application.Models.RequestModels
{
    public class UpdateSubmissionWindowRequest
    {
        public DateTime StartingDate { get; set; }
        public DateTime EndingDate { get; set; }
    }
}
