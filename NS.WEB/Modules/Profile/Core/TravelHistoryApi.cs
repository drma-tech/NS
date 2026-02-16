namespace NS.WEB.Modules.Profile.Core;

public class TravelHistoryApi(IHttpClientFactory factory) : ApiCosmos<TravelHistory>(factory, ApiType.Authenticated, "travel-history")
{
    public async Task<TravelHistory?> Get(bool isUserAuthenticated)
    {
        if (isUserAuthenticated) return await GetAsync(Endpoint.Get);

        return new TravelHistory();
    }

    public async Task<TravelHistory?> Add(TravelHistory? obj, TravelHistoryEntry entry, AccountProduct? product)
    {
        SubscriptionHelper.ValidateTravelHistory(product, (obj?.Items.Count ?? 0) + 1);

        return await PostAsync(Endpoint.Add, entry);
    }

    public async Task<TravelHistory?> Update(TravelHistoryEntry entry)
    {
        return await PutAsync(Endpoint.Update, entry);
    }

    public async Task<TravelHistory?> Remove(string? id)
    {
        ArgumentNullException.ThrowIfNull(id);

        return await PostAsync(Endpoint.Remove(id), null);
    }

    private struct Endpoint
    {
        public const string Get = "travel-history/get";
        public const string Add = "travel-history/add";
        public const string Update = "travel-history/update";

        public static string Remove(string? id)
        {
            return $"travel-history/remove/{id}";
        }
    }
}