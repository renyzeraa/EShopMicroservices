using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Exceptions.Handler;

public class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger) : IExceptionHandler
{
  public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
  {
    logger.LogError(exception, "Error message: {Message}", exception.Message);

    var statusCode = exception switch
    {
      ValidationException or BadRequestException => StatusCodes.Status400BadRequest,
      NotFoundException => StatusCodes.Status404NotFound,
      _ => StatusCodes.Status500InternalServerError
    };

    var problemDetails = new ProblemDetails
    {
      Title = exception.GetType().Name,
      Detail = exception.Message,
      Status = statusCode,
      Instance = httpContext.Request.Path
    };

    problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;

    var details = exception switch
    {
      BadRequestException badRequest => badRequest.Details,
      InternalServerException internalServer => internalServer.Details,
      _ => null
    };

    if (details is not null)
    {
      problemDetails.Extensions["details"] = details;
    }

    if (exception is ValidationException validationException)
    {
      problemDetails.Extensions["errors"] = validationException.Errors
        .GroupBy(x => x.PropertyName)
        .ToDictionary(x => x.Key, x => x.Select(e => e.ErrorMessage).ToArray());
    }

    httpContext.Response.StatusCode = statusCode;
    await httpContext.Response.WriteAsJsonAsync(problemDetails, options: null, contentType: "application/problem+json", cancellationToken);

    return true;
  }
}
