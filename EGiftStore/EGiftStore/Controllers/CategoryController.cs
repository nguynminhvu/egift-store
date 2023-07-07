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
        private ICacheService _cacheService;
        private ICategoryService _categoryService;

        public CategoryController(ICategoryService category, ICacheService cacheService)
        {
            _cacheService = cacheService;
            _categoryService = category;
        }

        /// <summary>
        /// Create category
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthConfig("Admin")]
        public async Task<IActionResult> CreateCategory(string name)
        {
            var rs = await _categoryService.CreateCategory(name);
            string path = HttpContext.Request.Path.ToString();
            await _cacheService.RemoveCacheAsync(path);
            return rs != null ? StatusCode(StatusCodes.Status201Created) : StatusCode(StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Get categories
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [Cache(10000)]
        public async Task<IActionResult> GetCategories(string? name)
        {
            var rs = await _categoryService.GetCategories(name);
            if (rs is JsonResult jsonResult)
            {
                return Ok(jsonResult.Value);
            }
            return BadRequest();
        }

        /// <summary>
        /// Get category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        [Cache(10000)]
        public async Task<IActionResult> GetCategory([FromRoute] Guid id)
        {
            var rs = await _categoryService.GetCategory(id);
            return rs != null ? Ok(rs) : NotFound(new { Message = "No Category" });
        }

        /// <summary>
        /// Update category
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}")]
        [AuthConfig("Admin")]
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid id, string? name)
        {
            var rs = await _categoryService.UpdateCategory(id, name);
            await _cacheService.RemoveCacheAsync(HttpContext.Request.Path.ToString().Substring(0, HttpContext.Request.Path.ToString().LastIndexOf("/")));
            return Ok(rs);
        }

        /// <summary>
        /// Remove category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
                    case 204: await _cacheService.RemoveCacheAsync(HttpContext.Request.Path.ToString().Substring(0, HttpContext.Request.Path.ToString().LastIndexOf("/"))); return NoContent();
                    case 400: return BadRequest(new { Message = "CategoryId invalid" });
                    case 500: return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
            return BadRequest();
        }
    }
}
