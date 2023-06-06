using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistence.Entities;
using Repository;
using Service.Implement;
using Service.Interface;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

namespace EGiftStore.MiddlewareInvoke
{
    public static class WebApplicationConfig
    {
        public static void AddDependenceInjection(this IServiceCollection services)
        {
            services.AddScoped<IUnitIOfWork, UnitIOfWork>();
            services.AddScoped<ICustomerService, CustomerService>();
        }
        public static string GetSecretKey(this IConfiguration configuration)
        {
            return configuration.GetSection("AppSettings:SecretKey").Value ?? null!;
        }
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(x =>
            {
                x.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Type Bearer and space then paste the token.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                x.OperationFilter<SecurityRequirementsOperationFilter>();
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                  //  IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(GetSecretKey()))
                };
            });
        }
    }
}
