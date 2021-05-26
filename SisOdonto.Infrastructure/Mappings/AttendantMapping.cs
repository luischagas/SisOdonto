using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisOdonto.Domain.Entities;

namespace SisOdonto.Infrastructure.Mappings
{
    public class AttendantMapping : IEntityTypeConfiguration<Attendant>
    {
        #region Methods

        public void Configure(EntityTypeBuilder<Attendant> builder)
        {
            builder
                .ToTable("Attendants");

            builder
                .Property(s => s.CreatedOn)
                .HasColumnType("datetimeoffset")
                .IsRequired();

            builder
                .Property(s => s.IsDeleted)
                .IsRequired();

            builder
                .Property(si => si.Telephone)
                .IsRequired();

            builder
                .Property(si => si.Cellular)
                .IsRequired();

            builder
                .Ignore(si => si.CascadeMode);

            builder
                .Ignore(si => si.ValidationResult);

           
        }

        #endregion Methods
    }
}