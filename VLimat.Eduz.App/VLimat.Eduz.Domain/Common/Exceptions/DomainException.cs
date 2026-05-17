using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VLimat.Eduz.Domain.Common.Exceptions
{
    // VLimat.Eduz.Domain/Common/Exceptions/DomainException.cs

        public abstract class DomainException : Exception
        {
            public string ErrorCode { get; }
            public string Layer { get; } = "Domain";

            protected DomainException(string message, string errorCode) : base(message)
            {
                ErrorCode = errorCode;
            }

            protected DomainException(string message, string errorCode, Exception innerException)
                : base(message, innerException)
            {
                ErrorCode = errorCode;
            }
        }

        // Specific Domain Exceptions
        public class MasterConfigNotFoundException : DomainException
        {
            public MasterConfigNotFoundException(int id)
                : base($"Master config with ID {id} was not found", "DOMAIN_001")
            {
            }
        }

        public class DuplicateMasterConfigException : DomainException
        {
            public DuplicateMasterConfigException(string category, string configKey, int academicId)
                : base($"Config with key '{configKey}' already exists in category '{category}' for academic ID {academicId}",
                      "DOMAIN_002")
            {
            }
        }

        public class MasterConfigInactiveException : DomainException
        {
            public MasterConfigInactiveException(int id)
                : base($"Master config with ID {id} is inactive and cannot be modified",
                      "DOMAIN_003")
            {
            }
        }

        public class InvalidDomainOperationException : DomainException
        {
            public InvalidDomainOperationException(string operation, string reason)
                : base($"Operation '{operation}' is invalid: {reason}", "DOMAIN_004")
            {
            }
        }
    
}
