using ECommerse.Common.Interface;
using ECommerse.Infrastracture.Interface;
using ECommerse.Core.Entities;
using ECommerse.Common.DTOs;

namespace ECommerse.Common.Logic
{
    public class ProductService : IProductService
    {
        private readonly IGenaricRepository<Product> _productRepository;
       public ProductService(IGenaricRepository<Product> productRepository)
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