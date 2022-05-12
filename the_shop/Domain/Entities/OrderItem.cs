using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class OrderItem
    {
        public string Id { get; set; }
        public string InventoryId { get; set; }
        public string OrderId { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }

        public Inventory Inventory { get; set; }
        public Order Order { get; set; }
    }
}
