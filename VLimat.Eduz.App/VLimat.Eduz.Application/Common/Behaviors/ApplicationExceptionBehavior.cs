using MediatR;
using Microsoft.Extensions.Logging;
using VLimat.Eduz.Application.Common.Exceptions;


namespace VLimat.Eduz.Application.Common.Behaviors
{
   
        public class ApplicationExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
            where TRequest : notnull
        {
            private readonly ILogger<ApplicationExceptionBehavior<TRequest, TResponse>> _logger;

            public ApplicationExceptionBehavior(ILogger<ApplicationExceptionBehavior<TRequest, TResponse>> logger)
            {
                _logger = logger;
            }

            public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
            {
                try
                {
                    return await next();
                }
                catch ( ApplicationExceptionL ex)
                {
                    _logger.LogError(ex, "Application exception occurred in {RequestName}: {ErrorCode} - {Message}",
                        typeof(TRequest).Name, ex.ErrorCode, ex.Message);

                    // Application exceptions bubble up to presentation layer
                    throw;
                }
                catch (FluentValidation.ValidationException ex)
                {
                    _logger.LogWarning(ex, "Validation exception occurred in {RequestName}", typeof(TRequest).Name);

                    var errors = ex.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

                    throw new CommandValidationException(errors);
                }
            }
        }
    
}
