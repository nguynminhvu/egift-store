using Microsoft.AspNetCore.Mvc;
using Persistence.ViewModel.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
	public interface ICartService
	{
		public Task<IActionResult> AddToCart(Guid customerId, AddToCartModel atc);
		public Task<IActionResult> GetCartItems(Guid customerId);
		public Task<IActionResult> UpdateCart(CartUpdateModel cum);
		public Task<bool> ClearCart(Guid cartId);
	}
}
