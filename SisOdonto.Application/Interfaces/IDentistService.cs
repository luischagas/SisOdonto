using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SisOdonto.Application.Models.Dentist;

namespace SisOdonto.Application.Interfaces
{
    public interface IDentistService
    {
        #region Methods

        Task<DentistDataModel> Get(Guid id);

        Task<IEnumerable<DentistDataModel>> GetAll();

        Task Create(DentistDataModel request);

        Task Update(DentistDataModel request);

        Task Delete(Guid id);

        #endregion Methods
    }
}