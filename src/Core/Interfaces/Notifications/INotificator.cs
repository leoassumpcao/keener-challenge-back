using API.Core.Notifications;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;

namespace API.Core.Interfaces.Notifications
{
    public interface INotificator
    {
        void AddNotification(string message);
        void AddNotification(string key, string message);
        void AddNotification(Notification notification);
        void AddNotifications(IEnumerable<Notification> notifications);
        void AddNotifications(IEnumerable<string> messages);
        void AddNotifications(ValidationResult validationResult);
        void AddNotifications(IEnumerable<IdentityError> identityErrors);
        IReadOnlyCollection<Notification> GetNotifications();
    }
}
