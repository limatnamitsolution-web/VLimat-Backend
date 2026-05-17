using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Http;

namespace VVLimat.Eduz.App.Middleware
{
    public class HeaderToClaimsMiddleware
    {
        private readonly RequestDelegate _next;
        public HeaderToClaimsMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            var headers = context.Request.Headers;
            var identity = context.User?.Identity as ClaimsIdentity ?? new ClaimsIdentity();

            void TryAddClaim(string headerName, string claimType)
            {
                if (headers.TryGetValue(headerName, out StringValues values) && !StringValues.IsNullOrEmpty(values))
                {
                    var value = values.First();
                    if (!string.IsNullOrWhiteSpace(value) && !identity.HasClaim(c => c.Type == claimType))
                    {
                        identity.AddClaim(new Claim(claimType, value));
                    }
                }
            }

            // map headers to claims (use standard types where possible)
            TryAddClaim("UserId", ClaimTypes.NameIdentifier);
            TryAddClaim("UserName", ClaimTypes.Name);
            TryAddClaim("AcademicId", "academicId");
            TryAddClaim("AcademicName", "academicName");
            TryAddClaim("FyId", "fyId");
            TryAddClaim("Fy", "fy");

            // ensure HttpContext.User contains the identity with claims
            if (context.User == null || context.User.Identity == null || context.User.Identity != identity)
            {
                context.User = new ClaimsPrincipal(identity);
            }

            await _next(context);
        }
    }
}