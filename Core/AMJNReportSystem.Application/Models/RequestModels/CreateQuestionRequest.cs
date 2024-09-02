﻿using AMJNReportSystem.Domain.Entities;
using AMJNReportSystem.Domain.Enums;

namespace AMJNReportSystem.Application.Models.RequestModels
{
	public class CreateQuestionRequest
	{
		public Guid Id { get; set; }
		public string? CreatedBy { get; set; }
		public DateTime CreatedOn { get; private set; }
		public DateTime? DeletedOn { get; set; }
		public string? DeletedBy { get; set; }
		public bool IsDeleted { get; set; }
		public Guid ReportSectionId { get; set; }
		public string QuestionName { get; set; }
		public double Points { get; set; }
		public bool IsRequired { get; set; }
		public bool IsActive { get; set; }
		public QuestionType QuestionType { get; set; }
		public ResponseType ResponseType { get; set; }
		public List<QuestionOption>? Options { get; set; } = new List<QuestionOption>();
	}
}