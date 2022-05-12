using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IShopService
    {
        List<Inventory> FindArticles(string articleId, double maxPrice, int quantity);
        List<OrderItem> SellArticle(List<Inventory> inventories, int quantity);
    }
}
