using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Application.DTO.Identity;
using LMSWebAppClean.Application.Usecase.Users.CreateUser;
using LMSWebAppClean.Domain.Enum;
using LMSWebAppClean.Identity.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LMSWebAppClean.Identity.Services
{
    public class UserRegistrationService : IUserRegistrationService
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IMediator mediator;

        public UserRegistrationService(UserManager<AppUser> userManager, IMediator mediator)
        {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<UserRegistrationResult> RegisterUserAsync(
            string name,
            string email,
            string password,
            string userType,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // Validate input
                var validationErrors = ValidateInput(name, email, password);
                if (validationErrors.Any())
                {
                    return UserRegistrationResult.Failure(validationErrors);
                }

                // Check if user already exists
                if (await UserExistsAsync(email))
                {
                    return UserRegistrationResult.Failure("User with this email already exists.");
                }

                // Create Identity user
                var appUser = new AppUser
                {
                    UserName = email,
                    Email = email,
                    UserType = userType,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                // Create user in Identity
                var identityResult = await userManager.CreateAsync(appUser, password);
                
                if (!identityResult.Succeeded)
                {
                    var errors = identityResult.Errors.Select(e => e.Description);
                    return UserRegistrationResult.Failure(errors);
                }

                // Create domain user via CreateUserCommand without specifying ID
                var createUserCommand = new CreateUserCommand(name, userType, email);
                var domainUser = await mediator.Send(createUserCommand, cancellationToken);

                // Update AppUser with the domain user ID
                appUser.DomainUserId = domainUser.Id;
                await userManager.UpdateAsync(appUser);

                return UserRegistrationResult.Success(appUser.Id, domainUser.Id);
            }
            catch (Exception ex)
            {
                return UserRegistrationResult.Failure($"An error occurred during registration: {ex.Message}");
            }
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            return user != null;
        }

        private static List<string> ValidateInput(string name, string email, string password)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(name))
                errors.Add("Name is required.");

            if (string.IsNullOrWhiteSpace(email))
                errors.Add("Email is required.");

            if (string.IsNullOrWhiteSpace(password))
                errors.Add("Password is required.");

            return errors;
        }

        private static string ExtractFirstName(string fullName)
        {
            return fullName.Split(' ').FirstOrDefault() ?? string.Empty;
        }

        private static string ExtractLastName(string fullName)
        {
            var parts = fullName.Split(' ');
            return parts.Length > 1 ? string.Join(" ", parts.Skip(1)) : string.Empty;
        }
    }
}