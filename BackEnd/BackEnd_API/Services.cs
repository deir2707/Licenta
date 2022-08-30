using Domain;
using Microsoft.Extensions.DependencyInjection;
using Repository;
using Service;

namespace BackEnd
{
    public static class Services
    {
        public static IServiceCollection AddAppDependencies(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IUserService, UserService>();
            serviceCollection.AddTransient<IAuctionService, AuctionService>();
            serviceCollection.AddTransient<IStatisticsService, StatisticsService>();
            return serviceCollection;
        }
        
        public static IServiceCollection AddRepoDependencies(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IRepository<User>, EntityFrameworkRepository<User>>();
            serviceCollection.AddTransient<IRepository<Auction>, EntityFrameworkRepository<Auction>>();
            serviceCollection.AddTransient<IRepository<Bid>, EntityFrameworkRepository<Bid>>();
            serviceCollection.AddTransient<IRepository<Image>, EntityFrameworkRepository<Image>>();
            return serviceCollection;
        }
    }
}