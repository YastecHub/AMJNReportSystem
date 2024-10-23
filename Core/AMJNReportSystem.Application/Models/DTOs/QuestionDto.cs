using AMJNReportSystem.Domain.Entities;
using AMJNReportSystem.Domain.Enums;

namespace AMJNReportSystem.Application.Models.DTOs
{
	public class QuestionDto
	{
		public Guid Id { get; set; }
		public bool IsDeleted { get; set; }
		public Guid ReportSectionId { get; set; }
        public string SectionName { get; set; } 
        public string QuestionName { get; set; }
		public bool IsRequired { get; set; }
		public bool IsActive { get; set; }
		public QuestionType QuestionType { get; set; }
		public ResponseType ResponseType { get; set; }
		public List<QuestionOption>? Options { get; set; } = new List<QuestionOption>();
	}

    public class ReportTypeQuestionDto
    {
        public Guid ReportTypeId { get; set; }
        public string ReportTypeName { get; set; }
        public List<ReportTypeSectionQuestion>? ReportTypeSectionQuestions { get; set; } = new List<ReportTypeSectionQuestion>();
    }


    public class ReportTypeSectionQuestion
    {
        public Guid SectionId { get; set; }
        public string SectionName { get; set; }

        public List<ReportSectionQuestionDto>? ReportSectionQuestions { get; set; } = new List<ReportSectionQuestionDto>();
    }

    public class ReportTypeSectionQuestionSlim
    {
        public Guid SectionId { get; set; }
        public string SectionName { get; set; }
    }

    public class ReportSectionQuestionDto
    {
        public Guid Id { get; set; }
        public string QuestionText { get; set; }
        public bool IsRequired { get; set; }
        public bool IsActive { get; set; }
        public QuestionType QuestionType { get; set; }
        public ResponseType ResponseType { get; set; }
        public List<ReportQuestionOptionDto>? Options { get; set; } = new List<ReportQuestionOptionDto>();
    }
}
