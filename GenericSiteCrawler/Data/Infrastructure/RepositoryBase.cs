using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GenericSiteCrawler.Data.Infrastructure
{
    public abstract class RepositoryBase<T> where T : class
    {
        private CrawlerDBContext _dbContext;
        private readonly IDbSet<T> _dbSet;

        protected IDbFactory DbFactory { get; private set; }

        protected CrawlerDBContext DbContext
        {
            get { return _dbContext ?? (_dbContext = DbFactory.Init()); }
        }

        protected RepositoryBase(IDbFactory dbFactory)
        {
            DbFactory = dbFactory;
            _dbSet = DbContext.Set<T>();
        }

        public virtual void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void Update(T entity)
        {
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = _dbSet.Where(where).AsEnumerable();
            foreach (var obj in objects)
                _dbSet.Remove(obj);
        }

        public virtual T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public T Get(Expression<Func<T, bool>> where)
        {
            return _dbSet.Where(where).FirstOrDefault();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> where)
        {
            return await _dbSet.Where(where).FirstOrDefaultAsync();
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return _dbSet.Where(where).ToList();
        }

        public virtual async Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> where)
        {
            return await _dbSet.Where(where).ToListAsync();
        }
    }
}
