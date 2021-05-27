using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SisOdonto.Domain.Entities;

namespace SisOdonto.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IDisposable
    {
        #region Methods

        Task AddAsync(User user);

        Task<IEnumerable<User>> GetAllAsync();

        Task<User> GetAsync(Guid id);

        Task<User> GetAsync(string cpf);

        void Update(User user);

        #endregion Methods
    }
}