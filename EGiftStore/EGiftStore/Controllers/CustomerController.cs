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
        public async Task<IActionResult> CustomerRegister(CustomerRegisterViewModel crvm)
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
    }
}
