using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain;

namespace Repository
{
    public interface IRepository<T> where T : class, IEntity
    {
        IQueryable<T> FilterBy(Expression<Func<T, bool>> filterExpression,
            Expression<Func<T, object>>[]? propertiesToIncludes = null);
        IEnumerable<TProjected> FilterBy<TProjected>(
            Expression<Func<T, bool>> filterExpression,
            Expression<Func<T, TProjected>> projectionExpression,
            Expression<Func<T, object>>[]? propertiesToIncludes = null);
        T FindOne(Expression<Func<T, bool>> filterExpression, Expression<Func<T, object>>[]? propertiesToIncludes = null);
        Task<T> FindOneAsync(Expression<Func<T, bool>> filterExpression, Expression<Func<T, object>>[]? propertiesToIncludes = null);
        IQueryable<T> AsQueryable(Expression<Func<T, object>>[]? propertiesToIncludes = null);
        T FindById(Guid id, Expression<Func<T, object>>[]? propertiesToIncludes = null);
        Task<T> FindByIdAsync(Guid id, Expression<Func<T, object>>[]? propertiesToIncludes = null);
        void InsertOne(T entity);
        Task InsertOneAsync(T entity);
        void ReplaceOne(T document);
        Task ReplaceOneAsync(T document);
        void SaveChanges();
        Task SaveChangesAsync();
    }
}