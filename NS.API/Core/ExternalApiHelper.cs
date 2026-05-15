using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace NS.API.Core;

public static class ExternalApiHelper
{
    public static async Task<T?> GetApiData<T>(this HttpClient http, string requestUri, CancellationToken cancellationToken)
        where T : class
    {
        return await http.GetFromJsonAsync<T>(requestUri, cancellationToken);
    }

    public static async Task<T?> PostApiData<T>(this HttpClient http, string url, string doc, string key, CancellationToken cancellationToken)
        where T : class
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, url);

        request.Headers.TryAddWithoutValidation("content-type", "application/vnd.api+json");
        request.Headers.TryAddWithoutValidation("x-api-key", key);

        request.Content = new StringContent(doc, Encoding.UTF8);
        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/vnd.api+json");

        var response = await http.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == HttpStatusCode.TooManyRequests) return null;

            throw new UnhandledException(response.ReasonPhrase);
        }

        return await response.Content.ReadFromJsonAsync<T>(cancellationToken);
    }

    public static async Task<T?> GetNewsByNewsAPI<T>(this HttpClient http, string? topic, CancellationToken cancellationToken) where T : class
    {
        //hard limit: 100 / Day

        if (topic.Empty()) return null;

        var url = $"https://news-api14.p.rapidapi.com/v2/trendings?date={DateTime.Now:yyyy-MM-dd}&topic={topic}&language=en";
        using var request = new HttpRequestMessage(HttpMethod.Get, url);

        request.Headers.TryAddWithoutValidation("X-RapidAPI-Key", ApiStartup.Configurations.RapidAPI?.Key);
        request.Headers.TryAddWithoutValidation("X-RapidAPI-Host", "news-api14.p.rapidapi.com");

        var response = await http.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == HttpStatusCode.TooManyRequests) return null;

            throw new UnhandledException(response.ReasonPhrase);
        }

        return await response.Content.ReadFromJsonAsync<T>(cancellationToken);
    }

    public static async Task<T?> GetNewsByGoogleNews<T>(this HttpClient http, string? location, CancellationToken cancellationToken) where T : class
    {
        //hard limit: 100 / Day

        if (location.Empty()) return null;

        var url = $"https://google-news22.p.rapidapi.com/v2/search?q={location}&country=us&language=en&from={DateTime.Now.AddDays(-14):yyyy-MM-dd}&to={DateTime.Now:yyyy-MM-dd}'";
        using var request = new HttpRequestMessage(HttpMethod.Get, url);

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
        //+ $0.001: 500 / Day (only payed have 14 Day Forecast)

        var url = $"https://weatherapi-com.p.rapidapi.com/{endpoint}.json?q={city}&dt={halfMonth}";
        using var request = new HttpRequestMessage(HttpMethod.Get, url);

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