using EGiftStore.MiddlewareInvoke.Invoke;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistence.Entities;
using Persistence.ViewModel.Request;
using Persistence.ViewModel.Response;
using Service.Interface;

namespace EGiftStore.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private static string CUSTOMER_ROLE = "Customer";
        private static string ADMIN_ROLE = "Admin";
        private ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterCustomer(CustomerRegisterViewModel crvm)
        {
            var rs = await _customerService.CustomerRegisterAsync(crvm);

            if (rs is JsonResult jsonResult)
            {
                return StatusCode(StatusCodes.Status201Created, jsonResult.Value);
            }
            if (rs is StatusCodeResult status)
            {
                if (status.StatusCode == 400) return BadRequest(new { Message = "Username already exist" });
                if (status.StatusCode == 500) return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return BadRequest();
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginCustomer(AuthenticationLoginModel alm)
        {
            var rs = await _customerService.AuthenticationAsync(alm);
            return rs != null ? Ok(rs) : BadRequest(new { Message = "Username or password invalid" });
        }


        [HttpGet]
        [Route("{id}")]
        [AuthConfig("Customer", "Admin")]
        public async Task<IActionResult> GetCustomer([FromRoute] Guid id)
        {
            var rs = await _customerService.GetCustomerById(id);
            return rs != null ? Ok(rs) : NotFound(new { Message = "No Customer" });
        }


        [HttpGet]
        [AuthConfig("Admin")]
        public IActionResult GetCustomers()
        {
            return Ok((_customerService.GetCustomers() as JsonResult)!.Value);
        }


        [HttpPut]
        [AuthConfig("Customer")]
        [Route("update-password")]
        public async Task<IActionResult> UpdatePassword(string password)
        {
            var idRaw = HttpContext.Items["Id"]!.ToString()!;
            if (idRaw != null)
            {
                Guid id = Guid.Parse(idRaw);
                var rs = await _customerService.UpdatePassword(id, password);
                if (rs is StatusCodeResult status)
                {
                    if (status.StatusCode == 400) { return Unauthorized(); }
                    if (status.StatusCode == 500) { return StatusCode(StatusCodes.Status500InternalServerError); }
                }
                if (rs is JsonResult json)
                {
                    return Ok(json.Value);
                }
            }
            return Unauthorized();
        }


        [HttpPut]
        [AuthConfig("Customer")]
        public async Task<IActionResult> UpdateCustomer(CustomerUpdateModel cum)
        {
            var idRaw = HttpContext.Items["Id"]!.ToString()!;
            if (idRaw != null)
            {
                var rs = await _customerService.UpdateCustomer(Guid.Parse(idRaw), cum);
                if (rs is StatusCodeResult status)
                {
                    if (status.StatusCode == 400) { return Unauthorized(); }
                }
                if (rs is JsonResult json)
                {
                    return Ok(json.Value);
                }
            }
            return Unauthorized();
        }


        [HttpDelete]
        [Route("{id}")]
        [AuthConfig("Admin")]
        public async Task<IActionResult> RemoveCustomer([FromRoute] Guid id)
        {
            var rs = await _customerService.RemoveCustomer(id);
            if (rs is StatusCodeResult status)
            {
                switch (status.StatusCode)
                {
                    case 204: return NoContent();
                    case 400: return Unauthorized();
                    case 500: return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
            return Unauthorized();
        }
    }
}
