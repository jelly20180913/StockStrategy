using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
namespace WebApiService.Models.Repository
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        string Create(TEntity instance);

        void Update(TEntity instance);

        void Delete(TEntity instance);

        TEntity Get(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> where);

        IQueryable<TEntity> GetAll();

        void SaveChanges();

        List<string> CreateBatch(List<TEntity> instance);
        IQueryable<TEntity> Filter(string sql);
        DbSet<TEntity> GetDbSet();
        void Execute(string sql);
    }
}
