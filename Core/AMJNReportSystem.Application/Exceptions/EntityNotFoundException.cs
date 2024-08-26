using System.Net;

namespace AMJNReportSystem.Application.Exceptions
{
    public class EntityNotFoundException : CustomException
    {
        public EntityNotFoundException(string message)
        : base(message, null, HttpStatusCode.NotFound)
        {
        }
    }
}