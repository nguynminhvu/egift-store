using EGiftStore.MiddlewareInvoke.Invoke;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistence.Entities;
using Persistence.ViewModel.Response;
using Service.Interface;
using System.ComponentModel.DataAnnotations;

namespace EGiftStore.Controllers
{
	[Route("api/orders")]
	[ApiController]
	public class OrderController : ControllerBase
	{
		private ICartService _cartService;
		private IOrderService _orderService;
		private IOrderDetailService _orderDetailService;
		private IProductService _productService;

		public OrderController(IOrderService orderService, IOrderDetailService orderDetailService, IProductService productService, ICartService cartService)
		{
			_cartService = cartService;
			_orderService = orderService;
			_orderDetailService = orderDetailService;
			_productService = productService;
		}

		[HttpPost]
		[AuthConfig("Customer")]
		public async Task<IActionResult> CreateOrder(Guid cartId)
		{
			var id = HttpContext.Items["Id"]!.ToString();
			if (id != null)
			{
				Guid customerId = Guid.Parse(id);
				var cart = (await _cartService.GetCartItems(customerId) as JsonResult)!.Value as CartViewModel;
				if (cart == null || cart.CartItems.Count < 0)
				{
					return BadRequest(new { Message = "cart empty" });

				}
				if (await _productService.CheckStock(cart) is false)
				{
					return BadRequest(new { Message = "quantity product out of stock" });
				}
				if (await _cartService.ClearCart(cartId) is false)
				{
					return BadRequest(new { Message = "problem with cart" });
				}
				var orderJson = await _orderService.CreateOrder(customerId, cart);
				if (orderJson is JsonResult jsonResult)
				{
					var order = jsonResult.Value as OrderViewModel;
					return await _orderDetailService.CreateOrderDetail(order!.Id, cart) ? new JsonResult(await _orderService.GetOrderById(order.Id)) : StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Can't create Order Detail" }); ;
				}
				return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Can't create Order" });
			}
			return Unauthorized(new { Message = "Unauthorized" });
		}


		[HttpGet]
		[AuthConfig("Customer")]
		public async Task<IActionResult> GetOrderByCustomerId()
		{
			var id = HttpContext.Items["Id"]!.ToString();
			if (id == null)
			{
				return Unauthorized(new { Message = "Unauthorized" });
			}
			var rs = await _orderService.GetOrders(Guid.Parse(id));
			return rs != null ? new JsonResult(rs) : NotFound(new { Message = "no order" });
		}


		[HttpPut]
		[AuthConfig("Admin")]
		[Route("{id}")]
		public async Task<IActionResult> UpdateOrder([FromRoute] Guid id, [Required] string status)
		{
			var adminId = HttpContext.Items["AdminId"]!.ToString();
			if (adminId == null)
			{
				return Unauthorized(new { Message = "Unauthorized" });
			}
			var rs = await _orderService.UpdateOrder(id, status);
			if (rs is JsonResult jsonResult)
			{
				return Ok(jsonResult.Value);
			}
			return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Can't update order" });
		}
	}
}
