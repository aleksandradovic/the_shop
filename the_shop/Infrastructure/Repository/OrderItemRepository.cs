using Application.Repositories;
using Domain.Entities;
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

        public OrderItemRepository(InMemoryDbContext context)
        {
            _context = context;
        }

        public void Add(OrderItem orderItem)
        {
            _context.OrderItems.Add(orderItem);
        }

        public List<OrderItem> GetAll()
        {
            return _context.OrderItems.ToList();
        }

        public OrderItem GetById(int id)
        {
            return _context.OrderItems.FirstOrDefault(oi => oi.Id == id);
        }

        public void Remove(int id)
        {
            var orderItem = _context.OrderItems.FirstOrDefault(i => i.Id == id);

            if (orderItem != null)
            {
                _context.OrderItems.Remove(orderItem);
            }
        }
    }
}
