using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using NS.API.Core.Auth;
using NS.Shared.Core.Types;

namespace NS.API.Functions;

public class WishListFunction(CosmosMainRepository repo)
{
    [Function("WishListGet")]
    public async Task<HttpResponseData?> WishListGet(
        [HttpTrigger(AuthorizationLevel.Anonymous, Method.Get, Route = "wishlist/get")] HttpRequestData req, CancellationToken cancellationToken)
    {
        var userId = await req.GetUserIdAsync(cancellationToken);

        var doc = await repo.ReadItemAsync<WishList>(new MainIdentity(MainType.WishList, userId), cancellationToken);

        return await req.CreateResponse(doc, TtlCache.OneDay, cancellationToken);
    }

    [Function("WishListAdd")]
    public async Task<WishList?> WishListAdd(
        [HttpTrigger(AuthorizationLevel.Anonymous, Method.Post, Route = "wishlist/add")] HttpRequestData req, CancellationToken cancellationToken)
    {
        var userId = await req.GetUserIdAsync(cancellationToken);
        var body = await req.GetBody<WishListEntry>(cancellationToken: cancellationToken);

        var obj = await repo.ReadItemAsync<WishList>(new MainIdentity(MainType.WishList, userId), cancellationToken);

        obj ??= new WishList(userId);

        obj.Items.Add(body);

        return await repo.UpsertItemAsync(obj);
    }

    [Function("WishListUpdate")]
    public async Task<WishList?> WishListUpdate(
    [HttpTrigger(AuthorizationLevel.Anonymous, Method.Put, Route = "wishlist/update")] HttpRequestData req, CancellationToken cancellationToken)
    {
        var userId = await req.GetUserIdAsync(cancellationToken);
        var body = await req.GetBody<WishListEntry>(cancellationToken: cancellationToken);

        var obj = await repo.ReadItemAsync<WishList>(new MainIdentity(MainType.WishList, userId), cancellationToken);

        var dbEntry = obj!.Items.Single(x => x.Id == body.Id);

        dbEntry.RegionCode = body.RegionCode;
        dbEntry.CityCode = body.CityCode;
        dbEntry.RegionName = body.RegionName;
        dbEntry.CityName = body.CityName;
        dbEntry.Phase = body.Phase;
        dbEntry.CheckList = body.CheckList;
        dbEntry.ExperienceTags = body.ExperienceTags;
        dbEntry.IntentionTags = body.IntentionTags;
        dbEntry.ConditionsTags = body.ConditionsTags;
        dbEntry.AlertsTags = body.AlertsTags;

        return await repo.UpsertItemAsync(obj);
    }

    [Function("WishListRemove")]
    public async Task<WishList?> WishListRemove(
        [HttpTrigger(AuthorizationLevel.Anonymous, Method.Post, Route = "wishlist/remove/{id}")] HttpRequestData req, string id, CancellationToken cancellationToken)
    {
        var userId = await req.GetUserIdAsync(cancellationToken);

        var obj = await repo.ReadItemAsync<WishList>(new MainIdentity(MainType.WishList, userId), cancellationToken);

        obj ??= new WishList(userId);

        var item = obj.Items.FirstOrDefault(x => x.Id == id);
        if (item != null)
        {
            obj.Items.Remove(item);
            return await repo.UpsertItemAsync(obj);
        }

        return obj;
    }
}