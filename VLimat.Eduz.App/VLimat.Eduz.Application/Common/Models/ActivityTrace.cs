using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VLimat.Eduz.Application.Common.Models;

namespace VLimat.Eduz.Application.Common.Models
{
        public class ActivityTrace
        {
            public string TraceId { get; set; } = string.Empty;
            public string SessionId { get; set; } = string.Empty;
            public string UserId { get; set; } = string.Empty;
            public string UserName { get; set; } = string.Empty;
            public string UserRole { get; set; } = string.Empty;
            public string ClientIp { get; set; } = string.Empty;
            public string UserAgent { get; set; } = string.Empty;
            public DateTime StartTime { get; set; }
            public TimeSpan Duration { get; set; }
            public string Layer { get; set; } = string.Empty;
            public string Operation { get; set; } = string.Empty;
            public string RequestType { get; set; } = string.Empty;
            public object RequestData { get; set; } = default!;
            public object ResponseData { get; set; } = default!;
            public bool IsSuccess { get; set; }
            public string ErrorMessage { get; set; } = string.Empty;
            public Dictionary<string, object> AdditionalData { get; set; } = new();
        }

        public interface ITraceableRequest
        {
            string OperationName { get; }
            string UserContext { get; }
            bool LogSensitiveData { get; }
        }
    
}
