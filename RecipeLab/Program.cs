
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RecipeLab.Features.Recipes;
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

            builder.Services.AddAuthorization();

            builder.Services.AddScoped<IRecipeService, RecipeService>();

            builder.Services
                .AddIdentityApiEndpoints<ApplicationUser>()
                .AddEntityFrameworkStores<RecipeLabDbContext>();

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

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.MapGroup("/api/Auth").MapIdentityApi<ApplicationUser>();

            app.Run();
        }
    }
}
