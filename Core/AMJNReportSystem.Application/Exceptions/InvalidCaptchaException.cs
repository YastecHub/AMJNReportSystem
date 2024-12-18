﻿using System;

namespace AMJNReportSystem.Application.Exceptions
{
    public class InvalidCaptchaException : ApplicationException
    {
        public InvalidCaptchaException(string message)
            : base(message)
        {
        }

        public InvalidCaptchaException(string message, Exception exception)
            : base(message, exception)
        {
        }
    }
}
