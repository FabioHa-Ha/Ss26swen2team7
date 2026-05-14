using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tourplannerBackend.Exceptions;

namespace tourplannerBackend.Middleware
{
    /// <summary>
    /// App-wide exception handler registered via services.AddExceptionHandler&lt;T&gt;().
    ///
    /// Technique: IExceptionHandler (ASP.NET Core 8+) — runs as middleware before the response
    /// is written. Maps known AppException subtypes to RFC-7807 ProblemDetails responses;
    /// unrecognised exceptions produce a generic 500 and are fully logged.
    ///
    /// Why a global handler?
    ///   Controllers stay free of try/catch boilerplate. Consistent error shape across the whole API.
    ///   Pair with exception filters (DomainExceptionFilter) when a controller needs custom mapping.
    /// </summary>
    public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            var (statusCode, title) = exception switch
            {
                AppException app    => (app.StatusCode, exception.GetType().Name),
                ArgumentException   => (StatusCodes.Status400BadRequest,  "Bad Request"),
                _                   => (StatusCodes.Status500InternalServerError, "Internal Server Error")
            };

            if (statusCode >= 500)
                logger.LogError(exception, "Unhandled exception on {Method} {Path}",
                    httpContext.Request.Method, httpContext.Request.Path);
            else
                logger.LogWarning("[GlobalHandler] {ExType}: {Message}",
                    exception.GetType().Name, exception.Message);

            var problem = new ProblemDetails
            {
                Status   = statusCode,
                Title    = title,
                Detail   = exception.Message,
                Instance = httpContext.Request.Path
            };

            // Surface the optional Field from BusinessRuleException
            if (exception is BusinessRuleException { Field: not null } bre)
                problem.Extensions["field"] = bre.Field;

            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

            // return true → exception is handled, suppress further propagation
            return true;
        }
    }
}
