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
    public class ShopServiceTests
    {
        [Fact]
        public void FindArticles()
        {
            using var mock = AutoMock.GetLoose();

            // Arrange
            var articleCode = "123";
            var maxPrice = 250;
            var quantity = 10;

            var inventories = new List<Inventory> { new Inventory { Id = Guid.NewGuid().ToString(), Price = 250, Quantity = 1, ArticleCode = articleCode, SupplierId = "1" },
                                               new Inventory { Id = Guid.NewGuid().ToString(), Price = 500, Quantity = 2, ArticleCode = "111", SupplierId = "1" },
                                               new Inventory { Id = Guid.NewGuid().ToString(), Price = 110, Quantity = 15, ArticleCode = articleCode, SupplierId = "1" }};

            var context = mock.Mock<InMemoryDbContext>().Object;

            mock.Mock<ILogger<ShopService>>();
            mock.Mock<IInventoryRepository>().Setup(x => x.GetByArticleCodeWithFilter(It.IsAny<string>(), It.IsAny<Func<Inventory, bool>>()))
                .Returns(inventories.Where(i => i.ArticleCode == articleCode).ToList());

            var shopService = mock.Create<ShopService>();

            // Act
            var actual = shopService.FindArticles(articleCode, maxPrice, quantity);

            // Assert
            Assert.NotNull(actual);
            Assert.True(actual.Count == 2);
            Assert.True(actual.All(a => a.ArticleCode == articleCode));
        }

        [Fact]
        public void FindArticles_NotEnoughArticlesOnStock()
        {
            using var mock = AutoMock.GetLoose();

            // Arrange
            var articleCode = "123";
            var maxPrice = 250;
            var quantity = 10;

            var inventories = new List<Inventory> { new Inventory { Id = Guid.NewGuid().ToString(), Price = 250, Quantity = 1, ArticleCode = articleCode, SupplierId = "1" },
                                               new Inventory { Id = Guid.NewGuid().ToString(), Price = 500, Quantity = 2, ArticleCode = "111", SupplierId = "1" },
                                               new Inventory { Id = Guid.NewGuid().ToString(), Price = 110, Quantity = 5, ArticleCode = articleCode, SupplierId = "1" }};

            var context = mock.Mock<InMemoryDbContext>().Object;

            mock.Mock<ILogger<ShopService>>();
            mock.Mock<IInventoryRepository>().Setup(x => x.GetByArticleCodeWithFilter(It.IsAny<string>(), It.IsAny<Func<Inventory, bool>>()))
                .Returns(inventories.Where(i => i.ArticleCode == articleCode).ToList());

            var shopService = mock.Create<ShopService>();

            // Act
            var actual = shopService.FindArticles(articleCode, maxPrice, quantity);

            // Assert
            Assert.NotNull(actual);
            Assert.True(actual.Count == 0);
        }

        [Fact]
        public void SellArticle()
        {
            using var mock = AutoMock.GetLoose();

            // Arrange
            var articleCode = "123";
            var quantity = 10;

            var inventories = new List<Inventory> { new Inventory { Id = Guid.NewGuid().ToString(), Price = 250, Quantity = 1, ArticleCode = articleCode, SupplierId = "1" },
                                               new Inventory { Id = Guid.NewGuid().ToString(), Price = 110, Quantity = 15, ArticleCode = articleCode, SupplierId = "1" }};

            var context = mock.Mock<InMemoryDbContext>().Object;

            mock.Mock<ILogger<ShopService>>();
            mock.Mock<IOrderItemRepository>().Setup(x => x.Add(It.IsAny<OrderItem>()))
                .Returns(new OrderItem() { Id = Guid.NewGuid().ToString() });

            var shopService = mock.Create<ShopService>();

            // Act
            var actual = shopService.SellArticle(inventories, quantity);

            // Assert
            Assert.NotNull(actual);
            Assert.True(actual.Count == 2);
        }
    }
}
