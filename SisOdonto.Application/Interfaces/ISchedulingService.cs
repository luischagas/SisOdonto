using SisOdonto.Application.Models.Scheduling;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SisOdonto.Application.Interfaces
{
    public interface ISchedulingService
    {
        #region Methods

        Task<SchedulingDataModel> Get(Guid id);

        Task<IEnumerable<SchedulingDataModel>> GetAll(string userType);

        Task Create(SchedulingDataModel request);

        Task Update(SchedulingDataModel request);

        Task Delete(Guid id);

        #endregion Methods
    }
}