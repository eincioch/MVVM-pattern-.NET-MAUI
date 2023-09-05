namespace Recipes.Client.Core.Navigation;

public interface INavigatedFrom
{
    Task OnNavigatedFrom(NavigationType navigationType);
}

public interface INavigatable
{
    Task<bool> CanNavigateFrom(NavigationType navigationType);
}