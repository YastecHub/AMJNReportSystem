namespace AMJNReportSystem.Domain.Entities
{
    public class Muqam
    {
        public int JamaatId { get; set; }
        public string JamaatName { get; set; }
        public string? JamaatCode { get; set; } 
        public int CircuitId { get; set; }
        public string? CircuitCode { get; set; }
        public string CircuitName { get; set; }
    }
}
