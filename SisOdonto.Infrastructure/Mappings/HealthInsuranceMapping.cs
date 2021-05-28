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
                .HasKey(i => i.Id).IsClustered(false);

            builder
                .Property(hi => hi.CreatedOn)
                .HasColumnType("datetime")
                .IsRequired();

            builder
                .Property(hi => hi.IsDeleted)
                .IsRequired();

            builder
                .Property(hi => hi.Name)
                .IsRequired();

            builder
                .Property(hi => hi.Type)
                .IsRequired();

            builder
                .Ignore(hi => hi.CascadeMode);

            builder
                .Ignore(hi => hi.ValidationResult);


            builder
                .HasQueryFilter(u => u.IsDeleted == false);
        }

        #endregion Methods
    }
}
