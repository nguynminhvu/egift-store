using Persistence.Entities;
using Repository.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.Implement
{
	public class OrderRepository : Repository<Order>, IOrderRepository
	{
		public OrderRepository(EgiftShopContext egiftShop) : base(egiftShop)
		{
		}
	}
}
