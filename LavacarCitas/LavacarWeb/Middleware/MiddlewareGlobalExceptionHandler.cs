using System.Net;
using System.Text.Json;

namespace LavacarWeb.Middleware
{
    public class MiddlewareGlobalExceptionHandler
    {
        private readonly RequestDelegate _next;
        public MiddlewareGlobalExceptionHandler(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                bool isAjax = context.Request.Headers["X-Requested-With"] == "XMLHttpRequest"
                           || context.Request.Headers["Accept"].ToString().Contains("application/json", StringComparison.OrdinalIgnoreCase);

                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                if (isAjax)
                {
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonSerializer.Serialize(new { ok = false, message = ex.Message }));
                }
                else
                {
                    context.Response.ContentType = "text/plain";
                    await context.Response.WriteAsync(ex.Message);
                }
            }
        }
    }
}
