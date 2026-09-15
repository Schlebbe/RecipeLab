using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeLab.Features.Ingredients;
using System.Security.Claims;

namespace RecipeLab.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class IngredientsController : ControllerBase
    {
        private readonly IIngredientService _ingredientService;

        public IngredientsController(IIngredientService ingredientService)
        {
            _ingredientService = ingredientService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<IngredientResponseDto>>> GetForUserAsync(CancellationToken cancellation)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var userIngredients = await _ingredientService.GetForUserAsync(userId, cancellation);

            return Ok(userIngredients);
        }

        [HttpPost]
        public async Task<ActionResult<IngredientResponseDto>> CreateForUserAsync(CreateIngredientRequestDto request, CancellationToken cancellation)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var ingredient = await _ingredientService.CreateForUserAsync(userId, request, cancellation);

            return CreatedAtAction(nameof(GetByIdAsync), new { id = ingredient.Id }, ingredient);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<IngredientResponseDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var ingredient = await _ingredientService.GetByIdForUserAsync(userId, id, cancellationToken);

            if (ingredient is null)
            {
                return NotFound();
            }

            return Ok(ingredient);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<IngredientResponseDto>> UpdateByIdAsync(Guid id, UpdateIngredientRequestDto request, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var updatedIngredient = await _ingredientService.UpdateForUserAsync(userId, id, request, cancellationToken);

            if (updatedIngredient is null)
            {
                return NotFound();
            }

            return Ok(updatedIngredient);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var result = await _ingredientService.DeleteForUserAsync(userId, id, cancellationToken);

            return result ? NoContent() : NotFound();
        }
    }
}
