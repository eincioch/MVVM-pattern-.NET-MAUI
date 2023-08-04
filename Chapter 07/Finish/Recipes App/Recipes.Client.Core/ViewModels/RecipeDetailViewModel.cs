﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Recipes.Client.Core.Favorites;
using Recipes.Client.Core.Ratings;
using Recipes.Client.Core.Recipes;
using System.Collections.ObjectModel;

namespace Recipes.Client.Core.ViewModels;

public partial class RecipeDetailViewModel : ObservableObject
{
    private readonly IRecipeService recipeService;
    private readonly IFavoritesService favoritesService;
    private readonly IRatingsService ratingsService;

    string recipeId;

    private bool _hideAllergenInformation = true;

    string _title;
    public string Title 
    { 
        get => _title; 
        set => SetProperty(ref _title, value);
    }

    string[] _allergens = new string[0];
    public string[] Allergens
    { 
        get => _allergens; 
        set => SetProperty(ref _allergens, value);
    }

    int? _calories;
    public int? Calories 
    {
        get => _calories; 
        set => SetProperty(ref _calories, value);
    }

    int? _readyInMinutes;
    public int? ReadyInMinutes 
    {
        get => _readyInMinutes;
        set => SetProperty(ref _readyInMinutes, value);
    }

    DateTime _lastUpdated;
    public DateTime LastUpdated 
    {
        get => _lastUpdated; 
        set => SetProperty(ref _lastUpdated, value);
    }

    string _author;
    public string Author 
    {
        get => _author; 
        set => SetProperty(ref _author, value);
    }

    string _image;
    public string Image 
    { 
        get => _image;
        set => SetProperty(ref _image, value);
    }

    RecipeRatingsSummaryViewModel _ratingSummary;
    public RecipeRatingsSummaryViewModel RatingSummary 
    {
        get => _ratingSummary;
        set => SetProperty(ref _ratingSummary, value);
    }

    List<InstructionBaseViewModel> _instructions;
    public List<InstructionBaseViewModel> Instructions 
    {
        get => _instructions;
        set => SetProperty(ref _instructions, value);
    }

    IngredientsListViewModel _ingredientsList;
    public IngredientsListViewModel IngredientsList
    {
        get => _ingredientsList;
        set => SetProperty(ref _ingredientsList, value);
    }

    public bool HideAllergenInformation
    {
        get => _hideAllergenInformation;
        set => SetProperty(ref _hideAllergenInformation, value);
    }

    private bool _isFavorite = false;
    public bool IsFavorite
    {
        get => _isFavorite;
        private set
        {
            if (SetProperty(ref _isFavorite, value))
            {
                AddAsFavoriteCommand.NotifyCanExecuteChanged();
                RemoveAsFavoriteCommand.NotifyCanExecuteChanged();
            }
        }
    }

    public ObservableCollection<RecipeIngredientViewModel> ShoppingList { get; } = new();

    public IRelayCommand AddAsFavoriteCommand { get; }
    public IRelayCommand RemoveAsFavoriteCommand { get; }
    public IRelayCommand AddToShoppingListCommand { get; }
    public IRelayCommand RemoveFromShoppingListCommand { get; }
    public IRelayCommand UserIsBrowsingCommand { get; }

    public RecipeDetailViewModel(IRecipeService recipeService, 
        IFavoritesService favoritesService, 
        IRatingsService ratingsService)
    {
        this.recipeService = recipeService;
        this.favoritesService  = favoritesService;
        this.ratingsService = ratingsService;

        AddAsFavoriteCommand =
               new AsyncRelayCommand(AddAsFavorite, CanAddAsFavorite);
        RemoveAsFavoriteCommand = 
            new AsyncRelayCommand(RemoveAsFavorite, CanRemoveAsFavorite);
        UserIsBrowsingCommand = new RelayCommand(UserIsBrowsing);
        AddToShoppingListCommand = new RelayCommand<RecipeIngredientViewModel>(AddToShoppingList);
        RemoveFromShoppingListCommand = new RelayCommand<RecipeIngredientViewModel>(RemoveFromShoppingList);

        LoadRecipe("3");
    }

    private async Task LoadRecipe(string recipeId)
    {
        var loadRecipeTask = recipeService.LoadRecipe(recipeId);
        var loadIsFavoriteTask = favoritesService.IsFavorite(recipeId);
        var loadRatingsTask = ratingsService.LoadRatingsSummary(recipeId);

        await Task.WhenAll(loadRecipeTask, loadRecipeTask, loadRatingsTask);

        if(loadRecipeTask.Result is not null)
            MapRecipeData(loadRecipeTask.Result, loadRatingsTask.Result, loadIsFavoriteTask.Result);
    }

    private void MapRecipeData(RecipeDetailDto recipe, RatingsSummaryDto ratings, bool isFavorite)
    {
        recipeId = recipe.Id;
        Title = recipe.Name;
        Allergens = recipe.Allergens;
        Calories =  recipe.Calories;
        ReadyInMinutes = recipe.ReadyInMinutes;
        LastUpdated = recipe.LastUpdated;
        Author = recipe.Author;
        Image = recipe.Image ?? "fallback.png";

        var instructionVMs = new List<InstructionBaseViewModel>();
        foreach (var item in recipe.Instructions)
        {
            if (item.IsNote)
                instructionVMs.Add(new NoteViewModel(item.Text));
            else
                instructionVMs.Add(new InstructionViewModel(item.Index ?? 0, item.Text));
        }

        Instructions = instructionVMs;

        IngredientsList = new IngredientsListViewModel(recipe.Ingredients);

        IsFavorite = isFavorite;

        RatingSummary = new RecipeRatingsSummaryViewModel(ratings.TotalReviews, ratings.AverageRating, ratings.MaxRating);
    }

    private Task AddAsFavorite()
    {
        IsFavorite = true;
        return favoritesService.Add(recipeId);
    }

    private bool CanAddAsFavorite() => !IsFavorite;

    private Task RemoveAsFavorite()
    {
        IsFavorite = false;
        return favoritesService.Remove(recipeId);
    }

    private bool CanRemoveAsFavorite() => IsFavorite;

    private void UserIsBrowsing()
    {
        //Do Logging
    }

    private void AddToShoppingList(RecipeIngredientViewModel viewModel)
    {
        if (ShoppingList.Contains(viewModel))
            return;
        ShoppingList.Add(viewModel);
    }

    private void RemoveFromShoppingList(RecipeIngredientViewModel viewModel)
    {
        if (ShoppingList.Contains(viewModel))
            ShoppingList.Remove(viewModel);
    }
}
