﻿namespace Recipes.Client.Core.Ratings;

public interface IRatingsService
{
    Task<RatingsSummaryDto> LoadRatingsSummary(string recipeId);
    Task<RatingDto[]> LoadRatings(string recipeId);
}
