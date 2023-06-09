using EGiftStore.MiddlewareInvoke.Invoke;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistence.Entities;
using Persistence.ViewModel.Request;
using Service.Interface;

namespace EGiftStore.Controllers
{
    [Route("api/customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterCustomer(CustomerRegisterViewModel crvm)
        {
            try
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginCustomer(AuthenticationLoginModel alm)
        {
            try
            {
                var rs = await _customerService.AuthenticationAsync(alm);
                return rs != null ? Ok(rs) : BadRequest(new { Message = "Username or password invalid" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }


        [HttpGet]
        [AuthConfig("Customer")]
        public IActionResult GetCustomers()
        {
            try
            {
                var user = HttpContext.Items["User"] as Customer;
                if (user != null)
                {
                    return Ok(_customerService.GetCustomers());
                }
                return StatusCode(StatusCodes.Status401Unauthorized, new { Message = "Unauthorized" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
