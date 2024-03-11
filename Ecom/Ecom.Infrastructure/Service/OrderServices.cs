using Ecom.Core.Entities.Order;
using Ecom.Core.Interfaces;
using Ecom.Core.Service;
using Ecom.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Service
{
    public class OrderServices : IOrderServices
    {
        private readonly IUnitOfWork _uow;
        private readonly ApplicationDbContext _db;

        public OrderServices(IUnitOfWork uow,ApplicationDbContext db)
        {
            _uow = uow;
            _db = db;
        }
        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, ShipAddress shipAddress)
        {
            var basket = await _uow.BasketRepository.GetBasketAsync(basketId);
            var items= new List<OrderItem>();

            //fill item
            foreach (var item in basket.BasketItems)
            {
                var productItem = await _uow.ProductRepository.GetByIdAsync(item.Id);
                var productItemOrderd = new ProductItemOrderd(productItem.Id, productItem.Name, productItem.ProductPicture);
                var orderItem = new OrderItem(productItemOrderd, item.Price, item.Quantity);
                items.Add(orderItem);
            }
            //await _db.OrderItems.AddRangeAsync(items);
            //await _db.SaveChangesAsync();

            //Parallel.ForEach(basket.BasketItems, item =>
            //{
            //    var productItem =  _uow.ProductRepository.GetByIdAsync(item.Id).GetAwaiter().GetResult();
            //    var productItemOrderd = new ProductItemOrderd(productItem.Id, productItem.Name, productItem.ProductPicture);
            //    var orderItem = new OrderItem(productItemOrderd, item.Price, item.Quantity);
            //    lock (items)
            //    {
            //        items.Add(orderItem);
            //    }
            //});

            await _db.OrderItems.AddRangeAsync(items);
            await _db.SaveChangesAsync();


            var devliveryMethod = await _db.DeliveryMethods.Where(x => x.Id == deliveryMethodId).FirstOrDefaultAsync();
            var subTotal= items.Sum(x=>x.Price * x.Quantity);
            var order = new Order(buyerEmail,shipAddress,devliveryMethod,items, subTotal);

            if (order is null) return null;
            
            await _db.Orders.AddAsync(order);
            await _db.SaveChangesAsync();
          
            await _uow.BasketRepository.DeleteBasketAsync(basketId);

            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
           return await _db.DeliveryMethods.ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var order = await _db.Orders
                      .Where(x => x.Id == id && x.BuyerEmail == buyerEmail)
                      .Include(x => x.OrderItems).ThenInclude(x => x.ProductItemOrderd)
                      .Include(x => x.DeliveryMethod)
                      .OrderByDescending(x => x.OrderDate)
                      .FirstOrDefaultAsync();

            return order;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {          
            var orders = await _db.Orders
                    .Where(x => x.BuyerEmail == buyerEmail)
                    .Include(x => x.OrderItems).ThenInclude(x => x.ProductItemOrderd)
                    .Include(x => x.DeliveryMethod)
                    .OrderByDescending(x => x.OrderDate)
                    .ToListAsync();
            return orders;
        }
    }
}
