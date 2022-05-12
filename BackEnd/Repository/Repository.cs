using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class Repository<T> : IRepository<T> where T :class, IEntity
    {
        private readonly AuctionContext DbContext;

        public Repository(AuctionContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<T> GetById(int id)
        {
            var dbSet = DbContext.Set<T>();
            return await dbSet.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            var dbSet = DbContext.Set<T>();
            return await dbSet.ToListAsync();
        }

        public async Task<bool> CheckIfExist(int id)
        {
            var dbSet = DbContext.Set<T>();
            return await dbSet.AnyAsync(u => u.Id == id);
        }

        public async Task<T> AddAsync(T newEntity)
        {
            var dbSet = DbContext.Set<T>();
            var x=await dbSet.AddAsync(newEntity);
            return x.Entity;
        }

        public void RemoveEntity(T entity)
        {
            var dbSet = DbContext.Set<T>();
            dbSet.Remove(entity);
        }

        public void Update(T entity)
        {
            var dbSet = DbContext.Set<T>();
            dbSet.Update(entity);
        }

        public async Task AddRange(IEnumerable<T> newEntities)
        {
            var dbSet = DbContext.Set<T>();
            await dbSet.AddRangeAsync(newEntities);
        }

        public async Task SaveChanges()
        {
            await DbContext.SaveChangesAsync();
        }
    }
}