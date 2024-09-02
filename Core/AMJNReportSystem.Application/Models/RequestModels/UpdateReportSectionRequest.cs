namespace AMJNReportSystem.Application.Models.RequestModels
{
    public class UpdateReportSectionRequest
    {
        public string Name { get; set; } = null!;  
        public int Value { get; set; }  
        public string Description { get; set; } = null!; 
    }
}