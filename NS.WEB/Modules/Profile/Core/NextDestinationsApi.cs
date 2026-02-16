namespace NS.WEB.Modules.Profile.Core;

public class NextDestinationsApi(IHttpClientFactory factory) : ApiCosmos<NextDestinations>(factory, ApiType.Authenticated, "next-destinations")
{
    public async Task<NextDestinations?> Get(bool isUserAuthenticated)
    {
        if (isUserAuthenticated) return await GetAsync(Endpoint.Get);

        return new NextDestinations();
    }

    public async Task<NextDestinations?> Add(NextDestinations? obj, NextDestinationsEntry entry, AccountProduct? product)
    {
        SubscriptionHelper.ValidateNextDestinations(product, (obj?.Items.Count ?? 0) + 1);

        return await PostAsync(Endpoint.Add, entry);
    }

    public async Task<NextDestinations?> Update(NextDestinationsEntry entry)
    {
        return await PutAsync(Endpoint.Update, entry);
    }

    public async Task<NextDestinations?> Remove(string? regionCode)
    {
        ArgumentNullException.ThrowIfNull(regionCode);

        return await PostAsync(Endpoint.Remove(regionCode), null);
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