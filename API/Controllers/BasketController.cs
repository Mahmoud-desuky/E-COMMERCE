using ECommerce.Core.Entities;
using ECommerce.Infrastructure.Interface;
using ECommerce.Common.Interface;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;

        public BasketController(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);
            return Ok(basket ?? new CustomerBasket(id));
        }
        [HttpPut]
        public async Task<IActionResult> Update(CustomerBasket basket)
        {
            var updateBasket = await _basketRepository.UpdateBasketAsync(basket);

            return Ok(updateBasket);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            return Ok(_basketRepository.DeleteBasketAsync(id));
        }
    }
}