using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VLimat.Eduz.Infrastructure.Common.Exceptions
{
    // VLimat.Eduz.Infrastructure/Common/Exceptions/InfrastructureException.cs
 
        public abstract class InfrastructureException : Exception
        {
            public string ErrorCode { get; }
            public string Layer { get; } = "Infrastructure";

            protected InfrastructureException(string message, string errorCode) : base(message)
            {
                ErrorCode = errorCode;
            }

            protected InfrastructureException(string message, string errorCode, Exception innerException)
                : base(message, innerException)
            {
                ErrorCode = errorCode;
            }
        }

        // Specific Infrastructure Exceptions
        public class DatabaseConnectionException : InfrastructureException
        {
            public DatabaseConnectionException(string databaseName, Exception innerException)
                : base($"Cannot connect to database '{databaseName}'", "INFRA_001", innerException)
            {
            }
        }

        public class RepositoryException : InfrastructureException
        {
            public RepositoryException(string entityName, string operation, Exception innerException)
                : base($"Error performing {operation} on {entityName}", "INFRA_002", innerException)
            {
            }
        }

        public class DataConstraintViolationException : InfrastructureException
        {
            public DataConstraintViolationException(string constraintName, Exception innerException)
                : base($"Database constraint violation: {constraintName}", "INFRA_003", innerException)
            {
            }
        }

        public class ExternalServiceException : InfrastructureException
        {
            public string ServiceName { get; }
            public int StatusCode { get; }

            public ExternalServiceException(string serviceName, string message, int statusCode)
                : base($"External service '{serviceName}' error: {message}", "INFRA_004")
            {
                ServiceName = serviceName;
                StatusCode = statusCode;
            }
        }
   
}
