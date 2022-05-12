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
    public class OrderRepository : IOrderRepository
    {
        private readonly InMemoryDbContext _context;
        private readonly ILogger<OrderRepository> _logger;

        public OrderRepository(InMemoryDbContext context, ILogger<OrderRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Order Add(Order order)
        {
            order.Id = Guid.NewGuid().ToString();
            _context.Orders.Add(order);
            return order; 
        }

        public List<Order> GetAll()
        {
            return _context.Orders.ToList();
        }

        public List<Order> GetByCustomerId(string customerId)
        {
            return _context.Orders.Where(o => o.CustomerId == customerId).ToList();
        }

        public Order GetById(string id)
        {
            return _context.Orders.FirstOrDefault(o => o.Id == id);
        }

        public void Remove(string id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == id);

            if(order != null)
            {
                _context.Orders.Remove(order);
            }
            else
            {
                _logger.LogInformation($"Failed deleting order. Order with id {id} does not exist.");
            }
        }
    }
}
