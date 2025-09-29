using System.Net.Http.Json;

namespace NS.API.Core;

public static class ApiCore
{
    public static async Task<T?> GetApiData<T>(this HttpClient http, string requestUri, CancellationToken cancellationToken)
        where T : class
    {
        return await http.GetFromJsonAsync<T>(requestUri, cancellationToken);
    }
}