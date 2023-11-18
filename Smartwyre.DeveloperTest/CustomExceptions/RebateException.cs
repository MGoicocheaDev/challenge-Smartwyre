using System;

namespace Smartwyre.DeveloperTest.CustomExceptions
{
    public class RebateException : Exception
    {
        public RebateException()
        {
        }

        public RebateException(string message) : base(message)
        {
        }

        public RebateException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
