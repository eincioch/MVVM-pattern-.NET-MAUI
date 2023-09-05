﻿using Recipes.Client.Core.Features.Recipes;
using Recipes.Client.Core.Navigation;

namespace Recipes.Mobile.Navigation;

public class NavigationService : INavigationService, INavigationInterceptor
{
    public Task GoToRecipeDetail(string recipeId)
        => Navigate("RecipeDetail", 
            new () { { "id", recipeId } });

    public Task GoToRecipeRatingDetail(RecipeDetailDto recipe)
        => Navigate("RecipeRating",
            new() { { "recipe", recipe } });

    public Task GoToAddRating(RecipeDetailDto recipe)
        => Navigate("AddRating",
           new() { { "recipe", recipe } });

    public Task GoBack()
        => Shell.Current.GoToAsync("..");

    private async Task Navigate(string pageName,
        Dictionary<string, object> parameters)
    {
        await Shell.Current.GoToAsync(pageName);

        if (Shell.Current.CurrentPage.BindingContext
            is INavigationParameterReceiver receiver)
        {
            await receiver.OnNavigatedTo(parameters);
        }
    }

    WeakReference<INavigatedFrom> previousFrom;
    public async Task OnNavigatedTo(object bindingContext,
        NavigationType navigationType)
    {
        if (previousFrom is not null && previousFrom
            .TryGetTarget(out INavigatedFrom from))
        {
            await from.OnNavigatedFrom(navigationType);
        }

        if (bindingContext
            is INavigatedTo to)
        {
            await to.OnNavigatedTo(navigationType);
        }

        if (bindingContext is INavigatedFrom navigatedFrom)
            previousFrom = new(navigatedFrom);
        else
            previousFrom = null;
    }

    public Task GoToOverview()
    {
        throw new NotImplementedException();
    }

    public Task<bool> CanNavigate(object bindingContext, NavigationType type)
    {
        if(bindingContext is INavigatable navigatable)
            return navigatable.CanNavigateFrom(type);

        return Task.FromResult(true);
    }
}