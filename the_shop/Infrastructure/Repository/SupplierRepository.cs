using Application.Repositories;
using Domain.Entities;
using Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly InMemoryDbContext _context;

        public SupplierRepository(InMemoryDbContext context)
        {
            _context = context;
        }

        public Supplier Add(Supplier supplier)
        {
            supplier.Id = Guid.NewGuid().ToString();
            _context.Suppliers.Add(supplier);
            return supplier;
        }

        public List<Supplier> GetAll()
        {
            return _context.Suppliers.ToList();
        }

        public Supplier GetById(string id)
        {
            return _context.Suppliers.FirstOrDefault(s => s.Id == id);
        }

        public Supplier GetByName(string name)
        {
            return _context.Suppliers.FirstOrDefault(s => s.Name == name);
        }

        public void Remove(string id)
        {
            var supplier = _context.Suppliers.FirstOrDefault(s => s.Id == id);

            if(supplier != null)
            {
                _context.Suppliers.Remove(supplier);
            }
        }
    }
}
