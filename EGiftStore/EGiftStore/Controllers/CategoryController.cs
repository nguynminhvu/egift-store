using EGiftStore.MiddlewareInvoke.Invoke;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace EGiftStore.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private ICategoryService _categoryService;

        public CategoryController(ICategoryService category)
        {
            _categoryService = category;
        }


        [HttpPost]
        [AuthConfig("Admin")]
        public async Task<IActionResult> CreateCategory(string name)
        {
            var rs = await _categoryService.CreateCategory(name);
            return rs != null ? StatusCode(StatusCodes.Status201Created) : StatusCode(StatusCodes.Status500InternalServerError);
        }


        [HttpGet]
        public async Task<IActionResult> GetCategories(string? name)
        {
            var rs = await _categoryService.GetCategories(name);
            if (rs is JsonResult jsonResult)
            {
                return Ok(jsonResult.Value);
            }
            return BadRequest();
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetCategory([FromRoute] Guid id)
        {
            var rs = await _categoryService.GetCategory(id);
            return rs != null ? Ok(rs) : NotFound(new { Message = "No Category" });
        }


        [HttpPut]
        [Route("{id}")]
        [AuthConfig("Admin")]
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid id, string? name)
        {
            return Ok(await _categoryService.UpdateCategory(id, name));
        }


        [HttpDelete]
        [Route("{id}")]
        [AuthConfig("Admin")]
        public async Task<IActionResult> RemoveCategory([FromRoute] Guid id)
        {
            var rs = await _categoryService.RemoveCategory(id);
            if (rs is StatusCodeResult status)
            {
                switch (status.StatusCode)
                {
                    case 204: return NoContent();
                    case 400: return BadRequest(new { Message = "CategoryId invalid" });
                    case 500: return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
            return BadRequest();
        }
    }
}
