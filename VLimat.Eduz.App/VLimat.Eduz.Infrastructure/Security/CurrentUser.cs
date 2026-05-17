using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using VLimat.Eduz.Domain.Security;

namespace VLimat.Eduz.Infrastructure.Security
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        // Backing fields allow manual Set(...) usage
        private int? _userId;
        private string? _userName;
        private int? _academicId;
        private string? _academicName;
        private int? _fyId;
        private string? _fy;

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            PopulateFromHttpContext();
        }

        public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true
                                       || _userId.HasValue;

        public int? UserId => GetClaimInt(ClaimTypes.NameIdentifier) ?? _userId ?? GetHeaderInt("UserId");
        public string UserName => GetClaim(ClaimTypes.Name) ?? _userName ?? GetHeader("UserName") ?? string.Empty;

        public int? AcademicId => GetClaimInt("academicId") ?? _academicId ?? GetHeaderInt("AcademicId");
        public string AcademicName => GetClaim("academicName") ?? _academicName ?? GetHeader("AcademicName") ?? string.Empty;

        public int? FyId => GetClaimInt("fyId") ?? _fyId ?? GetHeaderInt("FyId");
        public string Fy => GetClaim("fy") ?? _fy ?? GetHeader("Fy") ?? string.Empty;

        public void Set(int? userId = null, string? userName = null, int? academicId = null, string? academicName = null, int? fyId = null, string? fy = null)
        {
            if (userId.HasValue) _userId = userId;
            if (!string.IsNullOrEmpty(userName)) _userName = userName;
            if (academicId.HasValue) _academicId = academicId;
            if (!string.IsNullOrEmpty(academicName)) _academicName = academicName;
            if (fyId.HasValue) _fyId = fyId;
            if (!string.IsNullOrEmpty(fy)) _fy = fy;
        }

        private void PopulateFromHttpContext()
        {
            // no-op if HttpContext is not present
            var ctx = _httpContextAccessor.HttpContext;
            if (ctx == null) return;

            // prefer claims mapped by middleware; headers fallback handled by property getters
        }

        private string? GetClaim(string type) => _httpContextAccessor.HttpContext?.User?.FindFirst(type)?.Value;

        private int? GetClaimInt(string type)
        {
            var v = GetClaim(type);
            return int.TryParse(v, out var r) ? r : null;
        }

        private string? GetHeader(string key)
        {
            var values = _httpContextAccessor.HttpContext?.Request?.Headers[key];
            if (values.HasValue)
            {
                // StringValues can be indexed directly to get the first value, or use ToString() to get the first or concatenated value
                return values.Value.Count > 0 ? values.Value[0] : null;
            }
            return null;
        }

        private int? GetHeaderInt(string key)
        {
            var v = GetHeader(key);
            return int.TryParse(v, out var r) ? r : null;
        }
    }
}
