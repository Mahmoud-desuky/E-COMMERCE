using ECommerce.Common.Interface;
using ECommerce.Infrastructure.Interface;
using ECommerce.Core.Entities;
using ECommerce.Common.DTOs;
using ECommerce.Infrastructure.Data;

namespace ECommerce.Common.Logic
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Product> _productRepository;
       public ProductService(IGenericRepository<Product> productRepository)
        {
            _productRepository = productRepository; 
        
       }
         public async Task<Product> CreateProductAsync(ProductDTO product)
         {
            var newProduct = new Product
            {
                Name= product.Name,
                Description= product.Description,
                Price= product.Price,
                PictureUrl= product.PictureUrl,
            };
              return await _productRepository.AddAsync(newProduct);

         }
    }
}