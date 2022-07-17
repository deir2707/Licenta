using System.Threading.Tasks;

namespace Infrastructure.Notifications
{
    public interface INotificationPublisher
    {
        Task PublishMessageToUser(Notification notification);
    }
}