using Microsoft.EntityFrameworkCore;
using RecipeLab.Features.Ingredients;
using RecipeLab.Features.RecipeIngredients;
using RecipeLab.Features.RecipeExperiments;
using RecipeLab.Features.Recipes;
using RecipeLab.Infrastructure.Identity;
using RecipeLab.Infrastructure.Persistence;

namespace RecipeLab.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRecipeLabServices(this IServiceCollection services, IConfiguration configuration)
        {
            var frontendOrigin = configuration["Frontend:Origin"] ??
                throw new InvalidOperationException("Frontend:Origin configuration is required.");

            services.AddCors(options =>
            {
                options.AddPolicy("Frontend", policy =>
                {
                    policy
                        .WithOrigins(frontendOrigin)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            services.AddDbContext<RecipeLabDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services
                .AddIdentityApiEndpoints<ApplicationUser>()
                .AddEntityFrameworkStores<RecipeLabDbContext>();

            services.AddScoped<IRecipeService, RecipeService>();
            services.AddScoped<IIngredientService, IngredientService>();
            services.AddScoped<IRecipeExperimentService, RecipeExperimentService>();
            services.AddScoped<IRecipeIngredientService, RecipeIngredientService>();

            return services;
        }
    }
}
