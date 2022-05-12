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
    public class OrderRepository : IOrderRepository
    {
        private readonly InMemoryDbContext _context;

        public OrderRepository(InMemoryDbContext context)
        {
            _context = context;
        }

        public void Add(Order order)
        {
            _context.Orders.Add(order);
        }

        public List<Order> GetAll()
        {
            return _context.Orders.ToList();
        }

        public List<Order> GetByCustomerId(int customerId)
        {
            return _context.Orders.Where(o => o.CustomerId == customerId).ToList();
        }

        public Order GetById(int id)
        {
            return _context.Orders.FirstOrDefault(o => o.Id == id);
        }

        public void Remove(int id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == id);

            if(order != null)
            {
                _context.Orders.Remove(order);
            }
        }
    }
}
