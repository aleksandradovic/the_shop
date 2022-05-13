using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace the_shop.Data
{
    public static class DataSetup
    {
        public static InMemoryDbContext InsertData(IServiceProvider serviceProvider)
        {
            var context = ActivatorUtilities.GetServiceOrCreateInstance<InMemoryDbContext>(serviceProvider);

            GetArticles().ForEach(x => context.Articles.Add(x));
            GetCustomers().ForEach(x => context.Customers.Add(x));
            GetSuppliers().ForEach(x => context.Suppliers.Add(x));

            foreach (var supplier in context.Suppliers)
            {
                supplier.Inventories.ForEach(x => context.Inventories.Add(x));
            }

            return context;
        }

        private static List<Article> GetArticles()
        {
            return new List<Article>
            {
                new Article { Code = "111", Name = "Article 1" },
                new Article { Code = "222", Name = "Article 2" },
                new Article { Code = "333", Name = "Article 3" },
                new Article { Code = "444", Name = "Article 4" },
                new Article { Code = "555", Name = "Article 5" }
            };
        }

        private static List<Customer> GetCustomers()
        {
            return new List<Customer>
            {
                new Customer { Id = "1", Email = "customer01@mail.mail", FirstName = "Name1", LastName = "Customer1", PhoneNumber = "+123456", Orders = new List<Order>()},
                new Customer { Id = "2", Email = "customer02@mail.mail", FirstName = "Name2", LastName = "Customer2", PhoneNumber = "+654321", Orders = new List<Order>()},
                new Customer { Id = "3", Email = "customer03@mail.mail", FirstName = "Name3", LastName = "Customer3", PhoneNumber = "+112233", Orders = new List<Order>()},
            };
        }

        private static List<Inventory> GetInventories()
        {
            var articles = GetArticles();
            var inventories = new List<Inventory>();
            var quantity = 2;
            var price = 100;
            int id = 1;

            foreach (var article in articles)
            {
                if(article.Code != "555")
                {
                    inventories.Add(new Inventory { Id = id.ToString(), ArticleCode = article.Code, Price = price, Quantity = quantity });
                }

                if (article.Code == "111")
                {
                    id++;
                    price += 2;
                    inventories.Add(new Inventory { Id = id.ToString(), ArticleCode = article.Code, Price = price, Quantity = quantity });
                }

                quantity += 2;
                price += 10;
                id++;
            }

            return inventories;
        }

        private static List<Supplier> GetSuppliers()
        {
            var inventories = GetInventories();
            var suppliers = new List<Supplier>();

            suppliers.Add(new Supplier { Id = "1", Name = $"Supplier 1", Address = "Address 1", Inventories = inventories.GetRange(1,2) });
            suppliers.Add(new Supplier { Id = "2", Name = $"Supplier 2", Address = "Address 2", Inventories = inventories.GetRange(0,1) });
            suppliers.Add(new Supplier { Id = "3", Name = $"Supplier 3", Address = "Address 3", Inventories = inventories.GetRange(3,2) });

            return suppliers;
        }
    }
}
