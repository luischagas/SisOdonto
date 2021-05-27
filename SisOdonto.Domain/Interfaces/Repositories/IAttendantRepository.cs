using SisOdonto.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SisOdonto.Domain.Interfaces.Repositories
{
    public interface IAttendantRepository : IDisposable
    {
        #region Methods

        Task AddAsync(Attendant attendant);

        Task<IEnumerable<Attendant>> GetAllAsync();

        Task<Attendant> GetAsync(Guid id);

        Task<Attendant> GetAsync(string cpf);

        void Update(Attendant attendant);

        #endregion Methods
    }
}