using SisOdonto.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SisOdonto.Domain.Interfaces.Repositories
{
    public interface ISchedulingRepository : IDisposable
    {
        #region Methods

        Task AddAsync(Scheduling scheduling);

        Task<IEnumerable<Scheduling>> GetAllAsync();

        Task<IEnumerable<Scheduling>> GetAllByDentistAsync(Guid dentistId);

        Task<IEnumerable<Scheduling>> GetAllByPatientAsync(Guid patientId);

        Task<Scheduling> GetAsync(Guid id);
        void Update(Scheduling scheduling);

        #endregion Methods
    }
}