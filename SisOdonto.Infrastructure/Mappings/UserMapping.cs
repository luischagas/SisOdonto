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
                .HasKey(i => i.Id).IsClustered(false);

            builder
                .Property(u => u.CreatedOn)
                .HasColumnType("datetimeoffset")
                .IsRequired();

            builder
                .Property(u => u.IsDeleted)
                .IsRequired();

            builder
                .Property(u => u.Name)
                .IsRequired();

            builder
                .Property(u => u.BirthDate)
                .HasColumnType("datetimeoffset")
                .IsRequired();

            builder
                .Property(u => u.Cep)
                .IsRequired();

            builder
                .Property(u => u.Street)
                .IsRequired();

            builder
                .Property(u => u.Number)
                .IsRequired();

            builder
                .Property(u => u.Complement);

            builder
                .Property(u => u.District)
                .IsRequired();

            builder
                .Property(u => u.City)
                .IsRequired();

            builder
                .Property(u => u.State)
                .IsRequired();

            builder
                .Property(u => u.Cpf)
                .IsRequired();

            builder
                .Property(u => u.Email)
                .IsRequired();

            builder
                .Ignore(u => u.CascadeMode);

            builder
                .Ignore(u => u.ValidationResult);

            builder
                .HasQueryFilter(u => u.IsDeleted == false);
        }
    }
}