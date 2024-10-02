namespace AMJNReportSystem.Application.Identity.Tokens
{
    public record TokenResponse(string Token, string RefreshToken, DateTime RefreshTokenExpiryTime,Data data);

    public class MemberApiLoginResponse
    {
        public Data Data { get; set; }
        public string Token { get; set; }
        public DateTime Expiry { get; set; }
        public string Message { get; set; }
        public bool Status { get; set; }
    }

    public class Data
    {
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
        public string CircuitName { get; set; }
        public string JamaatName { get; set; }
        public string MemberName { get; set; }
        public int CircuitId { get; set; }
        public string Email { get; set; }
        public int JamaatId { get; set; }
        public string PhoneNo { get; set; }
    }
}