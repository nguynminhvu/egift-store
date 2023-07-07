using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistence.ViewModel.Request;
using Service.Interface;

namespace EGiftStore.Controllers
{
    [Route("api/admins")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private IAdminService _adminService;

        public AdminController(IAdminService admin)
        {
            _adminService = admin;
        }

        /// <summary>
        /// Login admin account
        /// </summary>
        /// <param name="am">Username and password</param>
        /// <returns>Accress Token</returns>
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> AdminLogin(AuthenticationLoginModel am)
        {
            var rs = await _adminService.AuthenticationAdmin(am);
            return rs != null ? Ok(rs) : BadRequest(new { Message = "Username or password invalid" });
        }


    }
}
