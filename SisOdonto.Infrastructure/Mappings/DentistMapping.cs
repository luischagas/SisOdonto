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
                .Property(d => d.CreatedOn)
                .HasColumnType("datetimeoffset")
                .IsRequired();

            builder
                .Property(d => d.IsDeleted)
                .IsRequired();

            builder
                .Property(d => d.Cro)
                .IsRequired();

            builder
                .Property(d => d.Expertise)
                .IsRequired();

            builder
                .Ignore(d => d.CascadeMode);

            builder
                .Ignore(d => d.ValidationResult);
        }

        #endregion Methods
    }
}
