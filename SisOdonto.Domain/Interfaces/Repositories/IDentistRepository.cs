using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SisOdonto.Domain.Entities;

namespace SisOdonto.Domain.Interfaces.Repositories
{
    public interface IDentistRepository : IDisposable
    {
        #region Methods

        Task AddAsync(Dentist dentist);

        Task<IEnumerable<Dentist>> GetAllAsync();

        Task<Dentist> GetAsync(Guid id);

        Task<Dentist> GetAsync(string cpf);
        void Update(Dentist dentist);

        #endregion Methods
    }
}