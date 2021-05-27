using SisOdonto.Application.Models.HealthInsurance;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SisOdonto.Application.Interfaces
{
    public interface IHealthInsuranceService
    {
        #region Methods

        Task Create(HealthInsuranceDataModel request);

        Task Delete(Guid id);

        Task<HealthInsuranceDataModel> Get(Guid id);

        Task<IEnumerable<HealthInsuranceDataModel>> GetAll();
        Task Update(HealthInsuranceDataModel request);
        #endregion Methods
    }
}