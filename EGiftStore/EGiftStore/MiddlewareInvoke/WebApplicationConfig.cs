using EGiftStore.MiddlewareInvoke.Invoke;
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
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICartService, CartService>();
        }


        public static string GetSecretKey(this IConfiguration configuration)
        {
            return configuration.GetSection("AppSettings:SecretKey").Value ?? null!;
        }

        public static void AddSwagger(this IServiceCollection services)
        {
            var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            var secretKey = configuration.GetSecretKey();

            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Custom EGift", new OpenApiSecurityScheme
                {
                    Description = "Keep working",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey

                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                    new OpenApiSecurityScheme{
                    Reference= new OpenApiReference{
                    Id="Custom EGift",
                    Type=ReferenceType.SecurityScheme
                    }
                    },
                    new  List<string>()
                    }
                });
                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });
        }

        public static void UseJwt(this IApplicationBuilder app)
        {
            app.UseMiddleware<JwtMiddlewareInvoke>();
        }
    }
}
