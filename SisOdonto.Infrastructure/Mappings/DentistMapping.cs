using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisOdonto.Domain.Entities;

namespace SisOdonto.Infrastructure.Mappings
{
    public class DentistMapping : IEntityTypeConfiguration<Dentist>
    {
        #region Methods

        public void Configure(EntityTypeBuilder<Dentist> builder)
        {
            builder
                .ToTable("Dentists");

            builder
                .Property(s => s.CreatedOn)
                .HasColumnType("datetimeoffset")
                .IsRequired();

            builder
                .Property(s => s.IsDeleted)
                .IsRequired();

            builder
                .Property(si => si.Cro)
                .IsRequired();

            builder
                .Property(si => si.Expertise)
                .IsRequired();

            builder
                .Ignore(si => si.CascadeMode);

            builder
                .Ignore(si => si.ValidationResult);
        }

        #endregion Methods
    }
}
