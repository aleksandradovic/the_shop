using Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Repositories;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly InMemoryDbContext _context;
        private readonly ILogger<CustomerRepository> _logger;

        public CustomerRepository(InMemoryDbContext context, ILogger<CustomerRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Customer Add(Customer customer)
        {
            customer.Id = Guid.NewGuid().ToString();
            _context.Customers.Add(customer);
            return customer;
            
        }

        public void AddOrder(string customerId, Order order)
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

        public Customer GetById(string id)
        {
            return _context.Customers.FirstOrDefault(c => c.Id == id);
        }

        public void Remove(string id)
        {
            var customer = _context.Customers.FirstOrDefault(x => x.Id == id);

            if(customer != null)
            {
                _context.Customers.Remove(customer);
            }
            else
            {
                _logger.LogInformation($"Failed deleting customer. Customer with id {id} does not exist.");
            }
        }
    }
}
