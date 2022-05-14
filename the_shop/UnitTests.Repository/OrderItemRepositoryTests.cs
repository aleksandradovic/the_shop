using Autofac.Extras.Moq;
using Domain.Entities;
using Infrastructure.Repository;
using Microsoft.Extensions.Logging;
using Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.Repository
{
    public class OrderItemRepositoryTests
    {
        [Fact]
        public void Add()
        {
            using var mock = AutoMock.GetLoose();

            // Arrange
            var orderItem = new OrderItem { Id = Guid.NewGuid().ToString(), Price = 100, Quantity = 2, InventoryId = "1", OrderId = "1" };

            var context = mock.Mock<InMemoryDbContext>().Object;

            mock.Mock<ILogger<OrderItemRepository>>();

            var orderItemRepository = mock.Create<OrderItemRepository>();

            // Act
            orderItemRepository.Add(orderItem);

            // Assert
            Assert.Single(context.OrderItems);
        }

        [Fact]
        public void GetAll()
        {
            using var mock = AutoMock.GetLoose();

            // Arrange
            var orderItems = new List<OrderItem> { new OrderItem { Id = Guid.NewGuid().ToString(), Price = 100, Quantity = 2, InventoryId = "1", OrderId = "1" },
                                               new OrderItem { Id = Guid.NewGuid().ToString(), Price = 150, Quantity = 1, InventoryId = "3", OrderId = "5" },
                                               new OrderItem { Id = Guid.NewGuid().ToString(), Price = 220, Quantity = 10, InventoryId = "2", OrderId = "41" }};

            var context = mock.Mock<InMemoryDbContext>().Object;
            context.OrderItems.AddRange(orderItems);

            mock.Mock<ILogger<OrderItemRepository>>();

            var orderItemRepository = mock.Create<OrderItemRepository>();

            // Act
            var actual = orderItemRepository.GetAll();

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(orderItems, actual);
        }

        [Fact]
        public void GetById()
        {
            using var mock = AutoMock.GetLoose();

            // Arrange
            var orderItems = new List<OrderItem> { new OrderItem { Id = Guid.NewGuid().ToString(), Price = 100, Quantity = 2, InventoryId = "1", OrderId = "1" },
                                               new OrderItem { Id = Guid.NewGuid().ToString(), Price = 150, Quantity = 1, InventoryId = "3", OrderId = "5" },
                                               new OrderItem { Id = Guid.NewGuid().ToString(), Price = 220, Quantity = 10, InventoryId = "2", OrderId = "41" }};

            var context = mock.Mock<InMemoryDbContext>().Object;

            mock.Mock<ILogger<OrderItemRepository>>();

            var orderItemRepository = mock.Create<OrderItemRepository>();
            context.OrderItems.AddRange(orderItems);

            // Act
            var actual = orderItemRepository.GetById(orderItems[1].Id);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(orderItems[1], actual);
        }

        [Fact]
        public void GetById_NonExisting()
        {
            using var mock = AutoMock.GetLoose();

            // Arrange
            var orderItem = new OrderItem { Id = "1", Price = 100, Quantity = 2, InventoryId = "1", OrderId = "1" };

            var context = mock.Mock<InMemoryDbContext>().Object;

            mock.Mock<ILogger<OrderItemRepository>>();

            var orderItemRepository = mock.Create<OrderItemRepository>();
            context.OrderItems.Add(orderItem);

            // Act
            var actual = orderItemRepository.GetById("2");

            // Assert
            Assert.Null(actual);
        }

        [Fact]
        public void Remove()
        {
            using var mock = AutoMock.GetLoose();

            // Arrange
            var orderItem = new OrderItem { Id = Guid.NewGuid().ToString(), Price = 100, Quantity = 2, InventoryId = "1", OrderId = "1" };

            var context = mock.Mock<InMemoryDbContext>().Object;

            mock.Mock<ILogger<OrderItemRepository>>();

            var orderItemRepository = mock.Create<OrderItemRepository>();
            orderItemRepository.Add(orderItem);

            // Act
            orderItemRepository.Remove(orderItem.Id);

            // Assert
            Assert.True(!context.OrderItems.Any());
        }
    }
}
