﻿using AMJNReportSystem.Domain.Entities;

namespace AMJNReportSystem.Application.Models.ResponseModels
{
    public class PdfSubmittedReportDto
    {
        public Guid Id { get; set; }
        public int JamaatId { get; set; }
        public string? JamaatName { get; set; }
        public int CircuitId { get; set; }
        public string? CircuitName { get; set; }
        public string? JammatEmailAddress { get; set; }
        public string? ReportTypeName { get; set; }
        public int SubmissionWindowId { get; set; }
        public List<SubmittedReportResponseDto>? Answers { get; set; } = new List<SubmittedReportResponseDto>();
    }
}
