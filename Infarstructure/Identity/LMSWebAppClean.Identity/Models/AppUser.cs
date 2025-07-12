using LMSWebAppClean.Domain.Enum;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSWebAppClean.Identity.Models
{
    public class AppUser : IdentityUser
    {
/*        public string? FirstName { get; set; }
        public string? LastName { get; set; }*/
        public string UserType { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }
        public bool IsActive { get; set; } = true;
        public int? DomainUserId { get; set; } // Add this to store BaseUser ID
    }
}
