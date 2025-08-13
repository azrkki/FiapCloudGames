using System.Diagnostics;
using System.Text;

namespace FCG.Api.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var requestId = Guid.NewGuid().ToString("N")[..8];
            
            // Log request
            await LogRequestAsync(context, requestId);

            // Capture response
            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[{RequestId}] Unhandled exception occurred during request processing", requestId);
                throw;
            }
            finally
            {
                stopwatch.Stop();
                
                // Log response
                await LogResponseAsync(context, requestId, stopwatch.ElapsedMilliseconds);
                
                // Copy response back to original stream
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private async Task LogRequestAsync(HttpContext context, string requestId)
        {
            var request = context.Request;
            var requestBody = string.Empty;

            // Read request body if it exists
            if (request.ContentLength > 0 && request.ContentType?.Contains("application/json") == true)
            {
                request.EnableBuffering();
                using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
                requestBody = await reader.ReadToEndAsync();
                request.Body.Position = 0;
            }

            _logger.LogInformation(
                "[{RequestId}] HTTP {Method} {Path} {QueryString} - Content-Type: {ContentType} - Body: {RequestBody}",
                requestId,
                request.Method,
                request.Path,
                request.QueryString,
                request.ContentType ?? "N/A",
                string.IsNullOrEmpty(requestBody) ? "N/A" : requestBody
            );
        }

        private async Task LogResponseAsync(HttpContext context, string requestId, long elapsedMs)
        {
            var response = context.Response;
            var responseBody = string.Empty;

            // Read response body if it's JSON
            if (response.ContentType?.Contains("application/json") == true && response.Body.CanSeek)
            {
                response.Body.Seek(0, SeekOrigin.Begin);
                using var reader = new StreamReader(response.Body, Encoding.UTF8, leaveOpen: true);
                responseBody = await reader.ReadToEndAsync();
                response.Body.Seek(0, SeekOrigin.Begin);
            }

            var logLevel = response.StatusCode >= 400 ? LogLevel.Warning : LogLevel.Information;
            
            _logger.Log(logLevel,
                "[{RequestId}] HTTP {StatusCode} - {ElapsedMs}ms - Content-Type: {ContentType} - Body: {ResponseBody}",
                requestId,
                response.StatusCode,
                elapsedMs,
                response.ContentType ?? "N/A",
                string.IsNullOrEmpty(responseBody) ? "N/A" : responseBody
            );
        }
    }

    public static class LoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggingMiddleware>();
        }
    }
}