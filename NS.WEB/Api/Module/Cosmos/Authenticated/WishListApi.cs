using NS.WEB.Api.Core;

namespace NS.WEB.Api.Module.Cosmos.Authenticated;

public class WishListApi(IHttpClientFactory factory) : ApiCosmos<WishList>(factory, ApiType.Authenticated, "wishlist", [], ApiContext.Default.WishList)
{
    public async Task<WishList?> Get(RenderControlState<WishList?>[] states, CancellationToken cancellationToken)
    {
        return await GetAsync("wishlist/get", setNewVersion: false, states, cancellationToken);
    }

    public async Task<WishList?> Add(WishList? obj, WishListEntry entry, AccountProduct? product, CancellationToken cancellationToken)
    {
        SubscriptionHelper.ValidateWishList(product, (obj?.Items.Count ?? 0) + 1);

        return await PostAsync("wishlist/add", entry, ApiContext.Default.WishListEntry, states: [], cancellationToken);
    }

    public async Task<WishList?> Update(WishListEntry entry, CancellationToken cancellationToken)
    {
        return await PostAsync("wishlist/update", entry, ApiContext.Default.WishListEntry, states: [], cancellationToken);
    }

    public async Task<WishList?> Remove(string? regionCode, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(regionCode);

        return await PostAsync($"wishlist/remove/{regionCode}", null, states: [], cancellationToken);
    }
}