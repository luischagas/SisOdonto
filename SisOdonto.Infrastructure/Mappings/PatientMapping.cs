using Microsoft.EntityFrameworkCore;
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
                .Property(si => si.Gender)
                .IsRequired();

            builder
                .Property(si => si.MaritalStatus)
                .IsRequired();

            builder
                .Property(si => si.Occupation)
                .IsRequired();

            builder
                .Property(si => si.Telephone);

            builder
                .Property(si => si.Cellular)
                .IsRequired();

            builder
                .HasOne(i => i.HealthInsurance)
                .WithOne()
                .HasForeignKey<Patient>(b => b.HealthInsuranceId)
                .IsRequired();

            builder
                .Ignore(si => si.CascadeMode);

            builder
                .Ignore(si => si.ValidationResult);
        }

        #endregion Methods
    }
}