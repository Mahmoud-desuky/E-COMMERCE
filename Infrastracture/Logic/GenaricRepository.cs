using System.Linq.Expressions;
using ECommerse.Infrastracture.Data;
using ECommerse.Core.Entities;
using ECommerse.Infrastracture.Data;
using ECommerse.Infrastracture.Interface;
using Microsoft.EntityFrameworkCore;

namespace ECommerse.Infrastracture.Logic
{
    public class GenaricRepository<T> : IGenaricRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _context;
        private DbSet<T> _dbSet;
        public GenaricRepository(StoreContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
          public async Task<T> GetByIdAsync(int id)
            {
                 return await _dbSet.FindAsync(id);
            }
        public virtual IQueryable<T> GetAllAsync()
            {
                return _dbSet.AsQueryable<T>();
            }

        public IQueryable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null
          , string IncludeProperties = "")
        {
            IQueryable<T> query = _dbSet;
            if (filter != null)
            {
                query = query.AsNoTracking().Where(filter);
            }
            foreach (var includeProperty in IncludeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            if (orderBy != null)
            {
                return orderBy(query);
            }
            return query;
        }
        public async Task<T> GetEntityWithSpec(ISpacification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }
        public async Task<IReadOnlyList<T>> ListAsync(ISpacification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }
        private IQueryable<T>ApplySpecification(ISpacification<T> spec)
        {
            return SpacificationEvaluatar<T>.GetQuery(_dbSet.AsQueryable(), spec);
        }

        public async Task<bool> Delete(int id)
        {
            var find=_dbSet.FirstOrDefault(t=>t.Id==id);
            if(find==null)
                return false;
            _dbSet.Remove(find);
            return true;
        }

        public async Task<T> Update(T entity)
        {
            var exist = _dbSet.Find(entity.Id);
            if (exist == null)
                return null;
            _context.Entry(exist).CurrentValues.SetValues(entity);
            return entity;
        }
        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            _context.SaveChanges();
            return entity;
        }
    }
}
