
using ECommerce.Common.DTOs;
using ECommerce.Core.Entities;


namespace ECommerce.Common.Interface
{
    public interface IProductService
    {
        public Task<Product> CreateProductAsync(ProductDTO product);
      
    }
}