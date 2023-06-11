using EGiftStore.MiddlewareInvoke.Invoke;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetProduct([FromRoute] Guid id)
        {
            var rs = await _productService.GetProduct(id);
            return rs != null ? Ok(rs) : NotFound(new { Message = "No Product" });
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] ProductFilterModel productFilterModel)
        {
            var rs = await _productService.GetProducts(productFilterModel);
            if (rs is JsonResult jsonResult)
            {
                return Ok(jsonResult.Value);
            }
            return Ok();
        }

        [HttpGet]
        [Route("category-id/{id}")]
        public async Task<IActionResult> GetProductsByCategoryId(Guid id)
        {
            var rs = await _productService.GetProductsByCategory(id);
            if (rs is JsonResult jsonResult)
            {
                return jsonResult.Value != null ? Ok(jsonResult.Value) : NotFound(new { Message = "No Product" });
            }
            return BadRequest();
        }

        [HttpPost]
        //  [AuthConfig("Admin")]
        public async Task<IActionResult> CreateProduct(ProductCreateModel productCreateModel)
        {
            var rs = await _productService.CreateProduct(productCreateModel);
            if (rs is JsonResult jsonResult)
            {
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

        [HttpPut]
        [Route("{id}")]
        //  [AuthConfig("Admin")]
        public async Task<IActionResult> UpdateProduct([FromRoute] Guid id, ProductUpdateModel productUpdateModel)
        {
            var rs = await _productService.UpdateProduct(id, productUpdateModel);
            if (rs is JsonResult jsonResult)
            {
                return Ok(jsonResult.Value);
            }
            if (rs is StatusCodeResult status)
            {
                if (status.StatusCode == 500) { return StatusCode(StatusCodes.Status500InternalServerError); }
                if (status.StatusCode == 400) { return BadRequest(new { Message = "ProductId invalid" }); }
            }
            return BadRequest();
        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> RemoveProduct([FromRoute] Guid id)
        {
            var rs = await _productService.RemoveProduct(id);
            if (rs is StatusCodeResult status)
            {
                switch (status.StatusCode)
                {
                    case 204: return NoContent();
                    case 400: return BadRequest(new { Message = "ProductId invalid" });
                    case 500: return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
            return BadRequest();
        }
    }
}
