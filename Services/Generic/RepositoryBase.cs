using Contracts.Generic;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Services.Generic
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected RepositoryContext _RepositoryContext { get; set; }
        public RepositoryBase(RepositoryContext repositoryContext)
        {
            this._RepositoryContext = repositoryContext;
        }
        public void create(T entity)
        {
            this._RepositoryContext.Set<T>().Add(entity);
            _RepositoryContext.SaveChanges();
        }

        public void delete(T entity)
        {
            this._RepositoryContext.Set<T>().Remove(entity);
        }
        public T find(object id)
        {
            return this._RepositoryContext.Find<T>(id);
        }

        public IQueryable<T> findAll()
        {
            return this._RepositoryContext.Set<T>().AsNoTracking();
        }
        public IQueryable<T> findAll(string toInclude)
        {
            return this._RepositoryContext.Set<T>().Include(toInclude).AsNoTracking();

        }

        public IQueryable<T> findByCondition(Expression<Func<T, bool>> expression)
        {
            return this._RepositoryContext.Set<T>().Where(expression).AsNoTracking();
        }
        public IQueryable<T> findByCondition(Expression<Func<T, bool>> expression,string toInclude)
        {
            return this._RepositoryContext.Set<T>().Include(toInclude).Where(expression).AsNoTracking();
        }

        public IQueryable<T> findByCondition(Expression<Func<T, bool>> expression, string[] toInclude)
        {
            var cantidadInclude = toInclude.Count();
            if (cantidadInclude == 1)
            {
                return this._RepositoryContext.Set<T>().Include(toInclude[0]).Where(expression).AsNoTracking();
            }
            if (cantidadInclude == 2)
            {
                return this._RepositoryContext.Set<T>().Include(toInclude[0]).Include(toInclude[1]).Where(expression).AsNoTracking();
            }
            return this._RepositoryContext.Set<T>().Include(toInclude[0]).Where(expression).AsNoTracking();

        }

        public void update(T entity)
        {
            this._RepositoryContext.Set<T>().Update(entity);
            _RepositoryContext.SaveChanges();
        }
    }
}
