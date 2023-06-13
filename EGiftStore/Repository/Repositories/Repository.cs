using Microsoft.EntityFrameworkCore;
using Persistence.Entities;
using System.Linq.Expressions;
namespace Repository.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbSet<T> _entity;

        public Repository(EgiftShopContext egiftShop)
        {
            _entity = egiftShop.Set<T>();
        }
        public IQueryable<T> GetAll()
        {
            return _entity;
        }
        public IQueryable<T> GetEntitiesPredicate(Expression<Func<T, bool>> expression)
        {
            return _entity.Where(expression);
        }
        public async Task AddAsync(T entity)
        {
            await _entity.AddAsync(entity);
        }
        public void Update(T entity)
        {
            _entity.Update(entity);
        }
        public void Remove(T entity)
        {
            _entity.Remove(entity);
        }
        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression)
        {
            return await _entity.FirstOrDefaultAsync(expression) ?? null!;
        }
        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, Object>> include)
        {
            return await _entity.Include(include).FirstOrDefaultAsync(predicate);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, Object>> include1, Expression<Func<T, Object>> include2)
        {
            return await _entity.Include(include1).Include(include2).FirstOrDefaultAsync(predicate);
        }
        public void RemoveRange(IEnumerable<T> entities)
        {
            _entity.RemoveRange(entities);
        }

        public IQueryable<T> GetEntitiesPredicate(Expression<Func<T, bool>> predicate, Expression<Func<T, Object>> include)
        {
            return _entity.Include(predicate).Where(predicate);
        }
    }
}
