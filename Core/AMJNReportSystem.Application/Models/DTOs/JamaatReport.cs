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


    public class AmjnReportByRole
    {
        public string TableTitle { get; set; }

        public List<JamaatReportByRole>? JamaatReportByRoles { get; set; }
    }
    public class JamaatReportByRole
    {
        public Guid? ReportSumbmissionId { get; set; }
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool HasSubmitted { get; set; }
        public Guid SubmissionWindowId { get; set; }
    }

}
