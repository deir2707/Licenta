using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain;
using Infrastructure.Mongo;
using MongoDB.Driver;

namespace Repository
{
    public class MongoRepository<T>: IRepository<T> where T: class, IEntity
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<T> _collection;
        public MongoRepository(IMongoDbSettings  settings)
        {
            _database = new MongoClient(settings.ConnectionString).GetDatabase(settings.DatabaseName);
            var collectionName = GetCollectionName(typeof(T));
            _collection = _database.GetCollection<T>(collectionName);
        }

        public IQueryable<T> AsQueryable(Expression<Func<T, object>>[]? propertiesToIncludes = null)
        {
            return _collection.AsQueryable();
        }

        private string? GetCollectionName(Type documentType) =>
            ((BsonCollectionAttribute) documentType.GetCustomAttributes(
                typeof(BsonCollectionAttribute),
                true).FirstOrDefault()!).CollectionName;

        public IQueryable<T> FilterBy(Expression<Func<T, bool>> filterExpression, Expression<Func<T, object>>[]? propertiesToIncludes = null)
        {
            return _collection.AsQueryable().Where(filterExpression);
        }

        public IEnumerable<TProjected> FilterBy<TProjected>(Expression<Func<T, bool>> filterExpression, Expression<Func<T, TProjected>> projectionExpression, Expression<Func<T, object>>[]? propertiesToIncludes = null)
        {
            return _collection.AsQueryable().Where(filterExpression).Select(projectionExpression);
        }

        public T FindOne(Expression<Func<T, bool>> filterExpression, Expression<Func<T, object>>[]? propertiesToIncludes = null)
        {
            return _collection.Find(filterExpression).FirstOrDefault();
        }

        public async Task<T> FindOneAsync(Expression<Func<T, bool>> filterExpression, Expression<Func<T, object>>[]? propertiesToIncludes = null)
        {
            return await _collection.Find(filterExpression).FirstOrDefaultAsync();
        }

        public T FindById(Guid id, Expression<Func<T, object>>[]? propertiesToIncludes = null)
        {
            var filter = Builders<T>.Filter.Eq(doc => doc.Id, id);
            return _collection.Find(filter).SingleOrDefault();
        }

        public async Task<T> FindByIdAsync(Guid id, Expression<Func<T, object>>[]? propertiesToIncludes = null)
        {
            var filter = Builders<T>.Filter.Eq(doc => doc.Id, id);
            return await _collection.Find(filter).SingleOrDefaultAsync();
        }

        public void InsertOne(T entity)
        {
            _collection.InsertOne(entity);
        }

        public async Task InsertOneAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public void ReplaceOne(T document)
        {
            var filter = Builders<T>.Filter.Eq(doc => doc.Id, document.Id);
            _collection.FindOneAndReplace(filter, document);
        }

        public async Task ReplaceOneAsync(T document)
        {
            var filter = Builders<T>.Filter.Eq(doc => doc.Id, document.Id);
            await _collection.FindOneAndReplaceAsync(filter, document);
        }

        public void SaveChanges()
        {
        }

        public Task SaveChangesAsync()
        {
            return Task.CompletedTask;
        }
    }
}