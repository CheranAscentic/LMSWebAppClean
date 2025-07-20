using LMSWebAppClean.Application.Interface;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace LMSWebAppClean.Application.Service
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public int? UserId => this.GetClaimValue<int?>(ClaimTypes.NameIdentifier);

        public int? DomainUserId => this.GetClaimValue<int?>("DomainUserId");

        public string? Email => this.GetClaimValue<string>(ClaimTypes.Email);

        public string? UserType => this.GetClaimValue<string>(ClaimTypes.Role);

        public bool IsAuthenticated => this.httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        private T? GetClaimValue<T>(string claimType)
        {
            try
            {
                var user = httpContextAccessor.HttpContext?.User;
                var claimValue = user?.FindFirst(claimType)?.Value;
                
                if (string.IsNullOrEmpty(claimValue))
                {
                    return default(T);
                }

                // Handle nullable types properly
                Type targetType = typeof(T);
                Type? underlyingType = Nullable.GetUnderlyingType(targetType);
                
                object convertedValue;
                if (underlyingType != null)
                {
                    // It's a nullable type, convert to the underlying type
                    convertedValue = Convert.ChangeType(claimValue, underlyingType);
                }
                else
                {
                    // It's not a nullable type, convert directly
                    convertedValue = Convert.ChangeType(claimValue, targetType);
                }
                
                return (T)convertedValue;
            }
            catch (Exception)
            {
                return default(T);
            }
        }
    }
}