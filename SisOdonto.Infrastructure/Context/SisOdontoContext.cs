using System.Linq;
using Microsoft.EntityFrameworkCore;
using SisOdonto.Domain.Entities;
using SisOdonto.Infrastructure.Mappings;

namespace SisOdonto.Infrastructure.Context
{
    public class SisOdontoContext : DbContext
    {
        public DbSet<Dentist> Dentists { get; set; }
        public DbSet<Attendant> Attendants { get; set; }

        #region Constructors

        public SisOdontoContext(DbContextOptions options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        #endregion Constructors

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetProperties()
                    .Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            modelBuilder.ApplyConfiguration(new UserMapping());
            modelBuilder.ApplyConfiguration(new DentistMapping());
            modelBuilder.ApplyConfiguration(new AttendantMapping());
            modelBuilder.ApplyConfiguration(new PatientMapping());
            modelBuilder.ApplyConfiguration(new HealthInsuranceMapping());
            modelBuilder.ApplyConfiguration(new SchedullingMapping());

        }
    }
}