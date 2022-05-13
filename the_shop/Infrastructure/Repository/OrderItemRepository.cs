using Application.Repositories;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly InMemoryDbContext _context;
        private readonly ILogger<OrderItemRepository> _logger;

        public OrderItemRepository(InMemoryDbContext context, ILogger<OrderItemRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public OrderItem Add(OrderItem orderItem)
        {
            orderItem.Id = Guid.NewGuid().ToString();
            _context.OrderItems.Add(orderItem);
            return orderItem;
        }

        public List<OrderItem> GetAll()
        {
            return _context.OrderItems.ToList();
        }

        public OrderItem GetById(string id)
        {
            return _context.OrderItems.FirstOrDefault(oi => oi.Id == id);
        }

        public void Remove(string id)
        {
            var orderItem = _context.OrderItems.FirstOrDefault(i => i.Id == id);

            if (orderItem != null)
            {
                _context.OrderItems.Remove(orderItem);
            }
            else
            {
                _logger.LogInformation($"Failed deleting order item. Order item with id {id} does not exist.");
            }
        }
    }
}
