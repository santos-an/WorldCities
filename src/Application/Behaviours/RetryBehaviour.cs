using Application.Exceptions;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Behaviours;

public class RetryBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : class, IRequest<TResponse>
{
    private const int RetryAttempts = 3;
    private readonly ILogger<TRequest> _logger;
    
    public RetryBehaviour(ILogger<TRequest> logger) => _logger = logger;

    public async Task<TResponse?> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = request.GetType().Name;
        var requestGuid = Guid.NewGuid().ToString();
        var requestNameWithGuid = $"{requestName} [{requestGuid}]";

        return await TryExecute(next, requestNameWithGuid);
    }
    
    private async Task<TResponse?> TryExecute(RequestHandlerDelegate<TResponse> next, string requestName)
    {
        TResponse? response = default;
        
        for (var i = 1; i <= RetryAttempts; i++)
        {
            try
            {
                response = await next();
                break;
            }
            catch (Exception e)
            {
                if (i != RetryAttempts)
                    _logger.LogWarning($"[Retrying the request] {requestName} {i} times");
                else
                    ThrowBadRequest(requestName, e);
            }
        }

        return response;
    }

    private void ThrowBadRequest(string requestName, Exception innerException)
    {
        var errorMessage = $"[FAILED] - [Request] {requestName} [{RetryAttempts}] times";
        _logger.LogError(errorMessage);
        
        throw new BadRequestException("Bad Request", errorMessage, innerException);
    }
}