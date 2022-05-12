using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace Repository
{
    public interface IRepository<T> where T : class, IEntity
    {
        Task<T> GetById(int id);
        Task<IEnumerable<T>> GetAll();
        Task<bool> CheckIfExist(int id);
        Task<T> AddAsync(T newEntity);
        void RemoveEntity(T entity);
        void Update(T entity);
        Task AddRange(IEnumerable<T> newEntities);
        Task SaveChanges();
    }
}