using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class EntityFrameworkRepository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly AuctionContext _context;

        public EntityFrameworkRepository(AuctionContext context)
        {
            _context = context;
        }

        public IQueryable<T> AsQueryable(Expression<Func<T, object>>[]? propertiesToIncludes = null)
        {
            var query = GetDbSetForEntity();

            propertiesToIncludes ??= Array.Empty<Expression<Func<T, object>>>();

            foreach (var property in propertiesToIncludes)
            {
                if (TryParseNavigationPath(property.Body, out var include))
                    query = query.Include(include);
            }

            return query;
        }
        private IQueryable<T> GetDbSetForEntity()
        {
            return _context.Set<T>();
        }

        public IQueryable<T> FilterBy(Expression<Func<T, bool>> filterExpression,
            Expression<Func<T, object>>[]? propertiesToIncludes = null)
        {
            return AsQueryable(propertiesToIncludes).Where(filterExpression);
        }

        public IEnumerable<TProjected> FilterBy<TProjected>(Expression<Func<T, bool>> filterExpression,
            Expression<Func<T, TProjected>> projectionExpression,
            Expression<Func<T, object>>[]? propertiesToIncludes = null)
        {
            return AsQueryable(propertiesToIncludes).Where(filterExpression).Select(projectionExpression);
        }

        public T FindOne(Expression<Func<T, bool>> filterExpression,
            Expression<Func<T, object>>[]? propertiesToIncludes = null)
        {
            return AsQueryable(propertiesToIncludes).FirstOrDefault(filterExpression);
        }

        public async Task<T> FindOneAsync(Expression<Func<T, bool>> filterExpression,
            Expression<Func<T, object>>[]? propertiesToIncludes = null)
        {
            return await AsQueryable(propertiesToIncludes).FirstOrDefaultAsync(filterExpression);
        }

        public T FindById(Guid id, Expression<Func<T, object>>[]? propertiesToIncludes = null)
        {
            return AsQueryable(propertiesToIncludes).FirstOrDefault(x => x.Id == id);
        }

        public async Task<T> FindByIdAsync(Guid id, Expression<Func<T, object>>[]? propertiesToIncludes = null)
        {
            return await AsQueryable(propertiesToIncludes).FirstOrDefaultAsync(x => x.Id == id);
        }

        public void InsertOne(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }

        public async Task InsertOneAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public void ReplaceOne(T document)
        {
            var entry = FindById(document.Id);
            _context.Entry(entry).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public async Task ReplaceOneAsync(T document)
        {
            var entry = await FindByIdAsync(document.Id);
            _context.Entry(entry).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        private static bool TryParseNavigationPath(Expression expression, out string path)
        {
            path = null;

            if (IsMemberExpression(expression, out var memberExpression))
            {
                var childPart = memberExpression.Member.Name;

                if (!TryParseNavigationPath(memberExpression.Expression, out var parentPart))
                    return false;

                path = parentPart == null ? childPart : ComputePath(parentPart, childPart);
            }

            if (IsMethodCallExpression(expression, out var callExpression))
            {
                if (!IsSelectWithTwoLevelDepth(callExpression))
                    return false;

                if (!TryParseNavigationPath(callExpression.Arguments[0], out var parentPart))
                    return false;

                if (parentPart == null)
                    return false;

                var subExpression = callExpression.Arguments[1] as LambdaExpression;

                if (subExpression == null)
                    return false;

                if (!TryParseNavigationPath(subExpression.Body, out var childPart))
                    return false;


                if (childPart == null)
                    return false;

                path = ComputePath(parentPart, childPart);

                return true;
            }

            return true;
        }

        private static bool IsMemberExpression(Expression expression, out MemberExpression memberExpression)
        {
            memberExpression = expression as MemberExpression;
            return memberExpression != null;
        }

        private static bool IsMethodCallExpression(Expression expression, out MethodCallExpression methodCallExpression)
        {
            methodCallExpression = expression as MethodCallExpression;
            return methodCallExpression != null;
        }

        private static string ComputePath(string parentPart, string thisPart)
        {
            return parentPart + "." + thisPart;
        }

        private static bool IsSelectWithTwoLevelDepth(MethodCallExpression callExpression)
        {
            return callExpression.Method.Name == "Select" && callExpression.Arguments.Count == 2;
        }
    }
}