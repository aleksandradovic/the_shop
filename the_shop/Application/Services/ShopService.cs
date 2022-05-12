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

        public List<Inventory> FindArticles(string articleId, double maxPrice, int quantity)
        {
            _logger.LogInformation($"Searching for article {articleId} with maximum price {maxPrice} and quantity {quantity}.");
            var inventories = _inventoryRepository.GetByArticleIdWithFilter(articleId, a => a.Price <= maxPrice && quantity > 0).OrderBy(i => i.Price).ToList();

            if (!inventories.Any())
            {
                _logger.LogInformation($"None of the inventories has article {articleId} with maximum price {maxPrice}.");
            }
            else if (inventories.Sum(i => i.Quantity) < quantity)
            {
                inventories = new List<Inventory>();
                _logger.LogInformation($"Not enough (quantity: {quantity}) articles {articleId} with maximum price {maxPrice} on stock.");
            }

            return inventories;
        }

        public List<OrderItem> SellArticle(List<Inventory> inventories, int quantity)
        {
            _logger.LogInformation($"Creating order items.");
            List<OrderItem> orderItems = new List<OrderItem>();

            if (inventories?.Count > 0)
            {
                orderItems = _orderItemRepository.CreateOrderItems(inventories, quantity);
            }

            return orderItems;
        }
    }
}
