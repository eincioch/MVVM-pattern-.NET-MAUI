using Recipes.Client.Core.ViewModels;

namespace Recipes.Mobile;

public partial class RecipeDetailPage : ContentPage
{
	public RecipeDetailPage()
	{
        Application.Current.UserAppTheme = AppTheme.Light;

        InitializeComponent();
		BindingContext = new RecipeDetailViewModel();

		//lblTitle.SetBinding(Label.TextProperty,
		//	nameof(RecipeDetailViewModel.Title),
		//	BindingMode.OneTime);
	}
}