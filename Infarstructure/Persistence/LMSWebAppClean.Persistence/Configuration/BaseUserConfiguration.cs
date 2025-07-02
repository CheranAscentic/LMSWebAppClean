using LMSWebAppClean.Domain.Base;
using LMSWebAppClean.Domain.Enum;
using LMSWebAppClean.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMSWebAppClean.Persistence.Configuration
{
    public class BaseUserConfiguration : IEntityTypeConfiguration<BaseUser>
    {
        public void Configure(EntityTypeBuilder<BaseUser> builder)
        {
            builder.HasKey(u => u.Id);
            
            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100);
            
            builder.UseTptMappingStrategy();

            builder.ToTable("Users");
        }
    }
}