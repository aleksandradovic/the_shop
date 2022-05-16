using Application.Interfaces;
using Domain.Entities;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class ShopService : IShopService
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly ILogger<ShopService> _logger;

        public ShopService(IInventoryRepository inventoryRepository, IOrderItemRepository orderItemRepository, ILogger<ShopService> logger)
        {
            _inventoryRepository = inventoryRepository;
            _orderItemRepository = orderItemRepository;
            _logger = logger;
        }

        public List<Inventory>? FindArticles(string articleCode, double maxPrice, int quantity)
        {
            _logger.LogInformation($"Searching for article {articleCode} with maximum price {maxPrice} and quantity {quantity}.");

            var inventories = _inventoryRepository.GetByArticleCodeWithFilter(articleCode, a => a.Price <= maxPrice && quantity > 0).OrderBy(i => i.Price).ToList();

            if (!inventories.Any())
            {
                _logger.LogInformation($"None of the inventories has article {articleCode} with maximum price {maxPrice}.");
                return null;
            }
            else if (inventories.Sum(i => i.Quantity) < quantity)
            {
                inventories = new List<Inventory>();
                _logger.LogInformation("Order creating failed. Not enough articles on stock.");
            }

            return inventories;
        }

        public List<OrderItem> SellArticle(List<Inventory> inventories, int quantity)
        {
            List<OrderItem> orderItems = new List<OrderItem>();

            foreach (var inventory in inventories)
            {
                if (quantity <= 0)
                {
                    break;
                }
                else
                {
                    var orderItem = CreateOrderItem(inventory, ref quantity);
                    orderItems.Add(orderItem);
                }
            }

            return orderItems;
        }

        private OrderItem CreateOrderItem(Inventory inventory, ref int quantity)
        {
            _logger.LogInformation($"Creating order items.");

            var orderItem = _orderItemRepository.Add(new OrderItem() { InventoryId = inventory.Id, Price = inventory.Price });

            // In case that one inventory does not have enough articles, get the rest from another inventory
            var quantityFromCurrentInventory = inventory.Quantity >= quantity ? quantity : inventory.Quantity;
            _inventoryRepository.DecreaseQuantity(inventory.Id, quantityFromCurrentInventory);

            orderItem.Quantity = quantityFromCurrentInventory;
            quantity -= quantityFromCurrentInventory;

            return orderItem;
        }
    }
}
