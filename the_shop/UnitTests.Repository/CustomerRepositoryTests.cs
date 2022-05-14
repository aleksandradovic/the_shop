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
    public class CustomerRepositoryTests
    {
        [Fact]
        public void Add()
        {
            using var mock = AutoMock.GetLoose();

            // Arrange
            var customer = new Customer { Id = Guid.NewGuid().ToString(), FirstName = "CustomerName",  LastName = "CustomerSurname", Email = "customer@mail.mail", PhoneNumber = "123-456" };

            var context = mock.Mock<InMemoryDbContext>().Object;

            mock.Mock<ILogger<CustomerRepository>>();

            var customerRepository = mock.Create<CustomerRepository>();

            // Act
            customerRepository.Add(customer);

            // Assert
            Assert.Single(context.Customers);
        }

        [Fact]
        public void AddOrder()
        {
            using var mock = AutoMock.GetLoose();

            // Arrange
            var customer = new Customer { Id = Guid.NewGuid().ToString(), FirstName = "CustomerName", LastName = "CustomerSurname", Email = "customer@mail.mail", PhoneNumber = "123-456" };

            var orderItems = new List<OrderItem> { new OrderItem() { Id = Guid.NewGuid().ToString(), InventoryId = Guid.NewGuid().ToString(), Price = 100, Quantity = 5 } };
            var order = new Order { Id = Guid.NewGuid().ToString(), CustomerId = customer.Id, TotalPrice = 250, CreatedAt = DateTime.Now, Items = orderItems };
            orderItems.ForEach(oi => oi.OrderId = order.Id);

            var context = mock.Mock<InMemoryDbContext>().Object;

            mock.Mock<ILogger<CustomerRepository>>();

            var customerRepository = mock.Create<CustomerRepository>();
            customerRepository.Add(customer);

            // Act
            customerRepository.AddOrder(customer.Id, order);
            var actual = customerRepository.GetById(customer.Id).Orders;

            // Assert
            Assert.Single(actual);
            Assert.Equal(order, actual.First());
        }

        [Fact]
        public void GetAll()
        {
            using var mock = AutoMock.GetLoose();

            // Arrange
            var customers = new List<Customer> { new Customer() { Id = Guid.NewGuid().ToString(), FirstName = "CustomerName1", LastName = "CustomerSurname1", Email = "customer1@mail.mail" },
                                               new Customer() { Id = Guid.NewGuid().ToString(), FirstName = "CustomerName2", LastName = "CustomerSurname2", Email = "customer2@mail.mail" },
                                               new Customer() { Id = Guid.NewGuid().ToString(), FirstName = "CustomerName3", LastName = "CustomerSurname3", Email = "customer3@mail.mail" }};

            var context = mock.Mock<InMemoryDbContext>().Object;
            context.Customers.AddRange(customers);

            mock.Mock<ILogger<CustomerRepository>>();

            var customerRepository = mock.Create<CustomerRepository>();

            // Act
            var actual = customerRepository.GetAll();

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(customers, actual);
        }

        [Fact]
        public void GetByEmail()
        {
            using var mock = AutoMock.GetLoose();

            // Arrange
            var customer = new Customer { Id = Guid.NewGuid().ToString(), FirstName = "CustomerName", LastName = "CustomerSurname", Email = "customer@mail.mail" };

            var context = mock.Mock<InMemoryDbContext>().Object;

            mock.Mock<ILogger<CustomerRepository>>();

            var customerRepository = mock.Create<CustomerRepository>();
            customerRepository.Add(customer);

            // Act
            var actual = customerRepository.GetByEmail(customer.Email);

            // Assert
            Assert.Equal(customer.Email, actual.Email);
        }

        [Fact]
        public void GetById()
        {
            using var mock = AutoMock.GetLoose();

            // Arrange
            var customer = new Customer { Id = Guid.NewGuid().ToString(), FirstName = "CustomerName", LastName = "CustomerSurname", Email = "customer@mail.mail" };

            var context = mock.Mock<InMemoryDbContext>().Object;

            mock.Mock<ILogger<CustomerRepository>>();

            var customerRepository = mock.Create<CustomerRepository>();
            customerRepository.Add(customer);

            // Act
            var actual = customerRepository.GetById(customer.Id);

            // Assert
            Assert.Equal(customer.Id, actual.Id);
        }

        [Fact]
        public void GetById_NonExisting()
        {
            using var mock = AutoMock.GetLoose();

            // Arrange
            var customer = new Customer { Id = Guid.NewGuid().ToString(), FirstName = "CustomerName", LastName = "CustomerSurname", Email = "customer@mail.mail" };

            var context = mock.Mock<InMemoryDbContext>().Object;

            mock.Mock<ILogger<CustomerRepository>>();

            var customerRepository = mock.Create<CustomerRepository>();
            customerRepository.Add(customer);

            // Act
            var actual = customerRepository.GetById("1");

            // Assert
            Assert.Null(actual);
        }

        [Fact]
        public void Remove()
        {
            using var mock = AutoMock.GetLoose();

            // Arrange
            var customer = new Customer { Id = Guid.NewGuid().ToString(), FirstName = "CustomerName", LastName = "CustomerSurname", Email = "customer@mail.mail" };

            var context = mock.Mock<InMemoryDbContext>().Object;

            mock.Mock<ILogger<CustomerRepository>>();

            var customerRepository = mock.Create<CustomerRepository>();
            customerRepository.Add(customer);

            // Act
            customerRepository.Remove(customer.Id);

            // Assert
            Assert.True(!context.Customers.Any());
        }
    }
}
