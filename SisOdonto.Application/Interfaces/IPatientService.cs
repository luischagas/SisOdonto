using SisOdonto.Application.Models.Patient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SisOdonto.Application.Interfaces
{
    public interface IPatientService
    {
        #region Methods

        Task Create(PatientDataModel request);

        Task Delete(Guid id);

        Task<PatientDataModel> Get(Guid id);

        Task<IEnumerable<PatientDataModel>> GetAll();
        Task Update(PatientDataModel request);
        #endregion Methods
    }
}