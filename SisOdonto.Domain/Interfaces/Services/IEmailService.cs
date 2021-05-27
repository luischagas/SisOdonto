using System.Threading.Tasks;

namespace SisOdonto.Domain.Interfaces.Services
{
    public interface IEmailService
    {
        #region Methods

        void SendEmail(string to, string subject, string shortTitle, string message);

        #endregion Methods
    }
}