using System;
using BackEnd;
using BackEnd.Controllers;
using Infrastructure.Notifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Repository;
using Service;

namespace Tests
{
    public class BaseTest
    {
        private readonly ServiceCollection _serviceCollection;
        public readonly ServiceProvider ServiceProvider;
        public AuctionContext AuctionContext;

        public Mock<INotificationPublisher> NotificationPublisherMock { get; set; }
        public Mock<ICurrentUserProvider> CurrentUserProviderMock { get; set; }

        protected BaseTest()
        {
            _serviceCollection = new ServiceCollection();
            _serviceCollection.AddTransient<IUserService, UserService>();
            _serviceCollection.AddTransient<IAuctionService, AuctionService>();
            _serviceCollection.AddTransient<IStatisticsService, StatisticsService>();
            
            _serviceCollection
                .AddRepoDependencies()
                .AddAppDependencies();

            AddMocks(_serviceCollection);

            ServiceProvider = _serviceCollection.BuildServiceProvider();
            AuctionContext = ServiceProvider.GetService<AuctionContext>();
            
            // RegisterControllers(_serviceCollection);
        }

        private void AddMocks(ServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<AuctionContext>(opt =>
                opt.UseInMemoryDatabase(Guid.NewGuid().ToString()).EnableSensitiveDataLogging());

            NotificationPublisherMock = new Mock<INotificationPublisher>();
            serviceCollection.AddTransient(x => NotificationPublisherMock.Object);

            CurrentUserProviderMock = new Mock<ICurrentUserProvider>();
            serviceCollection.AddTransient(x => CurrentUserProviderMock.Object);
        }

        private static void RegisterControllers(ServiceCollection services)
        {
            services.Scan(scan => scan.FromAssemblyOf<AuctionsController>()
                .AddClasses(x => x.AssignableTo<ControllerBase>()).AsSelf());
        }

        public T Controller<T>() where T : ControllerBase
        {
            return ServiceProvider.GetService<T>();
        }

        public T Service<T>()
        {
            return ServiceProvider.GetService<T>();
        }
    }
}