using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Persistence.Entities;
using Persistence.ViewModel.Request;
using Persistence.ViewModel.Response;
using Repository;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implement
{
    public class AdminService : IAdminService
    {
        private IConfiguration _configuaration;
        private IMapper _mapper;
        private IUnitIOfWork _uow;

        public AdminService(IUnitIOfWork unitIOfWork, IMapper mapper, IConfiguration configuration)
        {
            _configuaration = configuration;
            _mapper = mapper;
            _uow = unitIOfWork;
        }



        public async Task<AuthenticationViewModel> AuthenticationAdmin(AuthenticationLoginModel avm)
        {
            var admin = await _uow.AdminRepository.FirstOrDefaultAsync(x => x.Username.Equals(avm.Username) && x.Password.Equals(avm.Password));
            if (admin!=null)
            {
                admin.ExpireToken = DateTime.Now;
                return await _uow.SaveChangesAsync() > 0 ? new AuthenticationViewModel { AccessToken = GenerateToken(admin) } : null!;
            }
            return null!;
        }

        public string GenerateToken(Admin admin)
        {
            var key = System.Text.Encoding.UTF8.GetBytes(_configuaration.GetSection("AppSettings:SecretKey").Value ?? null!);
            var tokenDescribe = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[] {
                    new Claim("id", admin.Id.ToString()),
                    new Claim("role", "Admin")
                }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Expires = DateTime.Now.AddDays(7)
            };
            var tokenHandle = new JwtSecurityTokenHandler();
            var token = tokenHandle.CreateToken(tokenDescribe);
            return tokenHandle.WriteToken(token);
        }

        public async Task<DateTime?> GetExpireToken(Guid id)
        {
            var expire = (await _uow.AdminRepository.FirstOrDefaultAsync(x => x.Id.Equals(id)) as Admin).ExpireToken;
            return expire != null ? expire! : null;
        }


    }
}
