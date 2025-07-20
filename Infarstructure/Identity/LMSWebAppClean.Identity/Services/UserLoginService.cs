using LMSWebAppClean.Application.DTO.Identity;
using LMSWebAppClean.Application.DTO.Auth;
using LMSWebAppClean.Application.DTO;
using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Identity.Models;
using LMSWebAppClean.Domain.Base;
using Microsoft.AspNetCore.Identity;
using MediatR;

namespace LMSWebAppClean.Identity.Services
{
    public class UserLoginService : IUserLoginService
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ITokenService tokenService;
        private readonly IRepository<BaseUser> userRepository;

        public UserLoginService(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService,
            IRepository<BaseUser> userRepository)
        {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            this.tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<UserLoginResult> LoginAsync(string email, string password, bool rememberMe = false)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    return UserLoginResult.Failure("Email is required.");
                }

                if (string.IsNullOrWhiteSpace(password))
                {
                    return UserLoginResult.Failure("Password is required.");
                }

                var user = await userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return UserLoginResult.Failure("Invalid email or password.");
                }

                if (!user.IsActive)
                {
                    return UserLoginResult.Failure("User account is inactive.");
                }

                var result = await signInManager.PasswordSignInAsync(
                    user.UserName!,
                    password,
                    rememberMe,
                    lockoutOnFailure: false);

                if (!result.Succeeded)
                {
                    if (result.IsNotAllowed)
                    {
                        return UserLoginResult.Failure("User is not allowed to sign in.");
                    }
                    return UserLoginResult.Failure("Invalid email or password.");
                }

                user.LastLoginAt = DateTime.UtcNow;
                await userManager.UpdateAsync(user);

                // Get domain user info
                var domainUser = userRepository.Get(user.DomainUserId ?? 0);
                if (domainUser == null)
                {
                    return UserLoginResult.Failure("User profile not found.");
                }

                // Create UserDTO
                /*var userDto = new UserDTO
                {
                    Id = domainUser.Id,
                    Name = domainUser.Name,
                    Email = domainUser.Email,
                    Type = domainUser.Type
                    FirstName = domainUser.FirstName,
                    LastName = domainUser.LastName
                };*/

                // Generate JWT token
                var token = tokenService.GenerateJwtToken(domainUser); // Remove "Bearer " prefix
                var expiresAt = tokenService.GetTokenExpiration();

                var loginResponse = new LoginDTO(domainUser, token, expiresAt);
                return UserLoginResult.Success(loginResponse);
            }
            catch (Exception ex)
            {
                return UserLoginResult.Failure($"An error occurred during login: {ex.Message}");
            }
        }

        public async Task LogoutAsync()
        {
            await signInManager.SignOutAsync();
        }
    }
}