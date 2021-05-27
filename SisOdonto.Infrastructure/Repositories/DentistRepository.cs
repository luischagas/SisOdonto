using Microsoft.EntityFrameworkCore;
using SisOdonto.Domain.Entities;
using SisOdonto.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SisOdonto.Domain.Interfaces;
using SisOdonto.Domain.Interfaces.Repositories;

namespace SisOdonto.Infrastructure.Repositories
{
    public class DentistRepository : IDentistRepository
    {
        #region Fields

        private readonly SisOdontoContext _db;
        private readonly DbSet<Dentist> _dentists;
        private bool _disposed;

        #endregion Fields

        #region Constructors

        public DentistRepository(SisOdontoContext db)
        {
            _db = db;
            _dentists = _db.Set<Dentist>();
            _disposed = false;
        }

        public async Task AddAsync(Dentist dentist)
        {
            await _dentists
                .AddAsync(dentist);
        }

        public async Task<Dentist> GetAsync(Guid id)
        {
            return await _dentists
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<Dentist> GetAsync(string cpf)
        {
            return await _dentists
                .FirstOrDefaultAsync(d => d.Cpf == cpf);
        }


        public async Task<IEnumerable<Dentist>> GetAllAsync()
        {
            return await _dentists
                .ToListAsync();
        }

        public void Update(Dentist dentist)
        {
            _db.Update(dentist);
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