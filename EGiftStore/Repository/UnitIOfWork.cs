﻿using Microsoft.EntityFrameworkCore.Storage;
using Persistence.Entities;
using Repository.Repositories.Implement;
using Repository.Repositories.Interface;


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
            ProductImageRepository = new ProductImageRepository(_context);
            CartRepository = new CartRepository(_context);
            CartItemRepository = new CartItemRepository(_context);
            OrderDetailRepository = new OrderDetailRepository(_context);
            OrderRepository = new OrderRepository(_context);
        }
        public ICustomerRepository CustomerRepository { get; private set; }

        public IAdminRepository AdminRepository { get; private set; }

        public IProductRepository ProductRepository { get; private set; }

        public ICategoryRepository CategoryRepository { get; private set; }

        public IProductImageRepository ProductImageRepository { get; private set; }

        public ICartRepository CartRepository { get; private set; }

        public ICartItemRepository CartItemRepository { get; private set; }

        public IOrderDetailRepository OrderDetailRepository { get; private set; }

        public IOrderRepository OrderRepository { get; private set; }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public IDbContextTransaction Transaction()
        {
            return _context.Database.BeginTransaction();
        }
    }
}
