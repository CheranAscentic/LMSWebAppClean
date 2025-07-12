using LMSWebAppClean.Domain.Base;

namespace LMSWebAppClean.Application.Interface
{
    public interface ITokenService
    {
        string GenerateJwtToken(BaseUser user);
        DateTime GetTokenExpiration();
    }
}