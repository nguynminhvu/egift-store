using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Persistence.Entities;
using Persistence.ViewModel.Request;
using Persistence.ViewModel.Response;
namespace Service.Interface
{
    public interface ICustomerService
    {
        public Task<IActionResult> CustomerRegisterAsync(CustomerRegisterViewModel model);
        public Task<Customer> GetCustomerById(Guid id);
        public Task<AuthenticationViewModel> AuthenticationAsync(AuthenticationLoginModel av);
        public IActionResult GetCustomers();

    }
}
