using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Serilog.Context;

namespace CacheServices.Services.Helpers
{
    
    public class AddParamsToLogContextMiddleware
    {
        private readonly RequestDelegate _next;

        public AddParamsToLogContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            using (LogContext.PushProperty("RequestId", Guid.NewGuid()))
            using (LogContext.PushProperty("HttpMethod", context.Request.Method))
            using (LogContext.PushProperty("Url", context.Request.GetEncodedUrl()))
            using (LogContext.PushProperty("StatusCode", context.Response.StatusCode))
            using (LogContext.PushProperty("MachineName", Environment.MachineName))
            using (LogContext.PushProperty("AppVersion", typeof(Startup).Assembly.GetName().Version.ToString()))
            {
                return _next(context);
            }
        }
    }
}