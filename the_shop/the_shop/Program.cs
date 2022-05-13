using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Repository.Context;
using System;
using System.Collections.Generic;
using the_shop.Data;

namespace the_shop
{
	internal class Program
	{
		private static IShopService shopService;
		private static IOrderService orderService;
		private static InMemoryDbContext context;
		private static ILogger<Program> logger;

		private static void Main(string[] args)
		{
			// Application setup and start
			var host = Startup.Start();

			// Insert data
			context = DataSetup.InsertData(host.Services);

			// Get instances to work with
			GetInstancesFromDI(host.Services);

			logger.LogInformation("Starting");
			var inventories = SearchForArticles("1", 105, 2);
			var order = CreateOrders(inventories);

			var inventories2 = SearchForArticles("10", 200, 4);
			var order2 = CreateOrders(inventories2);

			Console.ReadLine();
		}

		private static void GetInstancesFromDI(IServiceProvider services)
		{
			shopService = ActivatorUtilities.GetServiceOrCreateInstance<IShopService>(services);
			orderService = ActivatorUtilities.GetServiceOrCreateInstance<IOrderService>(services);
			logger = ActivatorUtilities.GetServiceOrCreateInstance<ILogger<Program>>(services);
		}

        private static List<Inventory> SearchForArticles(string articleId, double maxPrice, int quantity)
        {
            var inventories = shopService.FindArticles(articleId, maxPrice, quantity);
			return inventories;
		}

        private static Order CreateOrders(List<Inventory> inventories)
        {
			var orderItems = shopService.SellArticle(inventories, 2);
            var order = orderService.CreateOrder(orderItems, context.Customers[0].Id);

			return order;
        }
    }
}