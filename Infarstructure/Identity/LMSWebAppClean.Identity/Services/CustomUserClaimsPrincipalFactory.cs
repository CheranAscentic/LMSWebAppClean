using LMSWebAppClean.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Numerics;
using System.Security.Claims;

namespace LMSWebAppClean.Identity.Services
{
    public class CustomUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<AppUser, IdentityType>
    {
        public CustomUserClaimsPrincipalFactory(
            UserManager<AppUser> userManager,
            RoleManager<IdentityType> roleManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, roleManager, optionsAccessor)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(AppUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            if (user.DomainUserId.HasValue)
            {
                identity.AddClaim(new Claim("DomainUserId", user.DomainUserId.Value.ToString()));
            }

            identity.AddClaim(new Claim("UserType", user.UserType.ToString()));
            
            /*if (!string.IsNullOrEmpty(user.FirstName))
            {
                identity.AddClaim(new Claim("FirstName", user.FirstName));
            }
            
            if (!string.IsNullOrEmpty(user.LastName))
            {
                identity.AddClaim(new Claim("LastName", user.LastName));
            }*/

            return identity;
        }
    }
}