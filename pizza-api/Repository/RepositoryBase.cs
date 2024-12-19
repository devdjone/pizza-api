using Microsoft.EntityFrameworkCore;
using pizza_api.Data;
using System.Linq.Expressions;

namespace pizza_api.Repository
{
    public abstract class RepositoryBase<T> where T : class
    {
        protected readonly pizza_apiContext _context;
        protected readonly DbSet<T> _dbSet;

        public RepositoryBase(pizza_apiContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _dbSet.AsNoTracking().ToList();
        }

        public virtual T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).AsNoTracking().ToList();
        }

        public virtual void Add(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbSet.Add(entity);
        }

        public virtual void AddRange(IEnumerable<T> entities)
        {
            if (entities == null || !entities.Any())
            {
                throw new ArgumentException("Entities collection cannot be null or empty.", nameof(entities));
            }

            _dbSet.AddRange(entities);
        }

        public virtual void Remove(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbSet.Remove(entity);
        }

        public virtual void RemoveRange(IEnumerable<T> entities)
        {
            if (entities == null || !entities.Any())
            {
                throw new ArgumentException("Entities collection cannot be null or empty.", nameof(entities));
            }

            _dbSet.RemoveRange(entities);
        }

        public virtual void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
