using LMSWebAppClean.Application.Interface;

namespace LMSWebAppClean.Application.DTO.Identity
{
    public class UserRegistrationResult : IQuery
    {
        public bool IsSuccess { get; set; }
        public string? IdentityUserId { get; set; }
        public int? DomainUserId { get; set; }
        public List<string> Errors { get; set; } = new();

        public static UserRegistrationResult Success(string identityUserId, int domainUserId)
        {
            return new UserRegistrationResult
            {
                IsSuccess = true,
                IdentityUserId = identityUserId,
                DomainUserId = domainUserId
            };
        }

        public static UserRegistrationResult Failure(IEnumerable<string> errors)
        {
            return new UserRegistrationResult
            {
                IsSuccess = false,
                Errors = errors.ToList()
            };
        }

        public static UserRegistrationResult Failure(string error)
        {
            return new UserRegistrationResult
            {
                IsSuccess = false,
                Errors = new List<string> { error }
            };
        }
    }
}