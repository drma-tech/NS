namespace NS.WEB.Modules.Profile.Core;

public class NextDestinationsApi(IHttpClientFactory factory) : ApiCosmos<NextDestinations>(factory, ApiType.Authenticated, "next-destinations", ApiContext.Default.NextDestinations)
{
    public async Task<NextDestinations?> Get(ComponentActions<NextDestinations?>? actions, CancellationToken cancellationToken)
    {
        return await GetAsync(Endpoint.Get, false, actions, cancellationToken);
    }

    public async Task<NextDestinations?> Add(NextDestinations? obj, NextDestinationsEntry entry, AccountProduct? product, CancellationToken cancellationToken)
    {
        SubscriptionHelper.ValidateNextDestinations(product, (obj?.Items.Count ?? 0) + 1);

        return await PostAsync(Endpoint.Add, entry, ApiContext.Default.NextDestinationsEntry, cancellationToken);
    }

    public async Task<NextDestinations?> Update(NextDestinationsEntry entry, CancellationToken cancellationToken)
    {
        return await PutAsync(Endpoint.Update, entry, ApiContext.Default.NextDestinationsEntry, cancellationToken);
    }

    public async Task<NextDestinations?> Remove(string? regionCode, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(regionCode);

        return await PostAsync(Endpoint.Remove(regionCode), null, cancellationToken);
    }

    private struct Endpoint
    {
        public const string Get = "next-destinations/get";
        public const string Add = "next-destinations/add";
        public const string Update = "next-destinations/update";

        public static string Remove(string? regionCode)
        {
            return $"next-destinations/remove/{regionCode}";
        }
    }
}