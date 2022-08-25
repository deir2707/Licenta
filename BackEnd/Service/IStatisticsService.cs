using System.Threading.Tasks;
using Service.Outputs;

namespace Service
{
    public interface IStatisticsService
    {
        public Task<StatisticsOutput> GetStatistics();
    }
}