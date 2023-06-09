using Microsoft.AspNetCore.Mvc;
using Persistence.ViewModel.Request;
using Persistence.ViewModel.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IAdminService
    {
        public Task<AuthenticationViewModel> AuthenticationAdmin(AuthenticationLoginModel avm);
        public  Task<DateTime?> GetExpireToken(Guid id);
    }
}
