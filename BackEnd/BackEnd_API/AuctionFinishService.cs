using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using Infrastructure.Notifications;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Repository;

namespace BackEnd
{
    public class AuctionFinishService : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly INotificationPublisher _notificationPublisher;

        private Timer? _timer = null;

        public AuctionFinishService(INotificationPublisher notificationPublisher, IServiceScopeFactory scopeFactory)
        {
            _notificationPublisher = notificationPublisher;
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var firstAuctionToEnd = GetFirstAuctionToEnd();

            if (firstAuctionToEnd != null)
            {
                _timer = new Timer(OnTimer, firstAuctionToEnd, firstAuctionToEnd.EndDate - DateTime.UtcNow,
                    Timeout.InfiniteTimeSpan);
            }

            return Task.CompletedTask;
        }

        private Auction GetFirstAuctionToEnd()
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<AuctionContext>();
            return context.Auctions.Where(a => a.EndDate > DateTime.UtcNow)
                .OrderBy(a => a.EndDate).FirstOrDefault();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        private void OnTimer(object state)
        {
            var auction = (Auction) state;

            _notificationPublisher.PublishMessageToUser(new Notification
            {
                Event = NotificationEvents.AuctionFinished,
                Data = $"Auction {auction.Id} has finished",
            });

            var nextAuctionToEnd = GetFirstAuctionToEnd();

            _timer = null;
            
            if (nextAuctionToEnd == null) return;
            
            _timer = new Timer(OnTimer, nextAuctionToEnd, nextAuctionToEnd.EndDate - DateTime.UtcNow,
                Timeout.InfiniteTimeSpan);
        }
    }
}