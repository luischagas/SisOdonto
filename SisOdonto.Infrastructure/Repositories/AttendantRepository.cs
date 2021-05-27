using Microsoft.EntityFrameworkCore;
using SisOdonto.Domain.Entities;
using SisOdonto.Domain.Interfaces.Repositories;
using SisOdonto.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SisOdonto.Infrastructure.Repositories
{
    public class AttendantRepository : IAttendantRepository
    {
        #region Fields

        private readonly DbSet<Attendant> _attendants;
        private readonly SisOdontoContext _db;
        private bool _disposed;

        #endregion Fields

        #region Constructors

        public AttendantRepository(SisOdontoContext db)
        {
            _db = db;
            _attendants = _db.Set<Attendant>();
            _disposed = false;
        }

        public async Task AddAsync(Attendant attendant)
        {
            await _attendants
                .AddAsync(attendant);
        }

        public void Dispose()
        {
            _db.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<IEnumerable<Attendant>> GetAllAsync()
        {
            return await _attendants
                .ToListAsync();
        }

        public async Task<Attendant> GetAsync(Guid id)
        {
            return await _attendants
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<Attendant> GetAsync(string cpf)
        {
            return await _attendants
                .FirstOrDefaultAsync(d => d.Cpf == cpf);
        }
        public void Update(Attendant attendant)
        {
            _db.Update(attendant);
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