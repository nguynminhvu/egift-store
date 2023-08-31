using EGiftStore.Configuration;
using EGiftStore.MiddlewareInvoke.Invoke;
using Microsoft.OpenApi.Models;
using Repository;
using Service.Implement;
using Service.Interface;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;

namespace EGiftStore.MiddlewareInvoke
{
    public static class WebApplicationConfig
    {
        public static void AddDependenceInjection(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConfiguration = new RedisConfiguration();
            var redisClusterConfiguration = new RedisClusterConfiguration();

            // Cluster
            // Binding value from Section RedisClusterConfiguration to Object type RedisClusterConfiguration
            configuration.GetSection("RedisClusterConfiguration").Bind(redisClusterConfiguration);

            // Single
            // Binding value from Section RedisConfiguration to Object type RedisConfiguration
            configuration.GetSection("RedisConfiguration").Bind(redisConfiguration);

            // Check binding enable
            services.AddSingleton(redisConfiguration);
            if (!redisConfiguration.Enable || !redisClusterConfiguration.Enable)
            {
                return;
            }
            Console.WriteLine("ConnectionString Redis: " + redisConfiguration.ConnectionString);


            #region Cluster
            foreach (var item in redisClusterConfiguration.RedisClusters)
            {
                services.AddSingleton<IConnectionMultiplexer>(x => ConnectionMultiplexer.Connect(item));
                services.AddStackExchangeRedisCache(option => option.Configuration = item);
            }
            #endregion


            #region Single
            //  services.AddSingleton<IConnectionMultiplexer>(x => ConnectionMultiplexer.Connect(redisConfiguration.ConnectionString));
            //  services.AddStackExchangeRedisCache(option => option.Configuration = redisConfiguration.ConnectionString);
            #endregion
            services.AddSingleton<ICacheService, CacheService>();
            services.AddScoped<IUnitIOfWork, UnitIOfWork>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderDetailService, OrderDetailService>();
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

                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
          $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
            });
        }

        public static void UseJwt(this IApplicationBuilder app)
        {
            app.UseMiddleware<JwtMiddlewareInvoke>();
        }
    }
}
