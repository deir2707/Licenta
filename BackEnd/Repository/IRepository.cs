using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace Repository
{
    public abstract class IRepository<T> where T : class, IEntity
    {
        private readonly AuctionContext _dbContext;

        protected IRepository(AuctionContext dbContext)
        {
            _dbContext = dbContext;
        }

        public AuctionContext DbContext => _dbContext;
        public abstract Task<T> GetById(int id);
        public abstract Task<IEnumerable<T>> GetAll();
        public abstract Task<bool> CheckIfExist(int id);
        public abstract Task<T> AddAsync(T newEntity);
        public abstract void RemoveEntity(T entity);
        public abstract void Update(T entity);
        public abstract Task AddRange(IEnumerable<T> newEntities);
        public abstract Task SaveChanges();
    }
}