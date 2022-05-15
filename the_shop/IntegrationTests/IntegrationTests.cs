using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using the_shop;
using the_shop.Data;
using Xunit;

namespace IntegrationTests
{
    public class IntegrationTests
    {
        public IShopService ShopService { get; set; }
        public IOrderService OrderService { get; set; }
        public InMemoryDbContext Context { get; set; }

        public IntegrationTests()
        {
            var host = Startup.Start();

            // Insert data
            Context = DataSetup.InsertData(host.Services);

            // Get service instances
            ShopService = ActivatorUtilities.GetServiceOrCreateInstance<IShopService>(host.Services);
            OrderService = ActivatorUtilities.GetServiceOrCreateInstance<IOrderService>(host.Services);
        }

        [Theory]
        [InlineData(true, "111", 100, 3)]
        [InlineData(false, "000", 150, 1)]
        [InlineData(false, "", 120, 1)]
        [InlineData(false, null, 200, 1)]
        public void FindArticles(bool shouldPass, string articleCode, double maxPrice, int quantity)
        {
            // Act
            var inventories = ShopService.FindArticles(articleCode, maxPrice, quantity);

            // Assert
            if (shouldPass)
            {
                
                Assert.NotNull(articleCode);
                Assert.True(inventories.All(i => i.ArticleCode == articleCode));
            }
            else
            {
                Assert.True(inventories.Count == 0);
            }
        }

        [Theory]
        [InlineData(new bool[] { true }, new string[] { "111" }, new double[] { 100 }, new int[] { 2 }, new string[] { "1" } )] // one article
        [InlineData(new bool[] { true, true }, new string[] { "111", "222" }, new double[] { 150, 200 }, new int[] { 2, 3 }, new string[] { "2", "3" })] // two articles
        [InlineData(new bool[] { true, false }, new string[] { "111", "111" }, new double[] { 160, 230 }, new int[] { 1, 15 }, new string[] { "1", "2" })] // one article twice, second order with invalid quantity
        [InlineData(new bool[] { false }, new string[] { "000" }, new double[] { 1000 }, new int[] { 7 }, new string[] { "3" })] // article does not exist
        [InlineData(new bool[] { false }, new string[] { "111" }, new double[] { 120 }, new int[] { 100 }, new string[] { "2" })] // quantity is bigger than available
        [InlineData(new bool[] { false }, new string[] { "111" }, new double[] { 50 }, new int[] { 1 }, new string[] { "3" })] // maxPrice is lower than min article price
        [InlineData(new bool[] { false }, new string[] { "" }, new double[] { 100 }, new int[] { 3 }, new string[] { "1" })] // string.Empty as article code 
        [InlineData(new bool[] { false }, new string[] { null }, new double[] { 110 }, new int[] { 5 }, new string[] { "1" })] // null as article code 
        public void TestFullArticleSellingFlow(bool[] createOrderItems, string[] articleCode, double[] maxPrice, int[] quantity, string[] customers)
        {
            // Act
            for (var i = 0; i < articleCode.Length; i++)
            {
                var inventories = ShopService.FindArticles(articleCode[i], maxPrice[i], quantity[i]);

                if (createOrderItems[i])
                {
                    var orderItems = ShopService.SellArticle(inventories, quantity[i]);

                    var order = OrderService.CreateOrder(orderItems, customers[i]);

                    // Assert
                    Assert.NotNull(orderItems);
                    Assert.NotNull(Context.Orders);
                }
            }

            Assert.Equal(Context.Orders.Count, createOrderItems.Where(x => x.Equals(true)).Count());
        }
    }
}
