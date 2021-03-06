using SisOdonto.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SisOdonto.Domain.Interfaces.Repositories
{
    public interface IPatientRepository : IDisposable
    {
        #region Methods

        Task AddAsync(Patient patient);

        Task<IEnumerable<Patient>> GetAllAsync();
        Task<Patient> GetByHealthInsuranceAsync(Guid healthInsuranceId);

        Task<Patient> GetAsync(Guid id);

        Task<Patient> GetAsync(string cpf);

        void Update(Patient patient);

        #endregion Methods
    }
}