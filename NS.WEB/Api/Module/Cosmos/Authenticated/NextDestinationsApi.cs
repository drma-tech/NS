using NS.WEB.Api.Core;

namespace NS.WEB.Api.Module.Cosmos.Authenticated;

public class NextDestinationsApi(IHttpClientFactory factory) : ApiCosmos<NextDestinations>(factory, ApiType.Authenticated, "next-destinations", [], ApiContext.Default.NextDestinations)
{
    public async Task<NextDestinations?> Get(RenderControlState<NextDestinations?>[] states, CancellationToken cancellationToken)
    {
        return await GetAsync("next-destinations/get", setNewVersion: false, states, cancellationToken);
    }

    public async Task<NextDestinations?> Add(NextDestinations? obj, NextDestinationsEntry entry, AccountProduct? product, CancellationToken cancellationToken)
    {
        SubscriptionHelper.ValidateNextDestinations(product, (obj?.Items.Count ?? 0) + 1);

        return await PostAsync("next-destinations/add", entry, ApiContext.Default.NextDestinationsEntry, states: [], cancellationToken);
    }

    public async Task<NextDestinations?> Update(NextDestinationsEntry entry, CancellationToken cancellationToken)
    {
        return await PutAsync("next-destinations/update", entry, ApiContext.Default.NextDestinationsEntry, states: [], cancellationToken);
    }

    public async Task<NextDestinations?> Remove(string? regionCode, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(regionCode);

        return await PostAsync($"next-destinations/remove/{regionCode}", null, states: [], cancellationToken);
    }
}