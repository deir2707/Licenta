using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace Repository
{
    public interface IEntityFrameworkRepository<T> where T : class, IEntity
    {
        public Task<T> GetById(Guid id);
        public Task<IEnumerable<T>> GetAll();
        public Task<bool> CheckIfExist(Guid id);
        public Task<T> AddAsync(T newEntity);
        public void RemoveEntity(T entity);
        public void Update(T entity);
        public Task AddRange(IEnumerable<T> newEntities);
        public Task SaveChanges();
    }
}