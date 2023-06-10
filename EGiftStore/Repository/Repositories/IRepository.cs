using Microsoft.EntityFrameworkCore;
using Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public interface IRepository<T> where T : class
    {
        public IQueryable<T> GetAll();

        public IQueryable<T> GetEntitiesPredicate(Expression<Func<T, bool>> expression);

        public Task AddAsync(T entity);

        public void Update(T entity);

        public void Remove(T entity);

        public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression);

        public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, Object>> include);

        public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, Object>> include1, Expression<Func<T, Object>> include2);

    }
}
