using SisOdonto.Application.Models.Attendant;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SisOdonto.Application.Interfaces
{
    public interface IAttendantService
    {
        #region Methods

        Task<AttendantDataModel> Get(Guid id);

        Task<IEnumerable<AttendantDataModel>> GetAll();

        Task Create(AttendantDataModel request);

        Task Update(AttendantDataModel request);

        Task Delete(Guid id);

        #endregion Methods
    }
}