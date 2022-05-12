using Application.Repositories;
using Domain.Entities;
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

        public OrderItemRepository(IInventoryRepository inventoryRepository, InMemoryDbContext context)
        {
            _inventoryRepository = inventoryRepository;
            _context = context;
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

                    // If cannot get all resources from one inventory we will take all resources for one order item,
                    // so we are creating order items for every inventory until we collect full quantity.
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
        }
    }
}
