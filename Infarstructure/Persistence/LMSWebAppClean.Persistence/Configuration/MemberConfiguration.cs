using LMSWebAppClean.Domain.Base;
using LMSWebAppClean.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMSWebAppClean.Persistence.Configuration
{
    public class MemberConfiguration : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            // Configure to use separate table with Table-per-Type (TPT) inheritance
            builder.ToTable("Members");
            
            // Configure the backing field for BorrowedBooks collection
            // Updated to match the new field name in Member class
            builder.Navigation(m => m.BorrowedBooks)
                .HasField("borrowedBooks")  // This matches your private field name
                .UsePropertyAccessMode(PropertyAccessMode.Field);
                
            // The relationship is configured in BookConfiguration to avoid conflicts
            // This ensures EF Core recognizes the relationship from both sides
        }
    }
}