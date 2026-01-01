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
        try
        {
            var userId = await req.GetUserIdAsync(cancellationToken);
            if (userId.Empty()) throw new InvalidOperationException("GetUserId null");

            var doc = await repo.Get<WishList>(DocumentType.WishList, userId, cancellationToken);

            return await req.CreateResponse(doc, TtlCache.OneDay, cancellationToken);
        }
        catch (Exception ex)
        {
            req.LogError(ex);
            throw;
        }
    }

    [Function("WishListAdd")]
    public async Task<WishList?> WishListAdd(
        [HttpTrigger(AuthorizationLevel.Anonymous, Method.Post, Route = "wishlist/add/{region}")] HttpRequestData req, string regionCode, CancellationToken cancellationToken)
    {
        try
        {
            var userId = await req.GetUserIdAsync(cancellationToken);
            if (string.IsNullOrEmpty(userId)) throw new InvalidOperationException("GetUserId null");

            var obj = await repo.Get<WishList>(DocumentType.WishList, userId, cancellationToken);

            if (obj == null)
            {
                obj = new WishList();

                obj.Initialize(userId);
            }

            obj.Regions.Add(regionCode);

            return await repo.UpsertItemAsync(obj, cancellationToken);
        }
        catch (Exception ex)
        {
            req.LogError(ex);
            throw;
        }
    }

    [Function("WishListRemove")]
    public async Task<WishList?> WishListRemove(
        [HttpTrigger(AuthorizationLevel.Anonymous, Method.Post, Route = "wishlist/remove/{region}")] HttpRequestData req, string regionCode, CancellationToken cancellationToken)
    {
        try
        {
            var userId = await req.GetUserIdAsync(cancellationToken);
            if (string.IsNullOrEmpty(userId)) throw new InvalidOperationException("GetUserId null");

            var obj = await repo.Get<WishList>(DocumentType.WishList, userId, cancellationToken);

            if (obj == null)
            {
                obj = new WishList();

                obj.Initialize(userId);
            }

            obj.Regions.Remove(regionCode);

            return await repo.UpsertItemAsync(obj, cancellationToken);
        }
        catch (Exception ex)
        {
            req.LogError(ex);
            throw;
        }
    }
}