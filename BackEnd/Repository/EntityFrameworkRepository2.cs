using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class EntityFrameworkRepository2<T> : IEntityFrameworkRepository<T> where T :class, IEntity
    {
        private readonly AuctionContext _dbContext;

        public AuctionContext DbContext => _dbContext;
        
        public EntityFrameworkRepository2(AuctionContext dbContext)
        {
            _dbContext = dbContext;
        }
        

        public async Task<T> GetById(Guid id)
        {
            var dbSet = _dbContext.Set<T>();
            return await dbSet.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            var dbSet = _dbContext.Set<T>();
            return await dbSet.ToListAsync();
        }

        public async Task<bool> CheckIfExist(Guid id)
        {
            var dbSet = _dbContext.Set<T>();
            return await dbSet.AnyAsync(u => u.Id == id);
        }

        public async Task<T> AddAsync(T newEntity)
        {
            var dbSet = _dbContext.Set<T>();
            var x=await dbSet.AddAsync(newEntity);
            return x.Entity;
        }

        public void RemoveEntity(T entity)
        {
            var dbSet = _dbContext.Set<T>();
            dbSet.Remove(entity);
        }

        public void Update(T entity)
        {
            var dbSet = _dbContext.Set<T>();
            dbSet.Update(entity);
        }

        public async Task AddRange(IEnumerable<T> newEntities)
        {
            var dbSet = _dbContext.Set<T>();
            await dbSet.AddRangeAsync(newEntities);
        }

        public async Task SaveChanges()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}