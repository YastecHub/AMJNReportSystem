using System;

namespace AMJNReportSystem.Application.Exceptions
{
    public class FileOperationException : Exception
    {
        public FileOperationException(string message)
            : base(message)
        {
        }

        public FileOperationException(string message, Exception exception)
            : base(message, exception)
        {
        }
    }
}
