using Domain.Entities;
using Application.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface ICustomerRepository : IBaseRepository<Customer>
    {
        public Customer GetByEmail(string email);
        public void AddOrder(string customerId, Order order);
    }
}
