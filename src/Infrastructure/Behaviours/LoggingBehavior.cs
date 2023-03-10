using System.Diagnostics;
using System.Text.Json;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Behaviours;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : class, IRequest<TResponse>
{
    private readonly ILogger<TRequest> _logger;

    public LoggingBehavior(ILogger<TRequest> logger) => _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = request.GetType().Name;
        var requestGuid = Guid.NewGuid().ToString();
        var requestNameWithGuid = $"{requestName} [{requestGuid}]";
        var stopwatch = Stopwatch.StartNew();
        
        TResponse response = default;

        try
        {
            _logger.LogInformation($"[START] {requestNameWithGuid} {JsonSerializer.Serialize(request)}");
            response = await next();
        }
        catch (NotSupportedException)
        {
            _logger.LogWarning($"[Serialization ERROR] {requestNameWithGuid} Could not serialize the request.");
        }
        finally
        {
            if (IsResult(response))
            {
                var result = (Result<object>)response;
                if (result.IsSuccess)
                    ShowSuccessResponse(result, stopwatch, requestNameWithGuid);
                else if (result.IsFailure)
                    ShowErrorResponse(result, stopwatch, requestNameWithGuid);
            }
            
            stopwatch.Stop();
        }

        return response;
    }

    private static bool IsResult(TResponse? response) => response.GetType().Name.Contains(typeof(Result).Name);

    private void ShowSuccessResponse(Result<object> result, Stopwatch stopwatch, string requestNameWithGuid)
    {
        _logger.LogInformation($"[RESPONSE] [SUCCESS] {requestNameWithGuid}; Result={result.Value};");
        _logger.LogInformation($"[RESPONSE] [TIME] {requestNameWithGuid}; Execution time={stopwatch.ElapsedMilliseconds}ms");
    }

    private void ShowErrorResponse(Result<object> result, Stopwatch stopwatch, string requestNameWithGuid)
    {
        _logger.LogError($"[RESPONSE] [ERROR] {requestNameWithGuid}; Error={result.Error};");
        _logger.LogError($"[RESPONSE] [TIME] {requestNameWithGuid}; Execution time={stopwatch.ElapsedMilliseconds}ms");
    }
}