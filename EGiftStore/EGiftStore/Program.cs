using EGiftStore.MiddlewareInvoke;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;
using Persistence.Mapper;

namespace EGiftStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddSwaggerGen();
            builder.Services.AddSwagger();
            builder.Services.AddControllers();
            builder.Services.AddDbContext<EgiftShopContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("EGiftDb"))
            );
            builder.Services.AddDependenceInjection();
            builder.Services.AddAutoMapper(typeof(MapperConfig));
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.RoutePrefix = string.Empty;
                });
            }
            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseJwt();

            app.MapControllers();

            app.Run();
        }
    }
}