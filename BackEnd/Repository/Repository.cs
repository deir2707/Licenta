using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class Repository<T> : IRepository<T> where T :class, IEntity
    {
        private readonly AuctionContext _dbContext;

        public AuctionContext DbContext => _dbContext;
        
        public Repository(AuctionContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        

        public override async Task<T> GetById(int id)
        {
            var dbSet = _dbContext.Set<T>();
            return await dbSet.FirstOrDefaultAsync(u => u.Id == id);
        }

        public override async Task<IEnumerable<T>> GetAll()
        {
            var dbSet = _dbContext.Set<T>();
            return await dbSet.ToListAsync();
        }

        public override async Task<bool> CheckIfExist(int id)
        {
            var dbSet = _dbContext.Set<T>();
            return await dbSet.AnyAsync(u => u.Id == id);
        }

        public override async Task<T> AddAsync(T newEntity)
        {
            var dbSet = _dbContext.Set<T>();
            var x=await dbSet.AddAsync(newEntity);
            return x.Entity;
        }

        public override void RemoveEntity(T entity)
        {
            var dbSet = _dbContext.Set<T>();
            dbSet.Remove(entity);
        }

        public override void Update(T entity)
        {
            var dbSet = _dbContext.Set<T>();
            dbSet.Update(entity);
        }

        public override async Task AddRange(IEnumerable<T> newEntities)
        {
            var dbSet = _dbContext.Set<T>();
            await dbSet.AddRangeAsync(newEntities);
        }

        public override async Task SaveChanges()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}