using System.ComponentModel.DataAnnotations;
using LMSWebAppClean.Application.Interface;

namespace LMSWebAppClean.Application.DTO.Auth
{
    public class LoginUserDTO : ICommand
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; } = false;
    }
}