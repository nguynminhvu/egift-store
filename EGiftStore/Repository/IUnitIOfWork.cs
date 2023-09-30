using Microsoft.EntityFrameworkCore.Storage;
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
		public ICategoryRepository CategoryRepository { get; }
		public IProductImageRepository ProductImageRepository { get; }
		public ICartRepository CartRepository { get; }
		public ICartItemRepository CartItemRepository { get; }
		public IOrderDetailRepository OrderDetailRepository { get; }
		IOrderRepository OrderRepository { get; }
		Task<int> SaveChangesAsync();
        IDbContextTransaction Transaction();
    }
}
