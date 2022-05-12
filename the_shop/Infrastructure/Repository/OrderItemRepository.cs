using Application.Repositories;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly InMemoryDbContext _context;
        private readonly ILogger<OrderItemRepository> _logger;

        public OrderItemRepository(IInventoryRepository inventoryRepository, InMemoryDbContext context, ILogger<OrderItemRepository> logger)
        {
            _inventoryRepository = inventoryRepository;
            _context = context;
            _logger = logger;
        }

        public OrderItem Add(OrderItem orderItem)
        {
            orderItem.Id = Guid.NewGuid().ToString();
            _context.OrderItems.Add(orderItem);
            return orderItem;
        }

        public List<OrderItem> CreateOrderItems(List<Inventory> inventories, int quantity)
        {
            var orderItems = new List<OrderItem>();

            foreach (var inventory in inventories)
            {
                if (quantity <= 0)
                {
                    break;
                }
                else
                {
                    var orderItem = Add(new OrderItem() { InventoryId = inventory.Id, Price = inventory.Price });

                    // In case that one inventory does not have enough articles, get the rest from another inventory
                    var quantityFromCurrentInventory = inventory.Quantity >= quantity ? quantity : inventory.Quantity;
                    _inventoryRepository.DecreaseQuantity(inventory.Id, quantityFromCurrentInventory);
                       
                     orderItem.Quantity = quantityFromCurrentInventory;
                     quantity -= inventory.Quantity;
                     orderItem.InventoryId = inventory.Id;
                     orderItems.Add(orderItem);
                }
            }

            if (quantity > 0)
            {
                foreach (var orderItem in orderItems)
                {
                    _inventoryRepository.IncreaseQuantity(orderItem.InventoryId, orderItem.Quantity);
                }

                orderItems = new List<OrderItem>();

                _logger.LogInformation("Not enough articles on stock.");
            }

            return orderItems;
        }

        public List<OrderItem> GetAll()
        {
            return _context.OrderItems.ToList();
        }

        public OrderItem GetById(string id)
        {
            return _context.OrderItems.FirstOrDefault(oi => oi.Id == id);
        }

        public void Remove(string id)
        {
            var orderItem = _context.OrderItems.FirstOrDefault(i => i.Id == id);

            if (orderItem != null)
            {
                _context.OrderItems.Remove(orderItem);
            }
            else
            {
                _logger.LogInformation($"Failed deleting order item. Order item with id {id} does not exist.");
            }
        }
    }
}
