using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int InventoryId { get; set; }
        public int ArticleId { get; set; }
        public double Price { get; set; }
        public long Quantity { get; set; }

        public Inventory Inventory { get; set; }
    }
}
