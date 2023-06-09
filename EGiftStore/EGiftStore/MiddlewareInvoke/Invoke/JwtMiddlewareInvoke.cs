using Microsoft.IdentityModel.Tokens;
using Service.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata;

namespace EGiftStore.MiddlewareInvoke.Invoke
{
    public class JwtMiddlewareInvoke
    {
        private RequestDelegate _next;
        private IConfiguration _config;

        public JwtMiddlewareInvoke(RequestDelegate request, IConfiguration configuration)
        {
            _next = request;
            _config = configuration;
        }
        public async Task Invoke(HttpContext context, ICustomerService customerService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                await TokenHandle(context, customerService, token);
            }
            await _next(context);
        }
        public async Task TokenHandle(HttpContext context, ICustomerService customerService, string token)
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
                var user = await customerService.GetCustomerById(Guid.Parse(tokenJwt.Claims.First(x => x.Type == "id").Value));
                context.Items["User"] = user;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
    }
}
