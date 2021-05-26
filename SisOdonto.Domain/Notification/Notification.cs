namespace SisOdonto.Domain.Notification
{
    public class Notification
    {
        #region Constructors

        public Notification(string message)
        {
            Message = message;
        }

        #endregion Constructors

        #region Properties

        public string Message { get; }

        #endregion Properties
    }
}