using LMSWebAppClean.Domain.Base;
using LMSWebAppClean.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMSWebAppClean.Persistence.Configuration
{
    public class StaffConfiguration : IEntityTypeConfiguration<Staff>
    {
        public void Configure(EntityTypeBuilder<Staff> builder)
        {
            // Configure as derived type of BaseUser
            builder.HasBaseType<BaseUser>();

            // Configure to use separate table
            builder.ToTable("Staffs");

            // Any Staff-specific configurations can go here
        }
    }
}