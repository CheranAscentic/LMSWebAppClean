using LMSWebAppClean.Domain.Enum;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSWebAppClean.Identity.Models
{
    public class IdentityType : IdentityRole
    {
        public string UserType { get; set; }
        public IdentityType() { }
    }
}
