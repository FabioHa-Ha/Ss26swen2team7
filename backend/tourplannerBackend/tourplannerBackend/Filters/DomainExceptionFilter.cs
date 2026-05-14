using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using tourplannerBackend.Exceptions;

namespace tourplannerBackend.Filters
{
    /// <summary>
    /// MVC Exception Filter applied selectively via [TypeFilter(typeof(DomainExceptionFilter))].
    ///
    /// Technique: IExceptionFilter — runs inside the MVC pipeline, after model binding and
    /// action execution. Only sees exceptions from actions on the decorated controller; unlike
    /// the global IExceptionHandler it does NOT catch middleware or routing errors.
    ///
    /// Use this when a specific controller needs different error mapping than the global default,
    /// e.g. adding controller-specific extension fields or wrapping errors in a custom envelope.
    ///
    /// If this filter handles the exception (ExceptionHandled = true) the GlobalExceptionHandler
    /// is NOT invoked for that request.
    /// </summary>
    public sealed class DomainExceptionFilter(ILogger<DomainExceptionFilter> logger) : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is not AppException appEx)
                return; // not a domain exception — propagate to GlobalExceptionHandler

            logger.LogWarning("[DomainExceptionFilter] {ExType}: {Message}",
                appEx.GetType().Name, appEx.Message);

            var problem = new ProblemDetails
            {
                Status   = appEx.StatusCode,
                Title    = appEx.GetType().Name,
                Detail   = appEx.Message,
                Instance = context.HttpContext.Request.Path
            };

            if (appEx is BusinessRuleException { Field: not null } bre)
                problem.Extensions["field"] = bre.Field;

            // Tag this response so clients can distinguish filter-handled from global-handled
            problem.Extensions["handledBy"] = "DomainExceptionFilter";

            context.Result           = new ObjectResult(problem) { StatusCode = appEx.StatusCode };
            context.ExceptionHandled = true;
        }
    }
}
