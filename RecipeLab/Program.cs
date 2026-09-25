
using RecipeLab.Extensions;
using RecipeLab.Infrastructure.Identity;
using Scalar.AspNetCore;

namespace RecipeLab
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Allow async suffix in action names to avoid name conflicts with async methods in controllers
            builder.Services.AddControllers(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;
            });

            // Add services to the container.

            builder.Services.AddOpenApi();
            builder.Services.AddDataProtection();
            builder.Services.AddAuthorization();

            // Add RecipeLab specific services
            builder.Services.AddRecipeLabServices(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.MapOpenApi();
            app.MapScalarApiReference();

            if (app.Environment.IsDevelopment())
            {
                await app.SeedDevelopmentDataAsync();
            }

            app.UseHttpsRedirection();

            app.UseCors("Frontend");

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.MapGroup("/api/Auth").MapIdentityApi<ApplicationUser>();

            app.Run();
        }
    }
}
