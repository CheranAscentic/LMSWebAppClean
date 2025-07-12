// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StaffConfiguration.cs" company="Ascentic">
//   Copyright (c) Ascentic. All rights reserved.
// </copyright>
// <summary>
//   EntityTypeConfiguration for Staff.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMSWebAppClean.Persistence.Configuration
{
    using LMSWebAppClean.Domain.Base;
    using LMSWebAppClean.Domain.Model;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// EntityTypeConfiguration for Staff.
    /// </summary>
    public class StaffConfiguration : IEntityTypeConfiguration<Staff>
    {
        /// <inheritdoc />
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