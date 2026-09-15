using System.Text.Json;
using ECommerce.Core.Entities;
using ECommerce.Infrastructure.Interface;
using ECommerce.Common.Interface;
using StackExchange.Redis;


namespace ECommerce.Common.Logic
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task<bool> DeleteBasketAsync(string BasketId)
        {
            return await _database.KeyDeleteAsync(BasketId);
        }

        public async Task<CustomerBasket?> GetBasketAsync(string BasketId)
        {
            var data = await _database.StringGetAsync(BasketId);
            
            return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(data);
        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
        {
            
            var updated = await _database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(30));
            if (!updated)
                return null;
            return await GetBasketAsync(basket.Id);
        }
    }
}