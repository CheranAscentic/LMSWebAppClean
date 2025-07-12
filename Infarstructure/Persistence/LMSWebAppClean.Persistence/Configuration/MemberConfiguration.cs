// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberConfiguration.cs" company="Ascentic">
//   Copyright (c) Ascentic. All rights reserved.
// </copyright>
// <summary>
//   EntityTypeConfiguration for Member.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMSWebAppClean.Persistence.Configuration
{
    using LMSWebAppClean.Domain.Model;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// EntityTypeConfiguration for Member.
    /// </summary>
    public class MemberConfiguration : IEntityTypeConfiguration<Member>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            // Configure to use separate table with Table-per-Type (TPT) inheritance
            builder.ToTable("Members");

            // Configure the backing field for BorrowedBooks collection
            builder.Navigation(m => m.BorrowedBooks)
                .HasField("borrowedBooks")
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}