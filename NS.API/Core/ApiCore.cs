using System.Net;
using System.Net.Http.Json;

namespace NS.API.Core;

public static class ApiCore
{
    public static async Task<T?> GetApiData<T>(this HttpClient http, string requestUri, CancellationToken cancellationToken)
        where T : class
    {
        return await http.GetFromJsonAsync<T>(requestUri, cancellationToken);
    }

    public static async Task<T?> GetNewsByGoogleNews<T>(this HttpClient http, string? location, CancellationToken cancellationToken) where T : class
    {
        if (location.Empty()) return null;

        using var request = new HttpRequestMessage(HttpMethod.Get, $"https://google-news22.p.rapidapi.com/v1/search?q={location}&country=us&language=en&from={DateTime.Now.AddDays(-14):yyyy-MM-dd}&to={DateTime.Now:yyyy-MM-dd}'");

        request.Headers.TryAddWithoutValidation("X-RapidAPI-Key", ApiStartup.Configurations.RapidAPI?.Key);
        request.Headers.TryAddWithoutValidation("X-RapidAPI-Host", "google-news22.p.rapidapi.com");

        var response = await http.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == HttpStatusCode.TooManyRequests) return null;

            throw new UnhandledException(response.ReasonPhrase);
        }

        return await response.Content.ReadFromJsonAsync<T>(cancellationToken);
    }

    public static async Task<T?> GetWeatherByWeatherApi<T>(this HttpClient http, string endpoint, string city, string halfMonth, CancellationToken cancellationToken) where T : class
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"https://weatherapi-com.p.rapidapi.com/{endpoint}.json?q={city}&dt={halfMonth}");

        request.Headers.TryAddWithoutValidation("X-RapidAPI-Key", ApiStartup.Configurations.RapidAPI?.Key);
        request.Headers.TryAddWithoutValidation("X-RapidAPI-Host", "weatherapi-com.p.rapidapi.com");

        var response = await http.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == HttpStatusCode.TooManyRequests) return null;

            throw new UnhandledException(response.ReasonPhrase);
        }

        return await response.Content.ReadFromJsonAsync<T>(cancellationToken);
    }
}