using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ContactManagement.Exceptions
{
    public static class ExceptionStatusCodes
    {
        private static Dictionary<Type, HttpStatusCode> exceptionStatusCodes = new Dictionary<Type, HttpStatusCode>
        {
            {typeof(CompanyArgumentException),HttpStatusCode.BadRequest },
            {typeof(CompanyNotFoundException),HttpStatusCode.NotFound },
            {typeof(ContactArgumentException),HttpStatusCode.BadRequest },
            {typeof(ContactNotFoundException),HttpStatusCode.NotFound }
        };
        public static HttpStatusCode GetExceptionStatusCode(Exception exception)
        {
            bool exceptionFound = exceptionStatusCodes.TryGetValue(exception.GetType(), out var statusCode);
            return exceptionFound ? statusCode : HttpStatusCode.InternalServerError;
        }
    }
}
