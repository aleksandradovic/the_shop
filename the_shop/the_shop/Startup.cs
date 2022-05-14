using Application.Interfaces;
using Application.Repositories;
using Application.Services;
using Infrastructure.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Repository.Context;
using Serilog;
using System;
using System.IO;

namespace the_shop
{
    public static class Startup
    {
        public static IHost Start()
        {
            // Create builder
            var builder = new ConfigurationBuilder();

            // Setup configuration
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Setup logger
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();      

            // Configure services
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // Repositories
                    services.AddTransient<IArticleRepository, ArticleRepository>();
                    services.AddTransient<ICustomerRepository, CustomerRepository>();
                    services.AddTransient<IInventoryRepository, InventoryRepository>();
                    services.AddTransient<IOrderRepository, OrderRepository>();
                    services.AddTransient<IOrderItemRepository, OrderItemRepository>();
                    services.AddTransient<ISupplierRepository, SupplierRepository>();

                    // Services
                    services.AddTransient<IShopService, ShopService>();
                    services.AddTransient<IOrderService, OrderService>();

                    // Singletons
                    services.AddSingleton<InMemoryDbContext>();
                })
                .UseSerilog()
                .Build();

            return host;
        }
    }
}
