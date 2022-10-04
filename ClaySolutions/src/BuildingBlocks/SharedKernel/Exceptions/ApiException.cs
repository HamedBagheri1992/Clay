using System.Globalization;
using System;

namespace SharedKernel.Exceptions
{
    public class ApiException : ApplicationException
    {
        public ApiException() : base() { }

        public ApiException(string message) : base(message) { }

        public ApiException(string message, params object[] args) : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}