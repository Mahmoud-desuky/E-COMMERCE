using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ECommerce.Common.Interface;
using ECommerce.Core.Entities;
using ECommerce.Core.Entities.Identity;
using ECommerce.Infrastructure.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Authorize]
    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly UserManager<User> _userRepository;
        public BasketController(IBasketRepository basketRepository,
        UserManager<User> userRepository)
        {
            _basketRepository = basketRepository;
            _userRepository = userRepository;
        }

        [HttpGet()]
        public async Task<ActionResult<CustomerBasket>> GetBasket()
        {
            var customerId = GetCustomerId();
            var findCurrentUser=_userRepository.FindByIdAsync(customerId);
            if(findCurrentUser==null)
            {
                return Unauthorized("User is not active.");
            }
            var basket = await _basketRepository.GetBasketAsync(customerId);

            if (basket == null)
            {
                return Ok(new CustomerBasket(customerId)
                {
                    CustomerId = customerId
                });
            }
            return Ok(basket);
        }

        [HttpPut]
        public async Task<ActionResult<CustomerBasket>> Update(CustomerBasket basket)
        {
            var customerId = GetCustomerId();
            basket.CustomerId = customerId;
            basket.Id = customerId;

            var updatedBasket = await _basketRepository.UpdateBasketAsync(basket);

            if (updatedBasket == null)
                return BadRequest("Failed to update basket.");

            return Ok(updatedBasket);
        }

        [HttpDelete()]
        public async Task<IActionResult> Delete()
        {
            var customerId = GetCustomerId();
            var basket = await _basketRepository.GetBasketAsync(customerId);

            if (basket == null)
                return NotFound();

            if (basket.CustomerId != GetCustomerId())
                return Unauthorized();

            var deleted = await _basketRepository.DeleteBasketAsync(customerId);

            if (!deleted)
                return NotFound();

            return NoContent();
        }

       
    }
}