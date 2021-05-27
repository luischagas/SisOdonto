using Microsoft.EntityFrameworkCore;
using SisOdonto.Domain.Entities;
using SisOdonto.Domain.Interfaces.Repositories;
using SisOdonto.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SisOdonto.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        #region Fields

        private readonly SisOdontoContext _db;
        private readonly DbSet<User> _users;
        private bool _disposed;

        #endregion Fields

        #region Constructors

        public UserRepository(SisOdontoContext db)
        {
            _db = db;
            _users = _db.Set<User>();
            _disposed = false;
        }

        public async Task AddAsync(User user)
        {
            await _users
                .AddAsync(user);
        }

        public void Dispose()
        {
            _db.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _users
                .ToListAsync();
        }

        public async Task<User> GetAsync(Guid id)
        {
            return await _users
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<User> GetAsync(string cpf)
        {
            return await _users
                .FirstOrDefaultAsync(d => d.Cpf == cpf);
        }
        public void Update(User user)
        {
            _db.Update(user);
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