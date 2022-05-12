using Domain.Entities;
using Application.Repositories;
using Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly InMemoryDbContext _context;

        public InventoryRepository(InMemoryDbContext context)
        {
            _context = context;
        }

        public void Add(Inventory inventory)
        {
            _context.Inventories.Add(inventory);
        }

        public void DecreaseQuantity(int inventoryId, int quantity = 1)
        {
            var inventory = _context.Inventories.FirstOrDefault(i => i.Id == inventoryId);

            if (inventory != null)
            {
                inventory.Quantity -= quantity;
            }
        }

        public void IncreaseQuantity(int inventoryId, int quantity = 1)
        {
            var inventory = _context.Inventories.FirstOrDefault(i => i.Id == inventoryId);

            if (inventory != null)
            {
                inventory.Quantity += quantity;
            }
        }

        public List<Inventory> GetAll()
        {
            return _context.Inventories.ToList();
        }

        public List<Inventory> GetByArticleCode(string articleCode)
        {
            return _context.Inventories.Where(i => i.Article.Code == articleCode).ToList();
        }

        public List<Inventory> GetByArticleIdWithFilter(int articleId, Func<Inventory, bool> func)
        {
            return _context.Inventories.Where(i => i.Article.Id == articleId).Where(func).ToList();
        }

        public Inventory GetById(int id)
        {
            return _context.Inventories.FirstOrDefault(i => i.Id == id);
        }

        public bool HasArticle(int inventoryId, int articleId)
        {
            return _context.Inventories.FirstOrDefault(i => i.Id == inventoryId).ArticleId == articleId;
        }

        public void Remove(int id)
        {
            var inventory = _context.Inventories.FirstOrDefault(i => i.Id == id);

            if(inventory != null)
            {
                _context.Inventories.Remove(inventory);
            }
        }
    }
}
