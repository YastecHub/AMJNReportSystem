using System.Net;

namespace AMJNReportSystem.Application.Exceptions
{
    public class ForbiddenException : CustomException
    {
        public ForbiddenException(string message)
            : base(message, null, HttpStatusCode.Forbidden)
        {
        }
    }
}