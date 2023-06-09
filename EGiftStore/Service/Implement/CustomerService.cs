﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Persistence.Entities;
using Persistence.ViewModel.Request;
using Persistence.ViewModel.Response;
using Repository;
using Service.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Service.Implement
{
    public class CustomerService : ICustomerService
    {
        private IMapper _mapper;
        private IConfiguration _configuration;
        private IUnitIOfWork _uow;


        public CustomerService(IUnitIOfWork unitIOfWork, IMapper mapper, IConfiguration configuration)
        {
            _configuration = configuration;
            _mapper = mapper;
            _uow = unitIOfWork;
        }

        public async Task<AuthenticationViewModel> AuthenticationAsync(Persistence.ViewModel.Request.AuthenticationLoginModel av)
        {
            var customer = await _uow.CustomerRepository.FirstOrDefaultAsync(x => x.IsActive && x.Username.Equals(av.Username) && x.Password.Equals(av.Password));
            if (customer != null)
            {
                return new AuthenticationViewModel
                {
                    AccessToken = GenerateToken(customer)
                };
            }
            return null!;
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

        public async Task<Customer> GetCustomerById(Guid id)
        {
            return await _uow.CustomerRepository.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public string GenerateToken(Customer customer)
        {
            var key = System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:SecretKey").Value ?? null!);
            var tokenDescribe = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[] {
                    new Claim("id", customer.Id.ToString()),
                }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Expires = DateTime.Now.AddDays(7)
            };
            var tokenHandle = new JwtSecurityTokenHandler();
            var token = tokenHandle.CreateToken(tokenDescribe);
            return tokenHandle.WriteToken(token);
        }

        public IActionResult GetCustomers()
        {
            return new JsonResult(_uow.CustomerRepository.GetAll().ProjectTo<CustomerViewModel>(_mapper.ConfigurationProvider));
        }
    }
}
