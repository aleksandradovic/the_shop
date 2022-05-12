using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Order
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public double TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }

        public Customer Customer { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

    }
}
