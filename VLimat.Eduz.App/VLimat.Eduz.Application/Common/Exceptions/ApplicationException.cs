using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VLimat.Eduz.Application.Common.Exceptions
{
    // VLimat.Eduz.Application/Common/Exceptions/ApplicationExceptionL.cs
  
        public abstract class ApplicationExceptionL : Exception
        {
            public string ErrorCode { get; }
            public string Layer { get; } = "Application";

            protected ApplicationExceptionL(string message, string errorCode) : base(message)
            {
                ErrorCode = errorCode;
            }

            protected ApplicationExceptionL(string message, string errorCode, Exception innerException)
                : base(message, innerException)
            {
                ErrorCode = errorCode;
            }
        }

        // Specific Application Exceptions
        public class CommandValidationException : ApplicationExceptionL
        {
            public IDictionary<string, string[]> Errors { get; }

            public CommandValidationException(IDictionary<string, string[]> errors)
                : base("One or more validation errors occurred", "APP_001")
            {
                Errors = errors;
            }
        }

        public class QueryHandlerException : ApplicationExceptionL
        {
            public QueryHandlerException(string queryName, Exception innerException)
                : base($"Error handling query '{queryName}'", "APP_002", innerException)
            {
            }
        }

        public class CommandHandlerException : ApplicationExceptionL
        {
            public CommandHandlerException(string commandName, Exception innerException)
                : base($"Error handling command '{commandName}'", "APP_003", innerException)
            {
            }
        }

        public class BusinessRuleViolationException : ApplicationExceptionL
        {
            public BusinessRuleViolationException(string rule, string details)
                : base($"Business rule violation: {rule}. {details}", "APP_004")
            {
            }
        }
    
}
