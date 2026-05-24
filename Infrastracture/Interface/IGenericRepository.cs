using System.Linq.Expressions;
using ECommerce.Core.Entities;

namespace ECommerce.Infrastructure.Interface
{
    public interface IGenericRepository<T> where T : BaseEntity
    {

        Task<T> GetByIdAsync(int id);
        IQueryable<T> GetAllAsync();
        IQueryable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string IncludeProperties = "");
        Task<T> GetEntityWithSpec(ISpacification<T> spec);
        Task<IReadOnlyList<T>> ListAsync(ISpacification<T> spec);
        Task<bool> Delete(int id);
        Task<T> Update(T entity);
        Task<T> AddAsync(T entity);
        
    }
}
