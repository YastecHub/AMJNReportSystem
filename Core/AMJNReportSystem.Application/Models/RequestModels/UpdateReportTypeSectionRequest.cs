using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMJNReportSystem.Application.Models.RequestModels
{
    public class UpdateReportTypeSectionRequest
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
