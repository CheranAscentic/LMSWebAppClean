namespace LMSWebAppClean.Application.DTO
{
    public class LoginDTO
    {
        public UserDTO User { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }

        public LoginDTO(UserDTO user, string token, DateTime expiresAt)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
            Token = token ?? throw new ArgumentNullException(nameof(token));
            ExpiresAt = expiresAt;
        }
    }
}