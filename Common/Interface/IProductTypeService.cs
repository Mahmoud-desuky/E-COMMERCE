using ECommerse.Common.DTOs;
using ECommerse.Core.Entities;

namespace ECommerse.Common.Interface
{
    public interface IProductTypeService
    {
        public Task<ProductType> AddType(string typeName);
    }
}