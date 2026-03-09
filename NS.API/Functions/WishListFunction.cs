using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using NS.API.Core.Auth;

namespace NS.API.Functions;

public class WishListFunction(CosmosRepository repo)
{
    [Function("WishListGet")]
    public async Task<HttpResponseData?> WishListGet(
        [HttpTrigger(AuthorizationLevel.Anonymous, Method.Get, Route = "wishlist/get")] HttpRequestData req, CancellationToken cancellationToken)
    {
        var userId = await req.GetUserIdAsync(cancellationToken);
        if (userId.Empty()) throw new InvalidOperationException("GetUserId null");

        var doc = await repo.Get<WishList>(DocumentType.WishList, userId, cancellationToken);

        return await req.CreateResponse(doc, TtlCache.OneDay, cancellationToken);
    }

    [Function("WishListAdd")]
    public async Task<WishList?> WishListAdd(
        [HttpTrigger(AuthorizationLevel.Anonymous, Method.Post, Route = "wishlist/add")] HttpRequestData req, CancellationToken cancellationToken)
    {
        var userId = await req.GetUserIdAsync(cancellationToken);
        if (string.IsNullOrEmpty(userId)) throw new InvalidOperationException("GetUserId null");
        var entry = await req.GetPublicBody<WishListEntry>(cancellationToken: cancellationToken);

        var obj = await repo.Get<WishList>(DocumentType.WishList, userId, cancellationToken);

        if (obj == null)
        {
            obj = new WishList();

            obj.Initialize(userId);
        }

        obj.Items.Add(entry);

        return await repo.UpsertItemAsync(obj, cancellationToken);
    }

    [Function("WishListUpdate")]
    public async Task<WishList?> WishListUpdate(
    [HttpTrigger(AuthorizationLevel.Anonymous, Method.Put, Route = "wishlist/update")] HttpRequestData req, CancellationToken cancellationToken)
    {
        var userId = await req.GetUserIdAsync(cancellationToken);
        if (string.IsNullOrEmpty(userId)) throw new InvalidOperationException("GetUserId null");
        var entry = await req.GetPublicBody<WishListEntry>(cancellationToken: cancellationToken);

        var obj = await repo.Get<WishList>(DocumentType.WishList, userId, cancellationToken);

        var dbEntry = obj!.Items.Single(x => x.Id == entry.Id);

        dbEntry.RegionCode = entry.RegionCode;
        dbEntry.CityCode = entry.CityCode;
        dbEntry.RegionName = entry.RegionName;
        dbEntry.CityName = entry.CityName;
        dbEntry.Phase = entry.Phase;
        dbEntry.CheckList = entry.CheckList;
        dbEntry.ExperienceTags = entry.ExperienceTags;
        dbEntry.IntentionTags = entry.IntentionTags;
        dbEntry.ConditionsTags = entry.ConditionsTags;
        dbEntry.AlertsTags = entry.AlertsTags;

        return await repo.UpsertItemAsync(obj, cancellationToken);
    }

    [Function("WishListRemove")]
    public async Task<WishList?> WishListRemove(
        [HttpTrigger(AuthorizationLevel.Anonymous, Method.Post, Route = "wishlist/remove/{id}")] HttpRequestData req, string id, CancellationToken cancellationToken)
    {
        var userId = await req.GetUserIdAsync(cancellationToken);
        if (string.IsNullOrEmpty(userId)) throw new InvalidOperationException("GetUserId null");

        var obj = await repo.Get<WishList>(DocumentType.WishList, userId, cancellationToken);

        if (obj == null)
        {
            obj = new WishList();

            obj.Initialize(userId);
        }

        var item = obj.Items.FirstOrDefault(x => x.Id == id);
        if (item != null)
        {
            obj.Items.Remove(item);
            return await repo.UpsertItemAsync(obj, cancellationToken);
        }

        return obj;
    }
}