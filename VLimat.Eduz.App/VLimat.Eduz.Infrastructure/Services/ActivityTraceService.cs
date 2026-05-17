using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using VLimat.Eduz.Domain.Common.Models;

    namespace VLimat.Eduz.Infrastructure.Services
    {
        public interface IActivityTraceService
        {
            ActivityTrace CurrentTrace { get; }
            void StartTrace(HttpContext httpContext, string operation);
            Task CompleteTraceAsync(ActivityTrace trace);
            Task SaveTraceAsync(ActivityTrace trace);
        }

        public class ActivityTraceService : IActivityTraceService
        {
            private readonly AsyncLocal<ActivityTrace> _currentTrace = new AsyncLocal<ActivityTrace>();
            private readonly ILogger<ActivityTraceService> _logger;

            public ActivityTrace CurrentTrace => _currentTrace.Value ?? new ActivityTrace();

            public ActivityTraceService(ILogger<ActivityTraceService> logger)
            {
                _logger = logger;
            }

            public void StartTrace(HttpContext httpContext, string operation)
            {
                var trace = new ActivityTrace
                {
                    TraceId = httpContext.TraceIdentifier,
                    SessionId = httpContext.Session?.Id ?? Guid.NewGuid().ToString(),
                    UserId = GetUserId(httpContext),
                    UserName = GetUserName(httpContext),
                    UserRole = GetUserRole(httpContext),
                    ClientIp = GetClientIp(httpContext),
                    UserAgent = httpContext.Request.Headers["User-Agent"].ToString(),
                    StartTime = DateTime.UtcNow,
                    Layer = "Presentation",
                    Operation = operation,
                    RequestType = httpContext.Request.Method
                };

                _currentTrace.Value = trace;

                _logger.LogInformation(
                    "🎬 TRACE STARTED: {Operation} | TraceId: {TraceId} | User: {UserName} | IP: {ClientIp}",
                    operation, trace.TraceId, trace.UserName, trace.ClientIp);
            }

            public async Task CompleteTraceAsync(ActivityTrace trace)
            {
                trace.Duration = DateTime.UtcNow - trace.StartTime;

                // Save to database, file, or external system
                await SaveTraceAsync(trace);
            }

            public async Task SaveTraceAsync(ActivityTrace trace)
            {
                try
                {
                    // Save to database (you can implement this based on your storage)
                    // await _dbContext.ActivityTraces.AddAsync(trace);
                    // await _dbContext.SaveChangesAsync();

                    // Or log to structured logging system
                    _logger.LogInformation(
                        "💾 TRACE SAVED: {Operation} | Duration: {Duration}ms | Success: {IsSuccess} | User: {UserId}",
                        trace.Operation, trace.Duration.TotalMilliseconds, trace.IsSuccess, trace.UserId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to save activity trace");
                }
            }

            private string GetUserId(HttpContext context)
            {
                return context.User.FindFirst("sub")?.Value
                    ?? context.User.FindFirst("userId")?.Value
                    ?? "Anonymous";
            }

            private string GetUserName(HttpContext context)
            {
                return context.User.FindFirst("name")?.Value
                    ?? context.User.Identity?.Name
                    ?? "Unknown";
            }

            private string GetUserRole(HttpContext context)
            {
                var roles = context.User.FindAll("role").Select(c => c.Value);
                return string.Join(",", roles) ?? "User";
            }

            private string GetClientIp(HttpContext context)
            {
                return context.Connection.RemoteIpAddress?.ToString()
                    ?? context.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                    ?? "Unknown";
            }
        }
    
}
