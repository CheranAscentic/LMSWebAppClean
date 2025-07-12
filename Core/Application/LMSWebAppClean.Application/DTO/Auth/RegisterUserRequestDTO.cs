using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace LMSWebAppClean.Application.DTO.Auth
{
    public class RegisterUserRequestDTO : ICommand
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string UserType { get; set; }
    }
}