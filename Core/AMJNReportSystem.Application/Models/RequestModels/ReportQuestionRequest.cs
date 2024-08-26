using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMJNReportSystem.Application.Models.RequestModels
{
    public class ReportQuestionRequest
    {
        public string Text { get; set; } = null!;
        public ResponseType ResponseType { get; set; }
        public double Points { get; set; }
        public Guid SectionId { get; set; }
    }
}
