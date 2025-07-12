using LMSWebAppClean.Application.Interface;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace LMSWebAppClean.Application.Service
{
    public class RequestUserValidationService : IRequestUserValidationService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public RequestUserValidationService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public int? UserId => GetClaimValue<int?>(ClaimTypes.NameIdentifier);
        public int? DomainUserId => GetClaimValue<int?>("DomainUserId");
        public string? Email => GetClaimValue<string>(ClaimTypes.Email);
        public string? UserType => GetClaimValue<string>(ClaimTypes.Role);
        public bool IsAuthenticated => httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        private T? GetClaimValue<T>(string claimType)
        {
            var claimValue = httpContextAccessor.HttpContext?.User?.FindFirst(claimType)?.Value;
            
            if (string.IsNullOrEmpty(claimValue))
                return default(T);

            try
            {
                return (T)Convert.ChangeType(claimValue, typeof(T));
            }
            catch
            {
                return default(T);
            }
        }
    }
}