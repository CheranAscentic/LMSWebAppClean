using LMSWebAppClean.Domain.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseUserConfiguration.cs" company="Ascentic">
//   Copyright (c) Ascentic. All rights reserved.
// </copyright>
// <summary>
//   EntityTypeConfiguration for BaseUser.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMSWebAppClean.Persistence.Configuration
{
    /// <summary>
    /// EntityTypeConfiguration for BaseUser.
    /// </summary>
    public class BaseUserConfiguration : IEntityTypeConfiguration<BaseUser>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<BaseUser> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.Type)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.FirstName)
                .HasMaxLength(50);

            builder.Property(u => u.LastName)
                .HasMaxLength(50);

            builder.Property(u => u.Address)
                .HasMaxLength(255);

            builder.UseTptMappingStrategy();

            builder.ToTable("Users");
        }
    }
}