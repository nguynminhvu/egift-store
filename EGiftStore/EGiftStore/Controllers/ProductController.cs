using EGiftStore.MiddlewareInvoke.Invoke;
using EGiftStore.RateLimit.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Persistence.ViewModel.Request;
using Service.Interface;

namespace EGiftStore.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private static string CUSTOMER_ROLE = "Customer";
        private static string ADMIN_ROLE = "Admin";
        private ICacheService _cacheService;
        private IProductService _productService;

        public ProductController(IProductService productService, ICacheService cacheService)
        {
            _cacheService = cacheService;
            _productService = productService;
        }

        /// <summary>
        /// Get product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetProduct([FromRoute] Guid id)
        {
            var rs = await _productService.GetProduct(id);
            return rs != null ? Ok(rs) : NotFound(new { Message = "No Product" });
        }

        /// <summary>
        /// Get products
        /// </summary>
        /// <param name="productFilterModel"></param>
        /// <returns></returns>
        [HttpGet]
        [Cache(10000)]
        [EnableRateLimiting(RateLimitOption.MyLimiter)]
        public async Task<IActionResult> GetProducts([FromQuery] ProductFilterModel productFilterModel)
        {
            var rs = await _productService.GetProducts(productFilterModel);
            if (rs is JsonResult jsonResult)
            {
                return Ok(jsonResult.Value);
            }
            return Ok();
        }

        /// <summary>
        /// Get products by category id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("category-id/{id}")]
        [Cache(10000)]
        public async Task<IActionResult> GetProductsByCategoryId(Guid id)
        {
            var rs = await _productService.GetProductsByCategory(id);
            if (rs is JsonResult jsonResult)
            {
                return jsonResult.Value != null ? Ok(jsonResult.Value) : NotFound(new { Message = "No Product" });
            }
            return BadRequest();
        }

        /// <summary>
        /// Create new product
        /// </summary>
        /// <param name="productCreateModel"></param>
        /// <returns></returns>
        [HttpPost]
        //  [AuthConfig("Admin")]
        public async Task<IActionResult> CreateProduct([FromQuery]ProductCreateModel productCreateModel)
        {
            var rs = await _productService.CreateProduct(productCreateModel);
            if (rs is JsonResult jsonResult)
            {
                await _cacheService.RemoveCacheAsync(HttpContext.Request.Path.ToString());
                return StatusCode(StatusCodes.Status201Created, jsonResult.Value);
            }
            if (rs is StatusCodeResult status)
            {
                if (status.StatusCode == 400)
                {
                    return BadRequest(new { Message = "CategoryId and ProductImage is required" });
                }
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Update product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productUpdateModel"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}")]
        //  [AuthConfig("Admin")]
        public async Task<IActionResult> UpdateProduct([FromRoute] Guid id, ProductUpdateModel productUpdateModel)
        {
            var rs = await _productService.UpdateProduct(id, productUpdateModel);
            if (rs is JsonResult jsonResult)
            {
                await _cacheService.RemoveCacheAsync(HttpContext.Request.Path.ToString());
                return Ok(jsonResult.Value);
            }
            if (rs is StatusCodeResult status)
            {
                if (status.StatusCode == 500) { return StatusCode(StatusCodes.Status500InternalServerError); }
                if (status.StatusCode == 400) { return BadRequest(new { Message = "ProductId invalid" }); }
            }
            return BadRequest();
        }

        /// <summary>
        /// Remove product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> RemoveProduct([FromRoute] Guid id)
        {
            var rs = await _productService.RemoveProduct(id);
            if (rs is StatusCodeResult status)
            {
                switch (status.StatusCode)
                {
                    case 204: await _cacheService.RemoveCacheAsync(HttpContext.Request.Path.ToString()); return NoContent();
                    case 400: return BadRequest(new { Message = "ProductId invalid" });
                    case 500: return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
            
            return BadRequest();
        }
    }
}
