using ECommerse.Infrastracture.Interface;
using ECommerse.Core.Entities;
using ECommerse.Common.DTOs;
using ECommerse.Common.Interface;


namespace ECommerse.Common.Logic
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly IGenaricRepository<ProductType> _productTypeRepository;

        public ProductTypeService(IGenaricRepository<ProductType> productTypeRepository)
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