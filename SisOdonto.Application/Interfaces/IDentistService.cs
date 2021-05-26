using System.Collections.Generic;
using System.Threading.Tasks;
using SisOdonto.Application.Models.Dentist;

namespace SisOdonto.Application.Interfaces
{
    public interface IDentistService
    {
        #region Methods

        Task Create(DentistDataModel request);

        Task<IEnumerable<DentistDataModel>> GetAll();

        #endregion Methods
    }
}