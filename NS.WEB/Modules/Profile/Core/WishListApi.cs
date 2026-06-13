namespace NS.WEB.Modules.Profile.Core;

public class WishListApi(IHttpClientFactory factory) : ApiCosmos<WishList>(factory, ApiType.Authenticated, "wishlist", ApiContext.Default.WishList)
{
    public async Task<WishList?> Get(ComponentActions<WishList?>? actions, CancellationToken cancellationToken)
    {
        return await GetAsync(Endpoint.Get, false, actions, cancellationToken);
    }

    public async Task<WishList?> Add(WishList? obj, WishListEntry entry, AccountProduct? product, CancellationToken cancellationToken)
    {
        SubscriptionHelper.ValidateWishList(product, (obj?.Items.Count ?? 0) + 1);

        return await PostAsync(Endpoint.Add, entry, ApiContext.Default.WishListEntry, cancellationToken);
    }

    public async Task<WishList?> Update(WishListEntry entry, CancellationToken cancellationToken)
    {
        return await PutAsync(Endpoint.Update, entry, ApiContext.Default.WishListEntry, cancellationToken);
    }

    public async Task<WishList?> Remove(string? regionCode, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(regionCode);

        return await PostAsync(Endpoint.Remove(regionCode), null, cancellationToken);
    }

    private struct Endpoint
    {
        public const string Get = "wishlist/get";
        public const string Add = "wishlist/add";
        public const string Update = "wishlist/update";

        public static string Remove(string? regionCode)
        {
            return $"wishlist/remove/{regionCode}";
        }
    }
}