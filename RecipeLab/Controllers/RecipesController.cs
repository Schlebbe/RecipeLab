using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeLab.Features.Recipes;
using System.Security.Claims;

namespace RecipeLab.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RecipesController : ControllerBase
    {
        private readonly IRecipeService _recipeService;

        public RecipesController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<RecipeResponseDto>>> GetForUserAsync(CancellationToken cancellation)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var userRecipes = await _recipeService.GetForUserAsync(userId, cancellation);

            return Ok(userRecipes);
        }

        [HttpPost]
        public async Task<ActionResult<RecipeResponseDto>> CreateForUserAsync(CreateRecipeRequestDto request, CancellationToken cancellation)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var recipe = await _recipeService.CreateForUserAsync(userId, request, cancellation);

            return CreatedAtAction(nameof(GetByIdAsync), new { id = recipe.Id }, recipe);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<RecipeResponseDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var recipe = await _recipeService.GetByIdForUserAsync(userId, id, cancellationToken);

            if (recipe is null)
            {
                return NotFound();
            }

            return Ok(recipe);
        }
    }
}
