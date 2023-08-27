using ContactManagement.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactManagement.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly  RequestDelegate next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }
        public async Task InvokeAsync(HttpContext  httpContext)
        {
            try
            {
                await next(httpContext);
    
            }
            catch(Exception ex)
            {
                 httpContext.Response.StatusCode = (int)ExceptionStatusCodes.GetExceptionStatusCode(ex);
                 httpContext.Response.Headers.Add("Message", ex.Message.ToString());
                 
            }
            
        }
    }

    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
