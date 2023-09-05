using Microsoft.AspNetCore.Mvc;
using Recipes.Shared.Dto;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseHttpsRedirection();


app.MapGet("/recipes", async (int pageSize, int pageIndex) =>
{
    return await LoadRecipes(pageSize, pageIndex);
})
.WithName("GetRecipes")
.WithOpenApi();

app.MapGet("/recipe/{id}", async (string id) =>
{
    return await LoadRecipe(id);
})
.WithName("GetRecipe")
.WithOpenApi();

app.MapGet("/recipe/{id}/ratings", async (string id) =>
{
    return await LoadRatings(id);
})
.WithName("GetRecipeRatings")
.WithOpenApi();

app.MapGet("/recipe/{id}/ratingssummary", async (string id) =>
{
    return await LoadRatingsSummary(id);
})
.WithName("GetRecipeRatingsSummary")
.WithOpenApi();

app.MapGet("/users/{userId}/favorites", (string userId) =>
{
    return FavoritesDataStore.GetFavorites(userId);
})
.WithName("GetUserFavorites")
.WithOpenApi();

app.MapPost("/users/{userId}/favorites", (string userId, [FromBody] FavoriteDto favorite) =>
{
    FavoritesDataStore.StoreFavorite(userId, favorite);
})
.WithName("AddFavorite")
.WithOpenApi();

app.MapDelete("/users/{userId}/favorites", (string userId, [FromBody] FavoriteDto favorite) =>
{
    FavoritesDataStore.DeleteFavorite(userId, favorite);
})
.WithName("DeleteFavorite")
.WithOpenApi();



app.Run();

async Task<RecipeDetailDto?> LoadRecipe(string id)
{
    var recipeDetails = await ReadRecipeDetailsFromStream();
    return recipeDetails.SingleOrDefault(r => r.Id == id);
}

async Task<RecipeOverviewItemsDto> LoadRecipes(int pageSize = 7, int pageIndex = 0)
{
    var recipeDetails = (await ReadRecipeDetailsFromStream()).ToList();

    var result = new RecipeOverviewItemsDto(recipeDetails.Count, pageSize, pageIndex,
        recipeDetails
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .Select(r => new RecipeOverviewItemDto(r.Id, r.Name, r.Image))
            .ToArray());

    return result;
}

async Task<RecipeDetailDto[]> ReadRecipeDetailsFromStream()
{
    string json = string.Empty;
    using (StreamReader reader = new StreamReader("recipedetails.json", Encoding.UTF8))
    {
        json = reader.ReadToEnd();
    }
    return JsonSerializer.Deserialize<RecipeDetailDto[]>(json) ?? new RecipeDetailDto[0];
}


async Task<RatingsSummaryDto> LoadRatingsSummary(string recipeId)
{
    var ratings = await ReadRatingsFromStream();

    var recipeRatings = await LoadRatings(recipeId);
    return new RatingsSummaryDto(recipeRatings.Count(), 4, recipeRatings.Sum(r => r.Rating) / recipeRatings.Count());
}

async Task<RatingDto[]> LoadRatings(string recipeId)
{
        var ratings = await ReadRatingsFromStream();
    return ratings.Where(r => r.RecipeId == recipeId).ToArray();
}

async Task<RatingDto[]> ReadRatingsFromStream()
{
    string json = string.Empty;
    using (StreamReader reader = new StreamReader("ratings.json", Encoding.UTF8))
    {
        json = reader.ReadToEnd();
    }
    var serializeOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    return JsonSerializer.Deserialize<RatingDto[]>(json, serializeOptions) ?? new RatingDto[0];
}




static class FavoritesDataStore
{
    static List<string> favorites = new List<string>();

    public static string[] GetFavorites(string userId)
    {
        return favorites.ToArray();
    }

    public static void StoreFavorite(string userId, FavoriteDto favorite)
    {
        if (favorites.Contains(favorite.RecipeId))
            return;
        favorites.Add(favorite.RecipeId);
    }

    public static void DeleteFavorite(string userId, FavoriteDto favorite)
    {
        if (favorites.Contains(favorite.RecipeId))
            favorites.Remove(favorite.RecipeId);
    }
}