namespace HabitTracker.Api;

/// <summary>Единый формат ошибок: { "error": "текст" }.</summary>
public static class Errors
{
    public static IResult BadRequest(string message) => Results.Json(new ApiError(message), statusCode: 400);
    public static IResult Unauthorized(string message) => Results.Json(new ApiError(message), statusCode: 401);
    public static IResult NotFound(string message) => Results.Json(new ApiError(message), statusCode: 404);
    public static IResult Conflict(string message) => Results.Json(new ApiError(message), statusCode: 409);

    public static Task Write(HttpResponse response, int statusCode, string message)
    {
        response.StatusCode = statusCode;
        return response.WriteAsJsonAsync(new ApiError(message));
    }

    /// <summary>Первая найденная ошибка валидации или null.</summary>
    public static string? First(params string?[] errors) => errors.FirstOrDefault(e => e is not null);
}
