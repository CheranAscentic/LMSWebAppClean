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

            builder.Property(b => b.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(b => b.Author)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(b => b.Category)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(b => b.Available)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(b => b.PublicationYear)
                .IsRequired(false);

            builder.HasOne(b => b.Member)
                .WithMany(m => m.BorrowedBooks)
                .HasForeignKey(b => b.MemberId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasIndex(b => b.MemberId)
                .HasDatabaseName("IX_Books_MemberId");

            builder.HasIndex(b => b.Available)
                .HasDatabaseName("IX_Books_Available");

            builder.Property(b => b.ISBN)
                .HasMaxLength(20)
                .IsRequired(false);

            builder.Property(b => b.Synopsis)
                .HasMaxLength(1000)
                .IsRequired(false);
        }
    }
}