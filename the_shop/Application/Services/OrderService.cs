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
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IOrderRepository orderRepository, ICustomerRepository customerRepository, ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _logger = logger;
        }

        public Order CreateOrder(List<OrderItem> orderItems, string customerId)
        {
            _logger.LogInformation($"Creating order for customer {customerId}.");

            // Create order
            var totalPrice = orderItems.Sum(oi => oi.Price * oi.Quantity);
            var order = _orderRepository.Add(new Order() { CustomerId = customerId, TotalPrice = totalPrice, CreatedAt = DateTime.Now, Items = orderItems });

            orderItems.ForEach(oi => oi.OrderId = order.Id);
           
            // Add order to customer
            var customer = _customerRepository.GetById(customerId);
            customer.Orders.Add(order);

            _logger.LogInformation($"Created order for customer {customerId} with total items {orderItems.Count}.");

            return order;
        }
    }
}
