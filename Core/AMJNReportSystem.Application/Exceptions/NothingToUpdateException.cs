using System.Net;

namespace AMJNReportSystem.Application.Exceptions
{
    public class NothingToUpdateException : CustomException
    {
        public NothingToUpdateException()
        : base("There are no new changes to update.", null, HttpStatusCode.NotAcceptable)
        {
        }
    }
}