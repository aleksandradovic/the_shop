using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Inventory
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public int SupplierId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }

        public Article Article { get; set; }
        public Supplier Supplier { get; set; }
    }
}