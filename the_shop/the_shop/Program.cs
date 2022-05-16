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
		private static ILogger<Program> logger;

		private static void Main(string[] args)
		{
			// Application setup and start
			var host = Startup.Start();

			// Insert data
			DataSetup.InsertData(host.Services);

			// Get instances to work with
			GetInstancesFromDI(host.Services);

			// Successfull order
			Shop("1", "333", 155, 2);
			Console.WriteLine();

			// Non existing article
			Shop("2", "555", 200, 4);
			Console.WriteLine();

			// Not enough articles on stock
			Shop("3", "222", 1000, 10);
			Console.WriteLine();

			// Order from multiple suppliers
			Shop("3", "111", 150, 3);
			Console.WriteLine();

			Console.ReadLine();
		}

		private static void GetInstancesFromDI(IServiceProvider services)
		{
			shopService = ActivatorUtilities.GetServiceOrCreateInstance<IShopService>(services);
			orderService = ActivatorUtilities.GetServiceOrCreateInstance<IOrderService>(services);
			logger = ActivatorUtilities.GetServiceOrCreateInstance<ILogger<Program>>(services);
		}

        private static List<Inventory> SearchForArticles(string articleCode, double maxPrice, int quantity)
        {
            var inventories = shopService.FindArticles(articleCode, maxPrice, quantity);
			return inventories;
		}

        private static Order CreateOrders(string customerId, int quantity, List<Inventory> inventories)
        {
			var orderItems = shopService.SellArticle(inventories, quantity);

			if (orderItems.Count > 0)
            {
				var order = orderService.CreateOrder(orderItems, customerId);
				return order;
			}
            else
            {
				return null;
            }
		}

		private static void Shop(string customerId, string articleCode, double maxPrice, int quantity)
		{
			var inventories = SearchForArticles(articleCode, maxPrice, quantity);

			if (inventories.Count > 0)
			{
				var order = CreateOrders(customerId, quantity, inventories);

				if (order != null)
				{
					logger.LogInformation(order.ToString());
					Console.WriteLine(order.ToString());
					return;
				}

				Console.WriteLine("Ordering failed.");
			}
		}
	}
}