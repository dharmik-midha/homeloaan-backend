using DataAccess.DBContext;
using DataAccess.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DataAccess.Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected DatabaseContext _context;
        public RepositoryBase(DatabaseContext context)
        {
            _context = context;
        }

        public T GetByKey(string key) => _context.Set<T>().Find(key);

        public IQueryable<T> FindAll(bool trackChanges) =>
            !trackChanges ?
              _context.Set<T>()
                .AsNoTracking() :
              _context.Set<T>();

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) =>
            !trackChanges ?
              _context.Set<T>()
                .Where(expression)
                .AsNoTracking() :
              _context.Set<T>()
                .Where(expression);

        public void Create(T entity) => _context.Set<T>().Add(entity);

        public void Update(T entity) => _context.Set<T>().Update(entity);

        public void Delete(T entity) => _context.Set<T>().Remove(entity);
    }
}