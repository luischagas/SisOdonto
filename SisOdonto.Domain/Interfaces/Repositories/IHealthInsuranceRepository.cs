using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SisOdonto.Domain.Entities;

namespace SisOdonto.Domain.Interfaces.Repositories
{
    public interface IHealthInsuranceRepository : IDisposable
    {
        #region Methods

        Task AddAsync(HealthInsurance healthInsurance);

        Task<IEnumerable<HealthInsurance>> GetAllAsync();

        Task<HealthInsurance> GetAsync(Guid id);

        Task<HealthInsurance> GetAsync(string name);

        void Update(HealthInsurance healthInsurance);

        #endregion Methods
    }
}