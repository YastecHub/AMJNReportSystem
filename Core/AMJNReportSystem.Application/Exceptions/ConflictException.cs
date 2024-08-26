using System.Net;

namespace AMJNReportSystem.Application.Exceptions
{
    public class ConflictException : CustomException
    {
        public ConflictException(string message)
            : base(message, null, HttpStatusCode.Conflict)
        {
        }
    }
}