using LMSWebAppClean.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMSWebAppClean.Persistence.Configuration
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(b => b.Id);
            
            builder.Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(200);
                
            builder.Property(b => b.Author)
                .HasMaxLength(100);
                
            builder.Property(b => b.Category)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(b => b.Available)
                .IsRequired();
                
            // Configure the relationship with Member (a book can be borrowed by only one Member)
            builder.HasOne<Member>()
                .WithMany(m => m.BorrowedBooks)
                .HasForeignKey("MemberId")
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}