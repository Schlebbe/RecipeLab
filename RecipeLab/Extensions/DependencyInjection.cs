using Microsoft.EntityFrameworkCore;
using RecipeLab.Features.Recipes;
using RecipeLab.Infrastructure.Identity;
using RecipeLab.Infrastructure.Persistence;

namespace RecipeLab.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRecipeLabServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<RecipeLabDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services
                .AddIdentityApiEndpoints<ApplicationUser>()
                .AddEntityFrameworkStores<RecipeLabDbContext>();

            services.AddScoped<IRecipeService, RecipeService>();

            return services;
        }
    }
}
