using NS.WEB.Api.Core;

namespace NS.WEB.Api.Module.Cosmos.Authenticated;

public class TravelHistoryApi(IHttpClientFactory factory) : ApiCosmos<TravelHistory>(factory, ApiType.Authenticated, "travel-history", [], ApiContext.Default.TravelHistory)
{
    public async Task<TravelHistory?> Get(RenderControlState<TravelHistory?>[] states, CancellationToken cancellationToken)
    {
        return await GetAsync("travel-history/get", setNewVersion: false, states, cancellationToken);
    }

    public async Task<TravelHistory?> Add(TravelHistory? obj, TravelHistoryEntry entry, AccountProduct? product, CancellationToken cancellationToken)
    {
        SubscriptionHelper.ValidateTravelHistory(product, (obj?.Items.Count ?? 0) + 1);

        return await PostAsync("travel-history/add", entry, ApiContext.Default.TravelHistoryEntry, states: [], cancellationToken);
    }

    public async Task<TravelHistory?> Update(TravelHistoryEntry entry, CancellationToken cancellationToken)
    {
        return await PostAsync("travel-history/update", entry, ApiContext.Default.TravelHistoryEntry, states: [], cancellationToken);
    }

    public async Task<TravelHistory?> Remove(string? id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        return await PostAsync($"travel-history/remove/{id}", null, states: [], cancellationToken);
    }
}