using System.Threading.Tasks;
using Infrastructure.Notifications;
using Microsoft.AspNetCore.SignalR;

namespace BackEnd.Hubs
{
    public class AuctionHub: Hub, INotificationPublisher
    {
        private IHubContext<AuctionHub> _context;

        public AuctionHub(IHubContext<AuctionHub> context)
        {
            _context = context;
        }
        
        public async Task SendMessage(string user, string message)
        {
            await _context.Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task PublishMessageToUser(Notification notification)
        {
            await _context.Clients.All.SendAsync("onPublishMessage", notification);
        }
    }
}