using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VLimat.Eduz.Infrastructure.Common.Exceptions;

namespace VLimat.Eduz.Infrastructure.Common.Behaviors
{


        public class InfrastructureExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
            where TRequest : notnull
        {
            private readonly ILogger<InfrastructureExceptionBehavior<TRequest, TResponse>> _logger;

            public InfrastructureExceptionBehavior(ILogger<InfrastructureExceptionBehavior<TRequest, TResponse>> logger)
            {
                _logger = logger;
            }

            public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
            {
                try
                {
                    return await next();
                }
                catch (InfrastructureException ex)
                {
                    _logger.LogError(ex, "Infrastructure exception occurred in {RequestName}: {ErrorCode} - {Message}",
                        typeof(TRequest).Name, ex.ErrorCode, ex.Message);

                // Convert InfrastructureException to ApplicationException
                //throw new Application.Common.Exceptions.CommandHandlerException(
                //    typeof(TRequest).Name,
                //    ex);
                throw;
                }
                catch (Microsoft.EntityFrameworkCore.DbUpdateException dbEx)
                {
                    _logger.LogError(dbEx, "Database update exception in {RequestName}", typeof(TRequest).Name);

                    // Handle specific database exceptions
                    if (dbEx.InnerException is Microsoft.Data.SqlClient.SqlException sqlEx)
                    {
                        switch (sqlEx.Number)
                        {
                            case 2627: // Unique constraint
                            case 2601: // Unique index
                                throw new DataConstraintViolationException("UNIQUE_CONSTRAINT", dbEx);
                            case 547: // Foreign key constraint
                                throw new DataConstraintViolationException("FOREIGN_KEY_CONSTRAINT", dbEx);
                        }
                    }

                    throw new RepositoryException("Entity", "save", dbEx);
                }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "💥 UNEXPECTED EXCEPTION in domain behavior: {RequestName}",
                    typeof(TRequest).Name);

                throw;
            }
            //catch (System.Data.SqlClient.SqlException sqlEx) // For older SQL Client
            //{
            //    _logger.LogError(sqlEx, "SQL exception in {RequestName}", typeof(TRequest).Name);
            //    throw new DatabaseConnectionException("ApplicationDbContext", sqlEx);
            //}
        }
        }
    
}
