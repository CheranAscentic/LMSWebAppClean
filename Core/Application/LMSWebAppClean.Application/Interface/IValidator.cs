using LMSWebAppClean.Application.Interface;

namespace LMSWebAppClean.Application.Interface
{
    public interface IValidator<in T> where T : IRequestData
    {
        Task<ValidationResult> ValidateAsync(T request, CancellationToken cancellationToken = default);
    }

    public class ValidationResult
    {
        public bool IsValid { get; private set; }
        public List<string> Errors { get; private set; } = new();

        public static ValidationResult Success() => new() { IsValid = true };
        
        public static ValidationResult Failure(params string[] errors) => new() 
        { 
            IsValid = false, 
            Errors = errors.ToList() 
        };
        
        public static ValidationResult Failure(IEnumerable<string> errors) => new() 
        { 
            IsValid = false, 
            Errors = errors.ToList() 
        };
    }
}