using SisOdonto.Application.Models.Scheduling;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SisOdonto.Application.Interfaces
{
    public interface ISchedulingService
    {
        #region Methods

        Task<SchedulingDataModel> Get(Guid id);

        Task<IEnumerable<SchedulingDataModel>> GetAll(Guid userId);

        Task Create(SchedulingDataModel request);

        Task Update(SchedulingDataModel request);

        Task Delete(Guid id);

        #endregion Methods
    }
}