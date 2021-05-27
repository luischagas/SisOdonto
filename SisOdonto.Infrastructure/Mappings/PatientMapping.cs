﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisOdonto.Domain.Entities;

namespace SisOdonto.Infrastructure.Mappings
{
    public class PatientMapping : IEntityTypeConfiguration<Patient>
    {
        #region Methods

        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder
                .ToTable("Patients");

            builder
                .Property(s => s.CreatedOn)
                .HasColumnType("datetimeoffset")
                .IsRequired();

            builder
                .Property(s => s.IsDeleted)
                .IsRequired();

            builder
                .Property(p => p.Gender)
                .IsRequired();

            builder
                .Property(p => p.MaritalStatus)
                .IsRequired();

            builder
                .Property(p => p.Occupation)
                .IsRequired();

            builder
                .Property(p => p.Telephone);

            builder
                .Property(p => p.Cellular)
                .IsRequired();

            builder
                .HasOne(i => i.HealthInsurance)
                .WithOne()
                .HasForeignKey<Patient>(b => b.HealthInsuranceId)
                .IsRequired();

            builder
                .Ignore(p => p.CascadeMode);

            builder
                .Ignore(p => p.ValidationResult);
        }

        #endregion Methods
    }
}