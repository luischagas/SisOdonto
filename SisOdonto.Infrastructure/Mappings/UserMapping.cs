using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisOdonto.Domain.Entities;

namespace SisOdonto.Infrastructure.Mappings
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder
                .HasKey(si => si.Id);

            builder
                .Property(si => si.CreatedOn)
                .HasColumnType("datetimeoffset")
                .IsRequired();

            builder
                .Property(si => si.IsDeleted)
                .IsRequired();

            builder
                .Property(si => si.Name)
                .IsRequired();

            builder
                .Property(si => si.BirthDate)
                .HasColumnType("datetimeoffset")
                .IsRequired();

            builder
                .Property(si => si.Cep)
                .IsRequired();

            builder
                .Property(si => si.Street)
                .IsRequired();

            builder
                .Property(si => si.Number)
                .IsRequired();

            builder
                .Property(si => si.Complement);

            builder
                .Property(si => si.District)
                .IsRequired();

            builder
                .Property(si => si.City)
                .IsRequired();

            builder
                .Property(si => si.State)
                .IsRequired();

            builder
                .Property(si => si.Cpf)
                .IsRequired();

            builder
                .Property(si => si.Email)
                .IsRequired();

            builder
                .Ignore(si => si.CascadeMode);

            builder
                .Ignore(si => si.ValidationResult);
        }
    }
}