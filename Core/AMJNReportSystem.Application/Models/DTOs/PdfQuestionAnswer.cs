namespace AMJNReportSystem.Domain.Entities
{
    public class PdfQuestionAnswer
    {
        public string Question { get; set; }
        public string Answer { get; set; }

        public List<string>? OptionsAnswer { get; set; }
    }
}
