using MediatR;
using Microsoft.Extensions.Logging;
using VLimat.Eduz.Domain.Common.Exceptions;
namespace VLimat.Eduz.Domain.Common.Behaviors
{
   
        public class DomainExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
              where TRequest : notnull
        {
            private readonly ILogger<DomainExceptionBehavior<TRequest, TResponse>> _logger;

            public DomainExceptionBehavior(ILogger<DomainExceptionBehavior<TRequest, TResponse>> logger)
            {
                _logger = logger;
            }

            public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
            {
                try
                {
                    return await next();
                }
                catch (DomainException ex)
                {
                    _logger.LogWarning(ex,
                        "🚨 DOMAIN EXCEPTION: {RequestName} | ErrorCode: {ErrorCode} | Message: {Message}",
                        typeof(TRequest).Name, ex.ErrorCode, ex.Message);

                    // Re-throw to let the application layer handle it
                    throw;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                        "💥 UNEXPECTED EXCEPTION in domain behavior: {RequestName}",
                        typeof(TRequest).Name);

                    throw;
                }
            }
        }
    
}
