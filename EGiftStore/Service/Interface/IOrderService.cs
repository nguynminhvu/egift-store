using Microsoft.AspNetCore.Mvc;
using Persistence.ViewModel.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
	public interface IOrderService
	{
		public Task<IActionResult> CreateOrder(Guid customerId, CartViewModel cvm);
		public Task<IActionResult> UpdateOrder(Guid orderId, string status);
		public Task<OrderViewModel> GetOrderById(Guid id);
		public Task<List<OrderViewModel>> GetOrders(Guid customerId);
	}
}
