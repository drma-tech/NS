using NS.Shared.Models.Auth;
using NS.WEB.Shared;

namespace NS.WEB.Modules.Profile.Core;

public class WishListApi(IHttpClientFactory factory) : ApiCosmos<WishList>(factory, ApiType.Authenticated, "wishlist")
{
    public async Task<WishList?> Get(bool isUserAuthenticated, RenderControlCore<WishList?>? core)
    {
        if (isUserAuthenticated) return await GetAsync(Endpoint.Get, core);

        return new WishList();
    }

    public async Task<WishList?> Add(WishList? obj, NS.Shared.Enums.Country? country, AuthSubscription? subs)
    {
        ArgumentNullException.ThrowIfNull(country);
        SubscriptionHelper.ValidateWishList(subs?.ActiveProduct, (obj?.Countries.Count ?? 0) + 1);

        return await PostAsync(Endpoint.Add((int)country), null, null);
    }

    public async Task<WishList?> Remove(NS.Shared.Enums.Country? country)
    {
        ArgumentNullException.ThrowIfNull(country);

        return await PostAsync(Endpoint.Remove((int)country), null, null);
    }

    private struct Endpoint
    {
        public const string Get = "wishlist/get";

        public static string Add(int? country)
        {
            return $"wishlist/add/{country}";
        }

        public static string Remove(int? country)
        {
            return $"wishlist/remove/{country}";
        }
    }
}