using Microsoft.AspNetCore.Mvc;
using Persistence.ViewModel.Request;
namespace Service.Interface
{
    public interface ICustomerService
    {
        public Task<IActionResult> CustomerRegisterAsync(CustomerRegisterViewModel model);
    }
}
