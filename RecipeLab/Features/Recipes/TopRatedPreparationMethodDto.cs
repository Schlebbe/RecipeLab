namespace RecipeLab.Features.Recipes
{
    public sealed class TopRatedPreparationMethodDto
    {
        public string PreparationMethod { get; set; } = string.Empty;
        public double AverageRating { get; set; }
        public int ExperimentCount { get; set; }
    }
}
