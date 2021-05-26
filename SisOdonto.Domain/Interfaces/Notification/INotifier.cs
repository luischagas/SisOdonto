using System.Collections.Generic;

namespace SisOdonto.Domain.Interfaces.Notification
{
    public interface INotifier
    {
        #region Methods

        List<Domain.Notification.Notification> GetAllNotifications();

        void Handle(Domain.Notification.Notification notification);

        bool HasNotification();

        #endregion Methods
    }
}