//using Recipes.Client.Core;
//using Recipes.Client.Core.Features.Ratings;
//using Recipes.Shared.Dto;
//using Refit;

//namespace Recipes.Mobile.Repositories;

//public abstract class ApiGateway
//{
//    protected async Task<Result<TResult>> Get<TResult, TDtoResult>(Task<ApiResponse<TDtoResult>> call, Func<TDtoResult, TResult> mapper)
//    {
//        try
//        {
//            var response = await call;

//            if (response.IsSuccessStatusCode)
//            {
//                return Result.Success(mapper(response.Content));
//            }
//            else
//            {
//                return Result.Fail<TResult>("", "");
//            }
//        }
//        catch (Exception ex)
//        {
//            return Result.Fail<TResult>("", "");
//        }
//    }
//}

//internal class RatingsApiGateway : ApiGateway, IRatingsRepository
//{
//    readonly IRatingsApi _api;
//    public Task<Result<Rating[]>> GetRatings(string recipeId    )
//    => Get(_api.GetRatings(recipeId), (dto) =>
//            dto.Select(r => new Rating(
//                        r.Id,
//                        r.RecipeId,
//                        r.Rating,
//                        r.UserName,
//                        r.Review)).ToArray()
//        );

//    public Task<Result<RatingsSummary>> GetRatingsSummary(string recipeId)
//    => Get(
//        _api.GetRatingsSummary(recipeId), 
//        (dto) => new RatingsSummary(dto.TotalReviews, dto.MaxRating, dto.AverageRating)
//        );

//    public RatingsApiGateway(IRatingsApi api)
//    {
//        _api = api;
//    }
//}

//public interface IRatingsApi
//{
//    [Get("/recipe/{recipeId}/ratings")]
//    Task<ApiResponse<RatingDto[]>> GetRatings(string recipeId);
//    [Get("/recipe/{recipeId}/ratingssummary")]
//    Task<ApiResponse<RatingsSummaryDto>> GetRatingsSummary(string recipeId);
//}
