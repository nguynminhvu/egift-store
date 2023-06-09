using EGiftStore.MiddlewareInvoke.Invoke;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistence.Entities;
using Persistence.ViewModel.Request;
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
        public async Task<IActionResult> GetCustomer(Guid id)
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
    }
}
