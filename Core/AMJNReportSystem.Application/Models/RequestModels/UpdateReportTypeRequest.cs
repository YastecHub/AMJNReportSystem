﻿using AMJNReportSystem.Domain.Entities;
using AMJNReportSystem.Domain.Enums;

namespace AMJNReportSystem.Application.Models.RequestModels
{
    public class UpdateReportTypeRequest
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public ReportTag ReportTag { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public bool IsActive { get; set; }
    }
}
