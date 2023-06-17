using Persistence.ViewModel.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
	public interface IOrderDetailService
	{
		public Task<bool> CreateOrderDetail(Guid orderId, CartViewModel cartViewModel);
	}
}
