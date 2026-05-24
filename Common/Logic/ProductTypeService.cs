using ECommerce.Infrastructure.Interface;
using ECommerce.Core.Entities;
using ECommerce.Common.DTOs;
using ECommerce.Common.Interface;


namespace ECommerce.Common.Logic
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly IGenericRepository<ProductType> _productTypeRepository;

        public ProductTypeService(IGenericRepository<ProductType> productTypeRepository)
        {
            _productTypeRepository=productTypeRepository;
        }
        public async Task<ProductType> AddType(string typeName)
        {
            var productBrand=new ProductType
            {
                Name=typeName
            };
            return await _productTypeRepository.AddAsync(productBrand);
        }
    }

    
}