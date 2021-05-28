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
    public class SchedulingRepository : ISchedulingRepository
    {
        #region Fields

        private readonly SisOdontoContext _db;
        private readonly DbSet<Scheduling> _schedules;
        private bool _disposed;

        #endregion Fields

        #region Constructors

        public SchedulingRepository(SisOdontoContext db)
        {
            _db = db;
            _schedules = _db.Set<Scheduling>();
            _disposed = false;
        }

        public async Task AddAsync(Scheduling scheduling)
        {
            await _schedules
                .AddAsync(scheduling);
        }

        public void Dispose()
        {
            _db.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<IEnumerable<Scheduling>> GetAllAsync()
        {
            return await _schedules
                .Include(s => s.Patient)
                .Include(s => s.Dentist)
                .ToListAsync();
        }

        public async Task<IEnumerable<Scheduling>> GetAllByDentistAsync(Guid dentistId)
        {
            return await _schedules
                .Include(s => s.Patient)
                .Include(s => s.Dentist)
                .Where(s => s.DentistId == dentistId)

                .ToListAsync();
        }

        public async Task<IEnumerable<Scheduling>> GetAllByPatientAsync(Guid patientId)
        {
            return await _schedules
                .Include(s => s.Patient)
                .Include(s => s.Dentist)
                .Where(s => s.PatientId == patientId)
                .ToListAsync();
        }

        public async Task<Scheduling> GetAsync(Guid id)
        {
            return await _schedules
                .Include(s => s.Patient)
                .Include(s => s.Dentist)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
        public void Update(Scheduling scheduling)
        {
            _db.Update(scheduling);
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