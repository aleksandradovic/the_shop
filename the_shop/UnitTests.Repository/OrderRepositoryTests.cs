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
    public class OrderRepositoryTests
    {
        [Fact]
        public void Add()
        {
            using var mock = AutoMock.GetLoose();

            // Arrange
            var order = new Order { Id = Guid.NewGuid().ToString(), TotalPrice = 500, CreatedAt = DateTime.Now, CustomerId = "5" };

            var context = mock.Mock<InMemoryDbContext>().Object;

            mock.Mock<ILogger<OrderRepository>>();

            var orderRepository = mock.Create<OrderRepository>();

            // Act
            orderRepository.Add(order);

            // Assert
            Assert.Single(context.Orders);
        }

        [Fact]
        public void GetAll()
        {
            using var mock = AutoMock.GetLoose();

            // Arrange
            var orders = new List<Order> { new Order { Id = Guid.NewGuid().ToString(), TotalPrice = 500, CreatedAt = DateTime.Now, CustomerId = "5" },
                                               new Order { Id = Guid.NewGuid().ToString(), TotalPrice = 100, CreatedAt = DateTime.Now, CustomerId = "1" },
                                               new Order { Id = Guid.NewGuid().ToString(), TotalPrice = 250, CreatedAt = DateTime.Now, CustomerId = "3" } };

            var context = mock.Mock<InMemoryDbContext>().Object;
            context.Orders.AddRange(orders);

            mock.Mock<ILogger<OrderRepository>>();

            var orderRepository = mock.Create<OrderRepository>();

            // Act
            var actual = orderRepository.GetAll();

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(orders, actual);
        }

        [Fact]
        public void GetByCustomerId()
        {
            using var mock = AutoMock.GetLoose();

            // Arrange
            var orders = new List<Order> { new Order { Id = Guid.NewGuid().ToString(), TotalPrice = 500, CreatedAt = DateTime.Now, CustomerId = "5" },
                                               new Order { Id = Guid.NewGuid().ToString(), TotalPrice = 100, CreatedAt = DateTime.Now, CustomerId = "1" },
                                               new Order { Id = Guid.NewGuid().ToString(), TotalPrice = 250, CreatedAt = DateTime.Now, CustomerId = "3" } };

            var context = mock.Mock<InMemoryDbContext>().Object;

            mock.Mock<ILogger<OrderRepository>>();

            var orderRepository = mock.Create<OrderRepository>();
            context.Orders.AddRange(orders);

            // Act
            var actual = orderRepository.GetByCustomerId(orders[0].CustomerId);

            // Assert
            Assert.NotNull(actual);
            Assert.Single(actual);
            Assert.Equal(orders.First(), actual.First());
        }

        [Fact]
        public void GetById()
        {
            using var mock = AutoMock.GetLoose();

            // Arrange
            var orders = new List<Order> { new Order { Id = Guid.NewGuid().ToString(), TotalPrice = 500, CreatedAt = DateTime.Now, CustomerId = "5" },
                                               new Order { Id = Guid.NewGuid().ToString(), TotalPrice = 100, CreatedAt = DateTime.Now, CustomerId = "1" },
                                               new Order { Id = Guid.NewGuid().ToString(), TotalPrice = 250, CreatedAt = DateTime.Now, CustomerId = "3" } };

            var context = mock.Mock<InMemoryDbContext>().Object;

            mock.Mock<ILogger<OrderRepository>>();

            var orderRepository = mock.Create<OrderRepository>();
            context.Orders.AddRange(orders);

            // Act
            var actual = orderRepository.GetById(orders[0].Id);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(orders.First(), actual);
        }

        [Fact]
        public void GetById_NonExisting()
        {
            using var mock = AutoMock.GetLoose();

            // Arrange
            var order = new Order { Id = "1", TotalPrice = 500, CreatedAt = DateTime.Now, CustomerId = "5" };

            var context = mock.Mock<InMemoryDbContext>().Object;

            mock.Mock<ILogger<OrderRepository>>();

            var orderRepository = mock.Create<OrderRepository>();
            context.Orders.Add(order);

            // Act
            var actual = orderRepository.GetById("2");

            // Assert
            Assert.Null(actual);
        }

        [Fact]
        public void Remove()
        {
            using var mock = AutoMock.GetLoose();

            // Arrange
            var order = new Order { Id = Guid.NewGuid().ToString(), TotalPrice = 500, CreatedAt = DateTime.Now, CustomerId = "5" };

            var context = mock.Mock<InMemoryDbContext>().Object;

            mock.Mock<ILogger<OrderRepository>>();

            var orderRepository = mock.Create<OrderRepository>();
            orderRepository.Add(order);

            // Act
            orderRepository.Remove(order.Id);

            // Assert
            Assert.True(!context.Orders.Any());
        }
    }
}
