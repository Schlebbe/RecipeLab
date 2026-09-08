
using RecipeLab.Extensions;
using RecipeLab.Infrastructure.Identity;
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
            builder.Services.AddOpenApi();
            builder.Services.AddDataProtection();
            builder.Services.AddAuthorization();

            // Add RecipeLab specific services
            builder.Services.AddRecipeLabServices(builder.Configuration);

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
