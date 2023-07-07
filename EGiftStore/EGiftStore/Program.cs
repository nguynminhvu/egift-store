using EGiftStore.MiddlewareInvoke;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistence.Entities;
using Persistence.Mapper;
using Swashbuckle.AspNetCore.Filters;

namespace EGiftStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddSwagger();
            builder.Services.AddControllers();
            builder.Services.AddDbContext<EgiftShopContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("EGiftDb"))
            );
            builder.Services.AddDependenceInjection(builder.Configuration);
            builder.Services.AddAutoMapper(typeof(MapperConfig));
            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseJwt();

            app.MapControllers();

            app.Run();
        }
    }
}