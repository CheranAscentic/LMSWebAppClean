namespace LMSWebAppClean.Application.Interface
{
    public interface ICurrentUserService
    {
        int? UserId { get; }
        int? DomainUserId { get; }
        string? Email { get; }
        string? UserType { get; }
        bool IsAuthenticated { get; }
    }
}