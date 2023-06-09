using Microsoft.EntityFrameworkCore;
using Persistence.Entities;
using System.Linq.Expressions;
namespace Repository.Repositories
{
    public class Repository<T> where T : class
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
    }
}
