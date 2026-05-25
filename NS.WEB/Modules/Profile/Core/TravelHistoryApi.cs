namespace NS.WEB.Modules.Profile.Core;

public class TravelHistoryApi(IHttpClientFactory factory) : ApiCosmos<TravelHistory>(factory, ApiType.Authenticated, "travel-history", ApiContext.Default.TravelHistory)
{
    public async Task<TravelHistory?> Get(bool isUserAuthenticated, CancellationToken cancellationToken)
    {
        if (isUserAuthenticated) return await GetAsync(Endpoint.Get, false, cancellationToken);

        return new TravelHistory();
    }

    public async Task<TravelHistory?> Add(TravelHistory? obj, TravelHistoryEntry entry, AccountProduct? product, CancellationToken cancellationToken)
    {
        SubscriptionHelper.ValidateTravelHistory(product, (obj?.Items.Count ?? 0) + 1);

        return await PostAsync(Endpoint.Add, entry, ApiContext.Default.TravelHistoryEntry, cancellationToken);
    }

    public async Task<TravelHistory?> Update(TravelHistoryEntry entry, CancellationToken cancellationToken)
    {
        return await PutAsync(Endpoint.Update, entry, ApiContext.Default.TravelHistoryEntry, cancellationToken);
    }

    public async Task<TravelHistory?> Remove(string? id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        return await PostAsync(Endpoint.Remove(id), null, cancellationToken);
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