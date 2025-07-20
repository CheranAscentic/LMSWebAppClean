using LMSWebAppClean.Domain.Base;

namespace LMSWebAppClean.Application.DTO
{
    public class LoginDTO
    {
        public BaseUser User { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }

        public LoginDTO(BaseUser user, string token, DateTime expiresAt)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
            Token = token ?? throw new ArgumentNullException(nameof(token));
            ExpiresAt = expiresAt;
        }
    }
}