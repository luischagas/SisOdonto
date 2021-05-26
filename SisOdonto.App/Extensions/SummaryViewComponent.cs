using Microsoft.AspNetCore.Mvc;
using SisOdonto.Domain.Interfaces.Notification;
using System.Threading.Tasks;

namespace SisOdonto.App.Extensions
{
    public class SummaryViewComponent : ViewComponent
    {
        #region Fields

        private readonly INotifier _notifier;

        #endregion Fields

        #region Constructors

        public SummaryViewComponent(INotifier notifier)
        {
            _notifier = notifier;
        }

        #endregion Constructors

        #region Methods

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var notificacoes = await Task.FromResult(_notifier.GetAllNotifications());
            notificacoes.ForEach(c => ViewData.ModelState.AddModelError(string.Empty, c.Message));

            return View();
        }

        #endregion Methods
    }
}