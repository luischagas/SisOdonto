using Microsoft.EntityFrameworkCore;
using SisOdonto.Domain.Entities;
using SisOdonto.Domain.Interfaces.Repositories;
using SisOdonto.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SisOdonto.Infrastructure.Repositories
{
    public class HealthInsuranceRepository : IHealthInsuranceRepository
    {
        #region Fields

        private readonly SisOdontoContext _db;
        private readonly DbSet<HealthInsurance> _healthInsurances;
        private bool _disposed;

        #endregion Fields

        #region Constructors

        public HealthInsuranceRepository(SisOdontoContext db)
        {
            _db = db;
            _healthInsurances = _db.Set<HealthInsurance>();
            _disposed = false;
        }

        public async Task AddAsync(HealthInsurance healthInsurance)
        {
            await _healthInsurances
                .AddAsync(healthInsurance);
        }

        public void Dispose()
        {
            _db.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<IEnumerable<HealthInsurance>> GetAllAsync()
        {
            return await _healthInsurances
                .ToListAsync();
        }

        public async Task<HealthInsurance> GetAsync(Guid id)
        {
            return await _healthInsurances
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<HealthInsurance> GetAsync(string name)
        {
            return await _healthInsurances
                .FirstOrDefaultAsync(d => d.Name == name);
        }
        public void Update(HealthInsurance healthInsurance)
        {
            _db.Update(healthInsurance);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
            }
            this._disposed = true;
        }

        #endregion Constructors
    }
}