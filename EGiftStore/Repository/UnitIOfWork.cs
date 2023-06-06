using Persistence.Entities;
using Repository.Repositories.Implement;
using Repository.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class UnitIOfWork : IUnitIOfWork
    {
        private readonly EgiftShopContext _context;
        private  ICustomerRepository _customerRepository = null!;
        public UnitIOfWork(EgiftShopContext context)
        {
            _context = context;
        }
        public ICustomerRepository CustomerRepository { get { return _customerRepository ??= new CustomerRepository(_context); } }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
