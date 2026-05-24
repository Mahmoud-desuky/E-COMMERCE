using ECommerce.Common.DTOs;
using ECommerce.Core.Entities;

namespace ECommerce.Common.Interface
{
    public interface IProductTypeService
    {
        public Task<ProductType> AddType(string typeName);
    }
}