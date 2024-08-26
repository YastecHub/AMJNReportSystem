using System;

namespace AMJNReportSystem.Application.Exceptions
{
    public class DataImportException : Exception
    {
        public DataImportException(string message)
            : base(message)
        {
        }

        public DataImportException(string message, Exception exception)
            : base(message, exception)
        {
        }
    }
}
