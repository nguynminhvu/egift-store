using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Persistence.Entities;
using Persistence.ViewModel.Response;
using System.Collections;

namespace EGiftStore.MiddlewareInvoke.Invoke
{
    public class AuthConfig : Attribute, IAuthorizationFilter
    {
        public ICollection<string> Roles { get; set; }
        private static readonly string CUSTOMER_ROLE = "Customer";
        public AuthConfig(params string[] role)
        {
            Roles = role.ToList();
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.Items["User"] as Customer;
            if (user == null)
            {
                context.Result = new JsonResult(new { Message = "Unauthorized" }) { StatusCode = 401 };
            }
            else
            {
                int expireToken = user.ExpireToken.AddDays(7).Day - DateTime.Now.Day;
                if (expireToken < 0)
                {
                    context.Result = new JsonResult(new { Message = "Token Expired" }) { StatusCode = 401 };
                }
                if (!Roles.Contains(CUSTOMER_ROLE))
                {
                    context.Result = new JsonResult(new { Message = "Forbidden" }) { StatusCode = 403 };
                }
            }
        }
    }
}
