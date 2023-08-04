using Recipes.Client.Core.ViewModels;

namespace Recipes.Mobile;

public partial class RecipesOverviewPage : ContentPage
{
    public RecipesOverviewPage(
		RecipesOverviewViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count != 0)
            Shell.Current.GoToAsync("RecipeDetail");
    }
}