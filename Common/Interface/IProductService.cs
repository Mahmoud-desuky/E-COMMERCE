
using ECommerse.Common.DTOs;
using ECommerse.Core.Entities;


namespace ECommerse.Common.Interface
{
    public interface IProductService
    {
        public Task<Product> CreateProductAsync(ProductDTO product);
      
    }
}