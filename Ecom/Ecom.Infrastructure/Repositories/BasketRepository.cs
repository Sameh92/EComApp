using AutoMapper;
using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Ecom.Helper.Dtos;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
   

        public BasketRepository(IConnectionMultiplexer redis)
        {
            _database=redis.GetDatabase();
           
        }
        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            var check= await _database.KeyExistsAsync(basketId);
            if(check)
            {
                return await _database.KeyDeleteAsync(basketId);
            }
            return false;
        }

        public async Task<CustomerBasket> GetBasketAsync(string basketId)
        {
            var data = await _database.StringGetAsync(basketId);
            var res= data.IsNullOrEmpty ? null:JsonSerializer.Deserialize<CustomerBasket>(data);
            return res;
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasketDto customerbasket)
        {
            var _basket = await _database.StringSetAsync(customerbasket.Id, JsonSerializer.Serialize(customerbasket), TimeSpan.FromDays(30));

            if (!_basket) return null;
            
            return await GetBasketAsync(customerbasket.Id);
        }
    }
}
