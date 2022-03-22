using API.Core.Interfaces.Notifications;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;

namespace API.Core.Notifications
{
    public class Notificator : INotificator
    {
        private readonly List<Notification> _notifications;

        public Notificator() => _notifications = new List<Notification>();

        public IReadOnlyCollection<Notification> GetNotifications()
        {
            return _notifications;
        }

        public void AddNotification(string message)
        {
            AddNotification("", message);
        }

        public void AddNotification(string key, string message)
        {
            AddNotification(new Notification(key, message));
        }

        public void AddNotification(Notification notification)
        {
            _notifications.Add(notification);
        }

        public void AddNotifications(IEnumerable<Notification> notifications)
        {
            _notifications.AddRange(notifications);
        }

        public void AddNotifications(IEnumerable<string> messages)
        {
            foreach (var message in messages)
                AddNotification("", message);
        }

        public void AddNotifications(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
                AddNotification(error.ErrorCode, error.ErrorMessage);
        }

        public void AddNotifications(IEnumerable<IdentityError> identityErrors)
        {
            foreach (var error in identityErrors)
                AddNotification(error.Code, error.Description);
        }

        public void Clear()
        {
            _notifications.Clear();
        }
    }
}
