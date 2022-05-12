using Domain.Entities;
using Application.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        public List<Order> GetByCustomerId(int customerId);
    }
}
