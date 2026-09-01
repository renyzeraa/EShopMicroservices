using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
  : IPipelineBehavior<TRequest, TResponse>
  where TRequest : notnull, IRequest<TResponse>
  where TResponse : notnull
{
  public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
  {
    logger.LogInformation("[START] Handling request={Request} - RequestData={@RequestData}",
      typeof(TRequest).Name, request);

    var timer = Stopwatch.StartNew();

    var response = await next(cancellationToken);

    timer.Stop();

    if (timer.Elapsed.TotalSeconds > 3)
    {
      logger.LogWarning("[PERFORMANCE] The request {Request} took {TimeTaken} seconds.",
        typeof(TRequest).Name, timer.Elapsed.TotalSeconds);
    }

    logger.LogInformation("[END] Handled request={Request} with response={Response} in {ElapsedMilliseconds}ms",
      typeof(TRequest).Name, typeof(TResponse).Name, timer.ElapsedMilliseconds);

    return response;
  }
}
