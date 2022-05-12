using Application.Interfaces;
using Domain.Entities;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ShopService : IShopService
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IOrderItemRepository _orderItemRepository;

        public ShopService(IInventoryRepository inventoryRepository, IOrderItemRepository orderItemRepository)
        {
            _inventoryRepository = inventoryRepository;
            _orderItemRepository = orderItemRepository;
        }

        public List<Inventory> FindArticles(string articleId, double maxPrice, int quantity)
        {
            var inventories = _inventoryRepository.GetByArticleIdWithFilter(articleId, a => a.Price <= maxPrice && quantity > 0).OrderBy(i => i.Price).ToList();
            return inventories;
        }

        public List<OrderItem> SellArticle(List<Inventory> inventories, int quantity)
        {
            List<OrderItem> orderItems = new List<OrderItem>();

            if (inventories?.Count > 0)
            {
                orderItems = _orderItemRepository.CreateOrderItems(inventories, quantity);
            }

            return orderItems;
        }
    }
}
