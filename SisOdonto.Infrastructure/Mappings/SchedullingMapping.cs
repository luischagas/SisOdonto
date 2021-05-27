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
                .HasKey(i => i.Id).IsClustered(false);

            builder
                .Property(s => s.CreatedOn)
                .HasColumnType("datetimeoffset")
                .IsRequired();

            builder
                .Property(s => s.IsDeleted)
                .IsRequired();

            builder
                .Property(s => s.Datetime)
                .HasColumnType("datetimeoffset")
                .IsRequired();

            builder
                .Property(s => s.Obs);

            builder
                .HasOne(s => s.Patient)
                .WithMany(a => a.Schedules)
                .HasForeignKey(s => s.PatientId)
                .IsRequired();

            builder
                .HasOne(s => s.Dentist)
                .WithMany(d => d.Schedules)
                .HasForeignKey(s => s.DentistId)
                .IsRequired();

            builder
                .Ignore(s => s.CascadeMode);

            builder
                .Ignore(s => s.ValidationResult);

            builder
                .HasQueryFilter(u => u.IsDeleted == false);
        }

        #endregion Methods
    }
}