using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Domain.Enum;
using Microsoft.AspNetCore.Antiforgery;
using System.ComponentModel.DataAnnotations;

namespace LMSWebAppClean.Application.DTO
{
    public class RegisterUserDTO : ICommand
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
