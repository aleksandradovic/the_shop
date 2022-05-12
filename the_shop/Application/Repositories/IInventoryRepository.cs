using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Application.Repositories.Base;

namespace Application.Repositories
{
    public interface IInventoryRepository : IBaseRepository<Inventory>
    {
        public List<Inventory> GetByArticleCode(string articleCode);
        public List<Inventory> GetByArticleIdWithFilter(int articleId, Func<Inventory, bool> func);
        public bool HasArticle(int inventoryId, int articleId);
        public void IncreaseQuantity(int inventoryId, int quantity = 1);
        public void DecreaseQuantity(int inventoryId, int quantity = 1);
    }
}
