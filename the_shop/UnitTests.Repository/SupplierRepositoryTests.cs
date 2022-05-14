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
    public class SupplierRepositoryTests
    {
        [Fact]
        public void Add()
        {
            using var mock = AutoMock.GetLoose();

            // Arrange
            var supplier = new Supplier { Id = Guid.NewGuid().ToString(), Name = "Supplier", Address = "SupplierAddress" };

            var context = mock.Mock<InMemoryDbContext>().Object;

            mock.Mock<ILogger<SupplierRepository>>();

            var supplierRepository = mock.Create<SupplierRepository>();

            // Act
            supplierRepository.Add(supplier);

            // Assert
            Assert.Single(context.Suppliers);
        }

        [Fact]
        public void GetAll()
        {
            using var mock = AutoMock.GetLoose();

            // Arrange
            var suppliers = new List<Supplier> { new Supplier { Id = Guid.NewGuid().ToString(), Name = "Supplier1", Address = "SupplierAddress1" },
                                           new Supplier { Id = Guid.NewGuid().ToString(), Name = "Supplier2", Address = "SupplierAddress2" },
                                           new Supplier { Id = Guid.NewGuid().ToString(), Name = "Supplier3", Address = "SupplierAddress3" } };

            var context = mock.Mock<InMemoryDbContext>().Object;
            context.Suppliers.AddRange(suppliers);

            mock.Mock<ILogger<SupplierRepository>>();

            var supplierRepository = mock.Create<SupplierRepository>();

            // Act
            var actual = supplierRepository.GetAll();

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(suppliers, actual);
        }

        [Fact]
        public void GetById()
        {
            using var mock = AutoMock.GetLoose();

            // Arrange
            var suppliers = new List<Supplier> { new Supplier { Id = Guid.NewGuid().ToString(), Name = "Supplier1", Address = "SupplierAddress1" },
                                           new Supplier { Id = Guid.NewGuid().ToString(), Name = "Supplier2", Address = "SupplierAddress2" },
                                           new Supplier { Id = Guid.NewGuid().ToString(), Name = "Supplier3", Address = "SupplierAddress3" } };

            var context = mock.Mock<InMemoryDbContext>().Object;
            context.Suppliers.AddRange(suppliers);

            mock.Mock<ILogger<SupplierRepository>>();

            var supplierRepository = mock.Create<SupplierRepository>();

            // Act
            var actual = supplierRepository.GetById(suppliers[0].Id);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(suppliers.First(), actual);
        }

        [Fact]
        public void GetById_NonExisting()
        {
            using var mock = AutoMock.GetLoose();

            // Arrange
            var supplier = new Supplier { Id = "1", Name = "Supplier1", Address = "SupplierAddress1" };

            var context = mock.Mock<InMemoryDbContext>().Object;
            context.Suppliers.Add(supplier);

            mock.Mock<ILogger<SupplierRepository>>();

            var supplierRepository = mock.Create<SupplierRepository>();

            // Act
            var actual = supplierRepository.GetById("2");

            // Assert
            Assert.Null(actual);
        }

        [Fact]
        public void GetByName()
        {
            using var mock = AutoMock.GetLoose();

            // Arrange
            var suppliers = new List<Supplier> { new Supplier { Id = Guid.NewGuid().ToString(), Name = "Supplier1", Address = "SupplierAddress1" },
                                           new Supplier { Id = Guid.NewGuid().ToString(), Name = "Supplier2", Address = "SupplierAddress2" },
                                           new Supplier { Id = Guid.NewGuid().ToString(), Name = "Supplier3", Address = "SupplierAddress3" } };

            var context = mock.Mock<InMemoryDbContext>().Object;
            context.Suppliers.AddRange(suppliers);

            mock.Mock<ILogger<SupplierRepository>>();

            var supplierRepository = mock.Create<SupplierRepository>();

            // Act
            var actual = supplierRepository.GetByName(suppliers[2].Name);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(suppliers[2], actual);
        }

        [Fact]
        public void Remove()
        {
            using var mock = AutoMock.GetLoose();

            // Arrange
            var supplier = new Supplier { Id = Guid.NewGuid().ToString(), Name = "Supplier", Address = "SupplierAddress" };

            var context = mock.Mock<InMemoryDbContext>().Object;

            mock.Mock<ILogger<SupplierRepository>>();

            var supplierRepository = mock.Create<SupplierRepository>();
            supplierRepository.Add(supplier);

            // Act
            supplierRepository.Remove(supplier.Id);

            // Assert
            Assert.True(!context.Suppliers.Any());
        }
    }
}
