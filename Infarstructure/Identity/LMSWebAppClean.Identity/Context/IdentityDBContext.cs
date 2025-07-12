using LMSWebAppClean.Domain.Enum;
using LMSWebAppClean.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LMSWebAppClean.Identity.Context
{
    public class IdentityDBContext : IdentityDbContext<AppUser, IdentityType, string>
    {
        public IdentityDBContext(DbContextOptions<IdentityDBContext> options) 
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure AppUser entity
            builder.Entity<AppUser>(entity =>
            {
                entity.Property(e => e.UserType).HasMaxLength(50);
            });

            // Seed default roles
            var roles = new []
            {
                new IdentityType
                {
                    Id = "1",
                    Name = "Member",
                    NormalizedName = "MEMBER",
                    UserType = UserType.Member,
                },
                new IdentityType
                {
                    Id = "2",
                    Name = "StaffMinor",
                    NormalizedName = "STAFFMINOR",
                    UserType = UserType.StaffMinor,
                },
                new IdentityType
                {
                    Id = "3",
                    Name = "StaffManagement",
                    NormalizedName = "STAFFMANAGEMENT",
                    UserType = UserType.StaffManagement,
                },
            };

            builder.Entity<IdentityType>().HasData(roles);
        }
    }
}
