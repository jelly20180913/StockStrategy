using System;
using System.Collections.Generic;
using System.Linq;
using System.Web; 
using System.Data.Entity;
using WebApiService.Models.DbContextFactory;
using System.Data.Entity.Validation;
using System.Linq.Expressions;
using System.Diagnostics;
namespace WebApiService.Models.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private DbContext _context
        {
            get;
            set;
        }
        //if use unity DI framwork and  this block has error ,inject EDIEntities by UnityConfig 
        //public Repository()
        //    : this(new EDIEntities())
        //{
        //}
        //public Repository(DbContext context)
        //{
        //    if (context == null)
        //    {
        //        throw new ArgumentNullException("context");
        //    }
        //    this._context = context;
        //}
        public Repository(IDbContextFactory factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }
            this._context = factory.GetDbContext();
        }
        /// <summary>
        /// Creates the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <exception cref="System.ArgumentNullException">instance</exception>
        public string Create(TEntity instance)
        {
            string _Err = "";
            try
            {
                if (instance == null)
                {
                    throw new ArgumentNullException("instance");
                }
                else
                {
                    this._context.Set<TEntity>().Add(instance);
                    this.SaveChanges();
                }
            }
            catch (DbEntityValidationException ex)
            {
                _Err = ex.EntityValidationErrors.First().ValidationErrors.First().ErrorMessage;
            }
            return _Err;
        }

        /// <summary>
        /// Updates the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Update(TEntity instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }
            else
            {
                this._context.Entry(instance).State = EntityState.Modified;
                this.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Delete(TEntity instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }
            else
            {
                this._context.Entry(instance).State = EntityState.Deleted;
                this.SaveChanges();
            }
        }

        /// <summary>
        /// Gets the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return this._context.Set<TEntity>().FirstOrDefault(predicate);
        }
        public IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> where)
        {
            return this._context.Set<TEntity>().Where(where).AsQueryable();
        }
        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> GetAll()
        {
            return this._context.Set<TEntity>().AsQueryable();
        }


        public void SaveChanges()
        {
            this._context.SaveChanges();
        }
        public void Dispose()

        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// this funtion for batch import excel 
        /// use batch and transaction to improve performance 4000 / 20sec
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public List<string> CreateBatch(List<TEntity> instance)
        {
            Stopwatch sw = new Stopwatch();
            sw.Reset();
            sw.Start();
            List<string> _ListError = new List<string>();
            string _Error = "";
            var batchcount = 100;//you can tune this parameter for better performance 
            int _Leave = instance.Count % batchcount;
            int _N = 0;
            this._context.Configuration.AutoDetectChangesEnabled = false;
            using (var beginTarn = this._context.Database.BeginTransaction())
            {
                try
                {
                    for (int i = 0; i < (instance.Count / batchcount); i++)
                    {
                        //using (EDIEntities db = new EDIEntities())
                        //{

                        for (int j = 0; j < batchcount; j++)
                        {
                            this._context.Set<TEntity>().Add(instance[i * batchcount + j]);
                            //PosData _P = new PosData();
                            //_P.FK_LoginId = 3;
                            //db.PosData.Add(_P);
                        }
                        this._context.SaveChanges();
                        _N++;
                        //}
                    }
                    // int _N = instance.Count / batchcount;
                    if (_Leave > 0)
                    {
                        //using (EDIEntities db = new EDIEntities())
                        //{
                        //    //resolve different database problem
                        //    db.Database.Connection.ConnectionString = this._context.Database.Connection.ConnectionString;
                        //    for (int k = 0; k < _Leave; k++)
                        //    {
                        //        db.Set<TEntity>().Add(instance[_N * batchcount + k]);
                        //    }
                        //    db.SaveChanges();
                        //} 
                        for (int k = 0; k < _Leave; k++)
                        {
                            this._context.Set<TEntity>().Add(instance[_N * batchcount + k]);
                        }
                        this._context.SaveChanges();
                    }
                    beginTarn.Commit();
                }
                catch (DbEntityValidationException ex)
                {
                    _Error = ex.EntityValidationErrors.First().ValidationErrors.First().ErrorMessage;
                    _ListError.Add(_N.ToString() + " batch(hundred) data has error value:" + _Error);
                    beginTarn.Rollback();

                }
            }
            sw.Stop();
            _ListError.Add(sw.ElapsedMilliseconds.ToString());
            return _ListError;
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._context != null)
                {
                    this._context.Dispose();
                    this._context = null;
                }
            }
        }
         
        public IQueryable<TEntity> Filter(string sql)
        {
            return this._context.Set<TEntity>().SqlQuery(sql).AsQueryable();
        }
        public DbSet<TEntity> GetDbSet()
        {
            return this._context.Set<TEntity>();
        }
        public void Execute(string sql)
        {
            this._context.Database.ExecuteSqlCommand(sql); ;
        }
    }
}