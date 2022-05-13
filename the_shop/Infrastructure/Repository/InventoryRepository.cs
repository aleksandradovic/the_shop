using Domain.Entities;
using Application.Repositories;
using Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Repository.Repository
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly InMemoryDbContext _context;
        private readonly ILogger<InventoryRepository> _logger;

        public InventoryRepository(InMemoryDbContext context, ILogger<InventoryRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Inventory Add(Inventory inventory)
        {
            inventory.Id = Guid.NewGuid().ToString();
            _context.Inventories.Add(inventory);
            return inventory;
        }

        public void DecreaseQuantity(string inventoryId, int quantity = 1)
        {
            var inventory = _context.Inventories.FirstOrDefault(i => i.Id == inventoryId);

            if (inventory != null && inventory.Quantity >= quantity)
            {
                inventory.Quantity -= quantity;
            }
        }

        public void IncreaseQuantity(string inventoryId, int quantity = 1)
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

        public List<Inventory> GetByArticleCodeWithFilter(string articleCode, Func<Inventory, bool> func)
        {
            return _context.Inventories.Where(i => i.ArticleCode == articleCode).ToList().Where(func).ToList();
        }

        public Inventory GetById(string id)
        {
            return _context.Inventories.FirstOrDefault(i => i.Id == id);
        }

        public bool HasArticle(string inventoryId, string articleCode)
        {
            return _context.Inventories.FirstOrDefault(i => i.Id == inventoryId).ArticleCode == articleCode;
        }

        public void Remove(string id)
        {
            var inventory = _context.Inventories.FirstOrDefault(i => i.Id == id);

            if(inventory != null)
            {
                _context.Inventories.Remove(inventory);
            }
            else
            {
                _logger.LogInformation($"Failed deleting inventory. Inventory with id {id} does not exist.");
            }
        }
    }
}
