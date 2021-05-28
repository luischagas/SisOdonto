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
                .Property(a => a.CreatedOn)
                .HasColumnType("datetime")
                .IsRequired();

            builder
                .Property(a => a.IsDeleted)
                .IsRequired();

            builder
                .Property(a => a.Telephone)
                .IsRequired();

            builder
                .Property(a => a.Cellular)
                .IsRequired();

            builder
                .Ignore(a => a.CascadeMode);

            builder
                .Ignore(a => a.ValidationResult);

           
        }

        #endregion Methods
    }
}