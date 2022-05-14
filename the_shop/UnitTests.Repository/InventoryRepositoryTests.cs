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
    public class InventoryRepositoryTests
    {
        [Fact]
        public void Add()
        {
            using var mock = AutoMock.GetLoose();

            // Arrange
            var inventory = new Inventory { Id = Guid.NewGuid().ToString(), Price = 500, Quantity = 2, ArticleCode = "111", SupplierId = "1" };

            var context = mock.Mock<InMemoryDbContext>().Object;

            mock.Mock<ILogger<InventoryRepository>>();

            var inventoryRepository = mock.Create<InventoryRepository>();

            // Act
            inventoryRepository.Add(inventory);

            // Assert
            Assert.Single(context.Inventories);
        }

        [Fact]
        public void DecreaseQuantity()
        {
            using var mock = AutoMock.GetLoose();

            // Arrange
            var quantity = 2;
            var inventory = new Inventory { Id = Guid.NewGuid().ToString(), Price = 500, Quantity = quantity, ArticleCode = "111", SupplierId = "1" };

            var context = mock.Mock<InMemoryDbContext>().Object;

            mock.Mock<ILogger<InventoryRepository>>();

            var inventoryRepository = mock.Create<InventoryRepository>();
            inventoryRepository.Add(inventory);

            // Act
            inventoryRepository.DecreaseQuantity(inventory.Id);
            quantity--;

            // Assert
            Assert.Equal(quantity, inventory.Quantity);
        }

        [Fact]
        public void IncreaseQuantity()
        {
            using var mock = AutoMock.GetLoose();

            // Arrange
            var quantity = 2;
            var inventory = new Inventory { Id = Guid.NewGuid().ToString(), Price = 500, Quantity = quantity, ArticleCode = "111", SupplierId = "1" };

            var context = mock.Mock<InMemoryDbContext>().Object;

            mock.Mock<ILogger<InventoryRepository>>();

            var inventoryRepository = mock.Create<InventoryRepository>();
            inventoryRepository.Add(inventory);

            // Act
            inventoryRepository.IncreaseQuantity(inventory.Id);
            quantity++;

            // Assert
            Assert.Equal(quantity, inventory.Quantity);
        }

        [Fact]
        public void GetAll()
        {
            using var mock = AutoMock.GetLoose();

            // Arrange
            var inventories = new List<Inventory> { new Inventory { Id = Guid.NewGuid().ToString(), Price = 500, Quantity = 1, ArticleCode = "111", SupplierId = "1" },
                                               new Inventory { Id = Guid.NewGuid().ToString(), Price = 500, Quantity = 2, ArticleCode = "111", SupplierId = "1" },
                                               new Inventory { Id = Guid.NewGuid().ToString(), Price = 500, Quantity = 3, ArticleCode = "111", SupplierId = "1" }};

            var context = mock.Mock<InMemoryDbContext>().Object;
            context.Inventories.AddRange(inventories);

            mock.Mock<ILogger<InventoryRepository>>();

            var inventoryRepository = mock.Create<InventoryRepository>();

            // Act
            var actual = inventoryRepository.GetAll();

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(inventories, actual);
        }

        [Fact]
        public void GetByArticleCode()
        {
            using var mock = AutoMock.GetLoose();

            // Arrange
            var inventories = new List<Inventory> { new Inventory { Id = Guid.NewGuid().ToString(), Price = 500, Quantity = 1, ArticleCode = "111", SupplierId = "1" },
                                               new Inventory { Id = Guid.NewGuid().ToString(), Price = 500, Quantity = 2, ArticleCode = "222", SupplierId = "1" }};

            var context = mock.Mock<InMemoryDbContext>().Object;

            mock.Mock<ILogger<InventoryRepository>>();

            var inventoryRepository = mock.Create<InventoryRepository>();
            context.Inventories.AddRange(inventories);

            // Act
            var actual = inventoryRepository.GetByArticleCode(inventories[0].ArticleCode);

            // Assert
            Assert.Single(actual);
            Assert.Equal(inventories[0], actual.First());
        }

        [Fact]
        public void GetByArticleCodeWithFilter()
        {
            using var mock = AutoMock.GetLoose();

            // Arrange
            var inventories = new List<Inventory> { new Inventory { Id = Guid.NewGuid().ToString(), Price = 100, Quantity = 1, ArticleCode = "111", SupplierId = "1" },
                                                    new Inventory { Id = Guid.NewGuid().ToString(), Price = 500, Quantity = 2, ArticleCode = "222", SupplierId = "1" },
                                                    new Inventory { Id = Guid.NewGuid().ToString(), Price = 150, Quantity = 2, ArticleCode = "111", SupplierId = "1" }};

            var context = mock.Mock<InMemoryDbContext>().Object;

            mock.Mock<ILogger<InventoryRepository>>();

            var inventoryRepository = mock.Create<InventoryRepository>();
            context.Inventories.AddRange(inventories);

            // Act
            var actual = inventoryRepository.GetByArticleCodeWithFilter(inventories[0].ArticleCode, a => a.Price < 200);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(2, actual.Count);
            Assert.Equal(inventories[0], actual.First());
        }

        [Fact]
        public void GetById()
        {
            using var mock = AutoMock.GetLoose();

            // Arrange
            var inventories = new List<Inventory> { new Inventory { Id = Guid.NewGuid().ToString(), Price = 100, Quantity = 1, ArticleCode = "111", SupplierId = "1" },
                                                    new Inventory { Id = Guid.NewGuid().ToString(), Price = 500, Quantity = 2, ArticleCode = "222", SupplierId = "1" }};

            var context = mock.Mock<InMemoryDbContext>().Object;

            mock.Mock<ILogger<InventoryRepository>>();

            var inventoryRepository = mock.Create<InventoryRepository>();
            context.Inventories.AddRange(inventories);

            // Act
            var actual = inventoryRepository.GetById(inventories[0].Id);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(inventories[0], actual);
        }

        [Fact]
        public void GetById_NonExisting()
        {
            using var mock = AutoMock.GetLoose();

            // Arrange
            var inventory = new Inventory { Id = "1", Price = 100, Quantity = 1, ArticleCode = "111", SupplierId = "1" };

            var context = mock.Mock<InMemoryDbContext>().Object;

            mock.Mock<ILogger<InventoryRepository>>();

            var inventoryRepository = mock.Create<InventoryRepository>();
            context.Inventories.Add(inventory);

            // Act
            var actual = inventoryRepository.GetById("2");

            // Assert
            Assert.Null(actual);
        }

        [Fact]
        public void HasArticle_True()
        {
            using var mock = AutoMock.GetLoose();

            // Arrange
            var inventory = new Inventory { Id = Guid.NewGuid().ToString(), Price = 100, Quantity = 1, ArticleCode = "111", SupplierId = "1" };

            var context = mock.Mock<InMemoryDbContext>().Object;

            mock.Mock<ILogger<InventoryRepository>>();

            var inventoryRepository = mock.Create<InventoryRepository>();
            context.Inventories.Add(inventory);

            // Act
            var actual = inventoryRepository.HasArticle(inventory.Id, "111");

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void HasArticle_False()
        {
            using var mock = AutoMock.GetLoose();

            // Arrange
            var inventory = new Inventory { Id = Guid.NewGuid().ToString(), Price = 100, Quantity = 1, ArticleCode = "111", SupplierId = "1" };

            var context = mock.Mock<InMemoryDbContext>().Object;

            mock.Mock<ILogger<InventoryRepository>>();

            var inventoryRepository = mock.Create<InventoryRepository>();
            context.Inventories.Add(inventory);

            // Act
            var actual = inventoryRepository.HasArticle(inventory.Id, "222");

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void Remove()
        {
            using var mock = AutoMock.GetLoose();

            // Arrange
            var inventory = new Inventory { Id = Guid.NewGuid().ToString(), Price = 100, Quantity = 1, ArticleCode = "111", SupplierId = "1" };

            var context = mock.Mock<InMemoryDbContext>().Object;

            mock.Mock<ILogger<InventoryRepository>>();

            var inventoryRepository = mock.Create<InventoryRepository>();
            inventoryRepository.Add(inventory);

            // Act
            inventoryRepository.Remove(inventory.Id);

            // Assert
            Assert.True(!context.Inventories.Any());
        }
    }
}
