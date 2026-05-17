using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace VLimat.Eduz.Infrastructure.Security
{
    public interface IUserContext
    {
        int? UserId { get; }
        string UserName { get; }
        int? AcademicId { get; }
        string AcademicName { get; }
        int? FyId { get; }
        string Fy { get; }
    }
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserContext(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;

        private HttpContext? HttpContext => _httpContextAccessor.HttpContext;

        public int? UserId => GetClaimInt(ClaimTypes.NameIdentifier) ?? GetIntHeader("UserId");
        public string UserName => GetClaim(ClaimTypes.Name) ?? GetHeader("UserName") ?? string.Empty;
        public int? AcademicId => GetClaimInt("academicId") ?? GetIntHeader("AcademicId");
        public string AcademicName => GetClaim("academicName") ?? GetHeader("AcademicName") ?? string.Empty;
        public int? FyId => GetClaimInt("fyId") ?? GetIntHeader("FyId");
        public string Fy => GetClaim("fy") ?? GetHeader("Fy") ?? string.Empty;

        private string? GetClaim(string type) => HttpContext?.User?.FindFirst(type)?.Value;
        private int? GetClaimInt(string type)
        {
            var v = GetClaim(type);
            return int.TryParse(v, out var r) ? r : null;
        }

        private string? GetHeader(string key) => HttpContext?.Request?.Headers[key].FirstOrDefault();
        private int? GetIntHeader(string key)
        {
            var v = GetHeader(key);
            return int.TryParse(v, out var r) ? r : null;
        }
    }
}