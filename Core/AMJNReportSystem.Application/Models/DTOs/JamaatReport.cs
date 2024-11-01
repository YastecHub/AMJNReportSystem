namespace AMJNReportSystem.Application.Models.DTOs
{
    public class JamaatReport
    {
        public Guid Id { get; set; }
        public string? JamaatName { get; set; }
        public int JamaatId { get; set; }
        public Guid SubmissionWindowId { get; set; } 
        public string? SubmissionWindowName { get; set; } 
    }
}
