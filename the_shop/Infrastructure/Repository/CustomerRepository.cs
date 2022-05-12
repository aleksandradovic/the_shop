using Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Repositories;
using Domain.Entities;

namespace Infrastructure.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly InMemoryDbContext _context;

        public CustomerRepository(InMemoryDbContext context)
        {
            _context = context;
        }

        public void Add(Customer customer)
        {
            _context.Customers.Add(customer);
        }

        public void AddOrder(int customerId, Order order)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.Id == customerId);

            if(customer != null && order != null)
            {
                customer.Orders.Add(order);
            }
        }

        public List<Customer> GetAll()
        {
            return _context.Customers.ToList();
        }

        public Customer GetByEmail(string email)
        {
            return _context.Customers.FirstOrDefault(c => c.Email == email);
        }

        public Customer GetById(int id)
        {
            return _context.Customers.FirstOrDefault(c => c.Id == id);
        }

        public void Remove(int id)
        {
            var customer = _context.Customers.FirstOrDefault(x => x.Id == id);

            if(customer != null)
            {
                _context.Customers.Remove(customer);
            }
        }
    }
}
