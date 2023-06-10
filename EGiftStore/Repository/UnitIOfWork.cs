using Persistence.Entities;
using Repository.Repositories.Implement;
using Repository.Repositories.Interface;
using System;
using System.Collections.Generic;

namespace Repository
{
    public class UnitIOfWork : IUnitIOfWork
    {
        private readonly EgiftShopContext _context;

        public UnitIOfWork(EgiftShopContext context)
        {
            _context = context;
            CustomerRepository = new CustomerRepository(_context);
            AdminRepository = new AdminRepository(_context);
            ProductRepository = new ProductRepository(_context);
            CategoryRepository = new CategoryRepository(_context);
        }
        public ICustomerRepository CustomerRepository { get; private set; }

        public IAdminRepository AdminRepository { get; private set; }

        public IProductRepository ProductRepository { get; private set; }

        public ICategoryRepository CategoryRepository { get; private set; }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
