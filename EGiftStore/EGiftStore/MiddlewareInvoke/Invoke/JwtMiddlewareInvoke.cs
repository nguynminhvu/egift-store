using EGiftStore.ThrowException;
using Microsoft.IdentityModel.Tokens;
using Service.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata;

namespace EGiftStore.MiddlewareInvoke.Invoke
{
    public class JwtMiddlewareInvoke
    {
        private static string CUSTOMER_ROLE = "Customer";
        private static string ADMIN_ROLE = "Admin";
        private RequestDelegate _next;
        private IConfiguration _config;

        public JwtMiddlewareInvoke(RequestDelegate request, IConfiguration configuration)
        {
            _next = request;
            _config = configuration;
        }

        public async Task Invoke(HttpContext context, ICustomerService customerService, IAdminService adminService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                await TokenHandle(context, customerService, token, adminService);
            }
            await _next(context);
        }

        public async Task TokenHandle(HttpContext context, ICustomerService customerService, string token, IAdminService adminService)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = System.Text.Encoding.UTF8.GetBytes(_config.GetSecretKey());
                tokenHandler.ValidateToken(token, new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken tokenHandled);
                var tokenJwt = (JwtSecurityToken)tokenHandled;
                var role = tokenJwt.Claims.First(x => x.Type == "role").Value;
                Guid id = Guid.Parse(tokenJwt.Claims.First(x => x.Type == "id").Value);
                if (role.Equals(CUSTOMER_ROLE))
                {
                    var expire = await customerService.GetExpireToken(id);
                    if (expire != null)
                    {
                        context.Items["Id"] = id;
                        context.Items["Expire"] = expire;
                        context.Items["Role"] = CUSTOMER_ROLE;
                    }
                }
                else if (role.Equals(ADMIN_ROLE))
                {
                    var expire = await adminService.GetExpireToken(id);
                    if (expire != null)
                    {
                        context.Items["AdminId"] = id;
                        context.Items["Expire"] = expire;
                        context.Items["Role"] = ADMIN_ROLE;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
    }
}
