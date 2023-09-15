using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Menominee.Api.Common
{
    public class DebugMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<DebugMiddleware> logger;

        public DebugMiddleware(RequestDelegate next, ILogger<DebugMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            logger.LogInformation($"Method: {context.Request.Method}, Path: {context.Request.Path}, QueryString: {context.Request.QueryString}");

            await next(context);

            logger.LogInformation($"Response Status Code: {context.Response.StatusCode}");
        }
    }
}
