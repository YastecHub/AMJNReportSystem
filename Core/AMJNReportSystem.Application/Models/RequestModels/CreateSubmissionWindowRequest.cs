namespace AMJNReportSystem.Application.Models.RequestModels
{
	public class CreateSubmissionWindowRequest
    {
        public Guid ReportTypeId { get; set; } 
        public DateTime StartingDate { get; set; }
        public DateTime EndingDate { get; set; }
		public int Month { get; set; } 
		public int Year { get; set; }
		public bool IsLocked { get; set; }
	}
}
