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
        public AuthConfig(params string[] role)
        {
            Roles = role.ToList();
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var role = context.HttpContext.Items["Role"];
            var expiredRaw = context.HttpContext.Items["Expire"];
            if (role == null || expiredRaw == null)
            {
                context.Result = new JsonResult(new { Message = "Unauthorized" }) { StatusCode = 401 };
            }
            else
            {
                var expired = (DateTime)(expiredRaw);
                int expireToken = expired.AddDays(7).Day - DateTime.Now.Day;
                if (expireToken < 0)
                {
                    context.Result = new JsonResult(new { Message = "Token Expired" }) { StatusCode = 401 };
                }
                if (!Roles.Contains(role.ToString()!))
                {
                    context.Result = new JsonResult(new { Message = "Forbidden" }) { StatusCode = 403 };
                }
            }
        }
    }
}
