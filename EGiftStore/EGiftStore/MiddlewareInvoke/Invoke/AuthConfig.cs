using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Persistence.Entities;
using Persistence.ViewModel.Response;
using Service.Interface;
using System.Collections;
using System.Text;

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
                return;
            }
            else
            {
                if (!Roles.Contains(role.ToString()!))
                {
                    context.Result = new JsonResult(new { Message = "Forbidden" }) { StatusCode = 403 };
                    return;
                }
            }
        }

    }
}
