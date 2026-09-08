
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RecipeLab.Infrastructure.Identity;
using RecipeLab.Infrastructure.Persistence;
using Scalar.AspNetCore;

namespace RecipeLab
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddDataProtection();

            builder.Services
                .AddIdentityCore<ApplicationUser>()
                .AddEntityFrameworkStores<RecipeLabDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            builder.Services.AddDbContext<RecipeLabDbContext>(options => {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
