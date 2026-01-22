using NS.Shared.Models.Auth;

namespace NS.WEB.Modules.Profile.Core;

public class WishListApi(IHttpClientFactory factory) : ApiCosmos<WishList>(factory, ApiType.Authenticated, "wishlist")
{
    public async Task<WishList?> Get(bool isUserAuthenticated)
    {
        if (isUserAuthenticated) return await GetAsync(Endpoint.Get);

        return new WishList();
    }

    public async Task<WishList?> Add(WishList? obj, WishListEntry entry, AuthSubscription? subs)
    {
        SubscriptionHelper.ValidateWishList(subs?.ActiveProduct, (obj?.Items.Count ?? 0) + 1);

        return await PostAsync(Endpoint.Add, entry);
    }

    public async Task<WishList?> Update(WishListEntry entry)
    {
        return await PutAsync(Endpoint.Update, entry);
    }

    public async Task<WishList?> Remove(string? regionCode)
    {
        ArgumentNullException.ThrowIfNull(regionCode);

        return await PostAsync(Endpoint.Remove(regionCode), null);
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