namespace tourplannerBackend.Exceptions
{
    /// <summary>
    /// Base class for all domain-level application exceptions.
    /// Subclasses carry an HTTP status code so the global handler can map them directly.
    /// Prefer specific subtypes over throwing AppException directly.
    /// </summary>
    public abstract class AppException(int statusCode, string message) : Exception(message)
    {
        public int StatusCode { get; } = statusCode;
    }

    /// <summary>
    /// Thrown when a requested resource cannot be found. Maps to HTTP 404 Not Found.
    /// </summary>
    public sealed class NotFoundException : AppException
    {
        public NotFoundException(string resourceName, object key)
            : base(StatusCodes.Status404NotFound, $"{resourceName} with id '{key}' was not found.") { }

        public NotFoundException(string message)
            : base(StatusCodes.Status404NotFound, message) { }
    }

    /// <summary>
    /// Thrown when a create operation would produce a duplicate. Maps to HTTP 409 Conflict.
    /// </summary>
    public sealed class ConflictException(string message)
        : AppException(StatusCodes.Status409Conflict, message);

    /// <summary>
    /// Thrown when input violates a domain/business rule (not a format issue). Maps to HTTP 422.
    /// Carry an optional Field name for client-side field highlighting.
    /// </summary>
    public sealed class BusinessRuleException : AppException
    {
        public string? Field { get; }

        public BusinessRuleException(string message, string? field = null)
            : base(StatusCodes.Status422UnprocessableEntity, message)
        {
            Field = field;
        }
    }

    /// <summary>
    /// Thrown when a downstream/external service call fails. Maps to HTTP 502 Bad Gateway.
    /// </summary>
    public sealed class ExternalServiceException(string serviceName, string detail)
        : AppException(StatusCodes.Status502BadGateway, $"External service '{serviceName}' failed: {detail}");
}
