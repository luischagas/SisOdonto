using Microsoft.EntityFrameworkCore;
using SisOdonto.Domain.Entities;
using SisOdonto.Domain.Interfaces.Repositories;
using SisOdonto.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SisOdonto.Infrastructure.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        #region Fields

        private readonly SisOdontoContext _db;
        private readonly DbSet<Patient> _patients;
        private bool _disposed;

        #endregion Fields

        #region Constructors

        public PatientRepository(SisOdontoContext db)
        {
            _db = db;
            _patients = _db.Set<Patient>();
            _disposed = false;
        }

        public async Task AddAsync(Patient patient)
        {
            await _patients
                .AddAsync(patient);
        }

        public async Task<Patient> GetAsync(Guid id)
        {
            return await _patients
                .Include(p => p.HealthInsurance)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<Patient> GetAsync(string cpf)
        {
            return await _patients
                .FirstOrDefaultAsync(d => d.Cpf == cpf);
        }

        public async Task<Patient> GetByHealthInsuranceAsync(Guid healthInsuranceId)
        {
            return await _patients
                .FirstOrDefaultAsync(d => d.HealthInsuranceId == healthInsuranceId);
        }
        public async Task<IEnumerable<Patient>> GetAllAsync()
        {
            return await _patients
                .ToListAsync();
        }

        public void Update(Patient patient)
        {
            _db.Update(patient);
        }

        public void Dispose()
        {
            _db.Dispose();
            GC.SuppressFinalize(this);
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