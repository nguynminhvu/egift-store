using Repository.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IUnitIOfWork
    {
        public ICustomerRepository CustomerRepository { get; }
        public IAdminRepository AdminRepository { get; }
        public IProductRepository ProductRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
