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
    public class HealthInsuranceMapping : IEntityTypeConfiguration<HealthInsurance>
    {
        #region Methods

        public void Configure(EntityTypeBuilder<HealthInsurance> builder)
        {
            builder
                .ToTable("HealthInsurances");

            builder
                .Property(s => s.CreatedOn)
                .HasColumnType("datetimeoffset")
                .IsRequired();

            builder
                .Property(s => s.IsDeleted)
                .IsRequired();

            builder
                .Property(si => si.Name)
                .IsRequired();

            builder
                .Property(si => si.Type)
                .IsRequired();

            builder
                .Ignore(si => si.CascadeMode);

            builder
                .Ignore(si => si.ValidationResult);
        }

        #endregion Methods
    }
}
