using ECommerse.Core.Entities;
using ECommerse.Infrastracture.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerse.Common.DTOs;
using ECommerse.Common.Interface;

namespace ECommerse.API.Controllers
{
    public class ProductController : BaseApiController
    {
        private readonly IGenaricRepository<Product> _productRepository;
        private readonly IProductService _productService;
        public ProductController(IGenaricRepository<Product> productRepository,
        IProductService productService)
            {
                _productService=productService;
                _productRepository = productRepository;
            }
        [HttpGet("id")]
        public async Task<IActionResult> GetById (int Id)
        {
            return Ok(await _productRepository.GetByIdAsync(Id));
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _productRepository.GetAllAsync().ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductDTO product)
        {
            
            return Ok(await _productService.CreateProductAsync(product));
        }

    }
}
