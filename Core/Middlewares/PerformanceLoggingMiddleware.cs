using Microsoft.AspNetCore.Http;
using Serilog;
using System.Diagnostics;

namespace Core.Middlewares;

public class PerformanceLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public PerformanceLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        await _next(context);

        stopwatch.Stop();
        Log.Information(
            "Request {Method} {Path} executed in {ElapsedMilliseconds}ms with status code {StatusCode}",
            context.Request.Method,
            context.Request.Path,
            stopwatch.ElapsedMilliseconds,
            context.Response.StatusCode);
    }
}
