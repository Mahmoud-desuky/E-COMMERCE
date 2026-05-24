using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using ECommerce.Common.DTOs;
using ECommerce.Common.Interface;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductTypeController : ControllerBase
    {
         private readonly IProductTypeService _productTypeService;
        public ProductTypeController(IProductTypeService productTypeService)
        {
            _productTypeService=productTypeService;
            
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] string  typeName)
        {
            
            return Ok(await _productTypeService.AddType(typeName));
        }
    }
}