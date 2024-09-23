namespace AMJNReportSystem.Application.Identity.Tokens
{
    public record TokenResponse(string Token, string RefreshToken, DateTime RefreshTokenExpiryTime);

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
        public int UserId { get; set; }
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
    }
}