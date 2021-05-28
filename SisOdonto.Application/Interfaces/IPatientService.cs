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

        Task<PatientDataModel> GetByCpf(string cpf);

        Task<IEnumerable<PatientDataModel>> GetAll();

        Task<IEnumerable<PatientDataModel>> GetAllToReport(bool particular);

        Task Update(PatientDataModel request);
        #endregion Methods
    }
}