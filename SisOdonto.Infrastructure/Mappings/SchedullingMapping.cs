using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisOdonto.Domain.Entities;

namespace SisOdonto.Infrastructure.Mappings
{
    public class SchedullingMapping : IEntityTypeConfiguration<Scheduling>
    {
        #region Methods

        public void Configure(EntityTypeBuilder<Scheduling> builder)
        {
            builder
                .ToTable("Schedules");

            builder
                .Property(s => s.CreatedOn)
                .HasColumnType("datetimeoffset")
                .IsRequired();

            builder
                .Property(s => s.IsDeleted)
                .IsRequired();

            builder
                .Property(si => si.Datetime)
                .HasColumnType("datetimeoffset")
                .IsRequired();

            builder
                .Property(si => si.Obs);

            builder
                .HasOne(s => s.Patient)
                .WithMany(a => a.Schedules)
                .HasForeignKey(s => s.PatientId)
                .IsRequired();

            builder
                .HasOne(s => s.Dentist)
                .WithMany(a => a.Schedules)
                .HasForeignKey(s => s.DentistId)
                .IsRequired();

            builder
                .Ignore(si => si.CascadeMode);

            builder
                .Ignore(si => si.ValidationResult);
        }

        #endregion Methods
    }
}