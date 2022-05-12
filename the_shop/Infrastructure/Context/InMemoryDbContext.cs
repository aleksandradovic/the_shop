using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Context
{
    public class InMemoryDbContext
    {
        public List<Article> Articles { get; set; } = new List<Article>();
        public List<Customer> Customers { get; set; } = new List<Customer>();
        public List<Inventory> Inventories { get; set; } = new List<Inventory>();
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public List<Order> Orders { get; set; } = new List<Order>();
        public List<Supplier> Suppliers { get; set; } = new List<Supplier>();
    }
}
