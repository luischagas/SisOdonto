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
                .HasColumnType("datetime")
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
                .WithMany(hi => hi.Patients)
                .HasForeignKey(b => b.HealthInsuranceId);

            builder
                .Ignore(p => p.CascadeMode);

            builder
                .Ignore(p => p.ValidationResult);
        }

        #endregion Methods
    }
}