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
        public Task<CustomerViewModel> GetCustomerById(Guid id);
        public Task<AuthenticationViewModel> AuthenticationAsync(AuthenticationLoginModel av);
        public IActionResult GetCustomers();
        public Task<IActionResult> AcceptCustomer(Guid id);
        public Task<DateTime?> GetExpireToken(Guid id);
        public Task<IActionResult> UpdatePassword(Guid id, string password);
        public Task<IActionResult> UpdateCustomer(Guid id, CustomerUpdateModel cum);
        public Task<IActionResult> RemoveCustomer(Guid id);
    }
}
