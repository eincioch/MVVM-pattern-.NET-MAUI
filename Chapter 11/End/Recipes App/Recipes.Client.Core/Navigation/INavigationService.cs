using Recipes.Client.Core.Features.Recipes;

namespace Recipes.Client.Core.Navigation;

public interface INavigationService
{
    Task GoToOverview();
    Task GoToRecipeDetail(string recipeId);
    Task GoToRecipeRatingDetail(RecipeDetail recipe);
    Task GoToAddRating(RecipeDetail recipe);
    Task GoBack();
}
