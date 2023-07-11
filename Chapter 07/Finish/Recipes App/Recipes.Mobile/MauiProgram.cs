﻿using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Recipes.Client.Core.Favorites;
using Recipes.Client.Core.Ratings;
using Recipes.Client.Core.Recipes;
using Recipes.Client.Core.ViewModels;

namespace Recipes.Mobile;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIconsRegular");
            });
		builder.UseMauiCommunityToolkit();


		builder.Services.AddTransient<RecipesOverviewPage>();
		builder.Services.AddTransient<RecipesOverviewViewModel>();

		builder.Services.AddTransient<RecipeDetailPage>();
		builder.Services.AddTransient<RecipeDetailViewModel>();
        
		builder.Services.AddTransient<RecipeRatingDetailPage>();
        builder.Services.AddTransient<RecipeRatingsDetailViewModel>();

        builder.Services.AddSingleton<IFavoritesService, FavoritesService>();

		builder.Services.AddSingleton<IRatingsService>(
            serviceProvider => new RatingsService(FileSystem.OpenAppPackageFileAsync("ratings.json")));

		builder.Services.AddTransient<IRecipeService>(
            serviceProvider => new RecipeService(FileSystem.OpenAppPackageFileAsync("recipedetails.json")));


       // FileSystem.OpenAppPackageFileAsync("recipes.json")

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}