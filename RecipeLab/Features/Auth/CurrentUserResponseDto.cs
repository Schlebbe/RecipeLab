namespace RecipeLab.Features.Auth
{
    public sealed class CurrentUserResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
