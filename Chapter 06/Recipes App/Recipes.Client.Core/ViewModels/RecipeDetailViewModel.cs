﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Recipes.Client.Core.ViewModels;

public partial class RecipeDetailViewModel : ObservableObject
{
    public string Title { get; set; } = "Classic Caesar Salad";

    public string[] Allergens { get; set; }
        = new string[] { "Milk", "Eggs", "Nuts", "Sesame" };

    private bool _hideAllergenInformation = true;
    public bool HideAllergenInformation
    {
        get => _hideAllergenInformation;
        set => SetProperty(ref _hideAllergenInformation, value);
    }

    public int? Calories { get; set; } = 240;

    public int? ReadyInMinutes { get; set; } = 30;

    public DateTime LastUpdated { get; set; }
        = new DateTime(2020, 7, 3);

    public string Author { get; set; } = "Sally Burton";

    public string Image { get; set; } = "caesarsalad.png";

    private bool? _isFavorite = false;
    public bool? IsFavorite
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

    public List<InstructionViewModel> Instructions { get; }

    public RecipeRatingsDetailViewModel RatingDetail { get; }
        = new();

    public ObservableCollection<RecipeIngredientViewModel> ShoppingList { get; } = new();

    public IngredientsListViewModel IngredientsList
    { get; } = new();

    public IRelayCommand AddAsFavoriteCommand { get; }
    public IRelayCommand RemoveAsFavoriteCommand { get; }
    public IRelayCommand AddToShoppingCartCommand { get; }
    public IRelayCommand RemoveFromShoppingCartCommand { get; }
    public IRelayCommand UserIsBrowsingCommand { get; }

    public RecipeDetailViewModel()
    {
        AddAsFavoriteCommand =
               new RelayCommand(AddAsFavorite, CanAddAsFavorite);
        RemoveAsFavoriteCommand = new RelayCommand(RemoveAsFavorite, CanRemoveAsFavorite);
        UserIsBrowsingCommand = new RelayCommand(UserIsBrowsing);
        AddToShoppingCartCommand = new RelayCommand<RecipeIngredientViewModel>(AddToShoppingCart);
        RemoveFromShoppingCartCommand = new RelayCommand<RecipeIngredientViewModel>(RemoveFromShoppingCart);


        Instructions = new List<InstructionViewModel>()
        {
            new ( 0, "Preheat your oven to 350°F (175°C). Place the baguette slices on a baking sheet and drizzle them with olive oil. Bake for about 10 minutes or until they are crispy and golden brown. Set aside to cool."),
            new (1, "In a small skillet, heat 2 tablespoons of olive oil over medium heat. Add the minced garlic and cook for about 1-2 minutes until fragrant. Remove from heat and let it cool."),
            new (2, "In a small bowl, whisk together the lemon juice, grated Parmesan cheese, minced anchovies (if using), and the cooled garlic-oil mixture. Set aside."),
            new (3, "Fill a medium-sized saucepan with water and bring it to a boil. Gently place the eggs into the boiling water and cook for 4-5 minutes for soft-boiled eggs or 9-10 minutes for hard-boiled eggs. Once cooked, remove the eggs from the boiling water and place them in a bowl of ice water to cool. Once cool, peel the eggs and set them aside."),
            new (4, "In a large salad bowl, add the torn romaine lettuce leaves. Pour the dressing over the lettuce and toss to coat evenly. Season with salt and freshly ground black pepper to taste."),
            new (5, "Break the baguette slices into smaller pieces and add them to the salad. Toss gently to combine."),
            new (6, "Cut the peeled eggs into halves or quarters and place them on top of the salad."),
            new (7, "Finally, sprinkle some additional grated Parmesan cheese on top as a garnish.")
        };
    }

    private void AddAsFavorite() => IsFavorite = true;
    private bool CanAddAsFavorite() => IsFavorite.HasValue && !IsFavorite.Value;

    private void RemoveAsFavorite() => IsFavorite = false;
    private bool CanRemoveAsFavorite() => IsFavorite.HasValue && IsFavorite.Value;

    private void UserIsBrowsing()
    {
        //Do Logging
    }

    private void AddToShoppingCart(RecipeIngredientViewModel viewModel)
    {
        if (ShoppingList.Contains(viewModel))
            return;
        ShoppingList.Add(viewModel);
    }

    private void RemoveFromShoppingCart(RecipeIngredientViewModel viewModel)
    {
        if (ShoppingList.Contains(viewModel))
            ShoppingList.Remove(viewModel);
    }
}
