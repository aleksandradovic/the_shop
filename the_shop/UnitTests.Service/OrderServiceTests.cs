using Application.Repositories;
using Application.Services;
using Autofac.Extras.Moq;
using Domain.Entities;
using Infrastructure.Repository;
using Microsoft.Extensions.Logging;
using Moq;
using Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.Service
{
    public class OrderServiceTests
    {
        [Fact]
        public void CreateOrder()
        {
            using var mock = AutoMock.GetLoose();

            // Arrange
            var quanity = 5;
            var inventory = new Inventory() { Id = Guid.NewGuid().ToString(), ArticleCode = "111", Price = 500, Quantity = 10, SupplierId = "1" };

            var orderItems = new List<OrderItem> { new OrderItem { Id = Guid.NewGuid().ToString(), Price = inventory.Price, Quantity = quanity, InventoryId = inventory.Id }};

            var customer = new Customer { Id = Guid.NewGuid().ToString(), FirstName = "CustomerName", LastName = "CustomerSurname", Email = "customer@mail.mail", PhoneNumber = "123-456" };

            var context = mock.Mock<InMemoryDbContext>().Object;

            mock.Mock<ILogger<OrderService>>();
            mock.Mock<IOrderRepository>().Setup(x => x.Add(It.IsAny<Order>()))
                .Returns(new Order() { Id = Guid.NewGuid().ToString(), TotalPrice = 120, CreatedAt = DateTime.Now, CustomerId = customer.Id, Items = orderItems });

            mock.Mock<ICustomerRepository>().Setup(x => x.GetById(It.IsAny<string>()))
                .Returns(customer);

            var orderService = mock.Create<OrderService>();

            // Act
            var actual = orderService.CreateOrder(orderItems, customer.Id);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(actual.Items, orderItems);
            Assert.Equal(actual.Items.Sum(oi => oi.Price * oi.Quantity), inventory.Price * quanity);
        }
    }
}
