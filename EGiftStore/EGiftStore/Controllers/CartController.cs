using EGiftStore.MiddlewareInvoke.Invoke;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Persistence.ViewModel.Request;
using Service.Interface;

namespace EGiftStore.Controllers
{
    [Route("api/carts")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost]
        [AuthConfig("Customer")]
        public async Task<IActionResult> AddToCart(AddToCartModel acm)
        {
            var idRaw = HttpContext.Items["Id"];
            if (idRaw != null)
            {
                Guid id = Guid.Parse(idRaw.ToString()!);
                var rs = await _cartService.AddToCart(id, acm);
                if (rs is JsonResult jsonResult)
                {
                    return Ok(jsonResult.Value);
                }
                if (rs is StatusCodeResult status)
                {
                    return new StatusCodeResult(status.StatusCode);
                }
            }
            return Unauthorized(new { Message = "Unauthorized" });
        }


        [HttpPut]
        [AuthConfig("Customer")]
        public async Task<IActionResult> UpdateCart(CartUpdateModel cum)
        {
            var idRaw = HttpContext.Items["Id"];
            if (idRaw != null)
            {
                var rs = await _cartService.UpdateCart(cum);
                if (rs is JsonResult jsonResult)
                {
                    return Ok(jsonResult.Value);
                }
                if (rs is StatusCodeResult status)
                {
                    return new StatusCodeResult(status.StatusCode);
                }
            }
            return Unauthorized(new { Message = "Unauthorized" });
        }


        [HttpGet]
        [AuthConfig("Customer")]
        public async Task<IActionResult> GetCartItem()
        {
            var idRaw = HttpContext.Items["Id"];
            if (idRaw != null)
            {
                Guid id = Guid.Parse(idRaw.ToString()!);
                var rs = await _cartService.GetCartItems(id);
                if (rs is JsonResult jsonResult)
                {
                    return Ok(jsonResult.Value);
                }
            }
            return Unauthorized(new { Message = "Unauthorized" });
        }
    }
}
