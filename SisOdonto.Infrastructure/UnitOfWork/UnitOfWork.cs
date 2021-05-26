using SisOdonto.Domain.Shared;
using SisOdonto.Infrastructure.Context;
using System;
using System.Threading.Tasks;

namespace SisOdonto.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Fields

        private readonly SisOdontoContext _context;
        private bool _disposed;

        #endregion Fields

        #region Constructors

        public UnitOfWork(SisOdontoContext context)
        {
            _context = context;
            _disposed = false;
        }

        #endregion Constructors

        #region Methods

        public bool Commit()
        {
            bool result;

            try
            {
                result = _context.SaveChanges() > 0;
            }
            catch (System.Exception)
            {
                throw;
            }

            return result;
        }

        public async Task<bool> CommitAsync()
        {
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

        #endregion Methods

        #region Dispose

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion Dispose
    }
}