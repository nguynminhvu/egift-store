using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Persistence.Entities;
using Persistence.ViewModel.Request;
using Persistence.ViewModel.Response;
using Repository;
using Service.Interface;


namespace Service.Implement
{
    public class CustomerService : ICustomerService
    {
        private IMapper _mapper;
        private IConfiguration _configuration;
        private IUnitIOfWork _uow;

        public CustomerService(IUnitIOfWork unitIOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _uow = unitIOfWork;
        }
        public async Task<IActionResult> CustomerRegisterAsync(CustomerRegisterViewModel model)
        {
            var customer = await _uow.CustomerRepository.FirstOrDefaultAsync(x => x.Username.Equals(model.Username));
            if (customer == null)
            {
                Customer newCustomer = new Customer
                {
                    Id = Guid.NewGuid(),
                    Username = model.Username,
                    Address = model.Address,
                    Email = model.Email,
                    Fullname = model.Fullname,
                    Password = model.Password,
                    Phone = model.Phone,
                    IsActive = true,
                    CreateDate = DateTime.Now,
                    LastUpdate = DateTime.Now
                };
                await _uow.CustomerRepository.AddAsync(newCustomer);
                return await _uow.SaveChangesAsync() > 0 ? new JsonResult(_mapper.Map<CustomerViewModel>(newCustomer)) : new StatusCodeResult(400);
            }
            return new StatusCodeResult(400);
        }
    }
}
