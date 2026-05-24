using System.Linq;
using ECommerce.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Data
{
    public class SpecificationEvaluatar<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpacification<T> specification)
        {
            var query = inputQuery;
            
            if (specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
            }
           
            if(specification.OrderBy!=null)
            {
                query=query.OrderBy(specification.OrderBy);

            }
            if(specification.OrderByDesc!=null)
            {
                query=query.OrderByDescending(specification.OrderByDesc);
            }
            query = specification.Includes.Aggregate(query, (current, include) => current.Include(include));
            return query;
        }
    }
}