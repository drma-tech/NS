using NS.Shared.Models.Auth;

namespace NS.WEB.Modules.Profile.Core;

public class WishListApi(IHttpClientFactory factory) : ApiCosmos<WishList>(factory, ApiType.Authenticated, "wishlist")
{
    public async Task<WishList?> Get(bool isUserAuthenticated)
    {
        if (isUserAuthenticated) return await GetAsync(Endpoint.Get);

        return new WishList();
    }

    public async Task<WishList?> Add(WishList? obj, string? regionCode, AuthSubscription? subs)
    {
        ArgumentNullException.ThrowIfNull(regionCode);
        SubscriptionHelper.ValidateWishList(subs?.ActiveProduct, (obj?.Regions.Count ?? 0) + 1);

        return await PostAsync(Endpoint.Add(regionCode), null);
    }

    public async Task<WishList?> Remove(string? regionCode)
    {
        ArgumentNullException.ThrowIfNull(regionCode);

        return await PostAsync(Endpoint.Remove(regionCode), null);
    }

    private struct Endpoint
    {
        public const string Get = "wishlist/get";

        public static string Add(string? regionCode)
        {
            return $"wishlist/add/{regionCode}";
        }

        public static string Remove(string? regionCode)
        {
            return $"wishlist/remove/{regionCode}";
        }
    }
}