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
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        
        private readonly ICustomerRepository _customerRepository;

        public OrderService(IOrderRepository orderRepository, ICustomerRepository customerRepository)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
        }

        public Order CreateOrder(List<OrderItem> orderItems, string customerId)
        {
            // Create order
            var totalPrice = orderItems.Sum(oi => oi.Price * oi.Quantity);
            var order = _orderRepository.Add(new Order() { CustomerId = customerId, TotalPrice = totalPrice, CreatedAt = DateTime.Now, Items = orderItems });

            orderItems.ForEach(oi => oi.OrderId = order.Id);
           
            // Add order to customer
            var customer = _customerRepository.GetById(customerId);
            customer.Orders.Add(order);

            return order;
        }
    }
}
