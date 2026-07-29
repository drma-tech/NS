using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using NS.API.Core.Auth;
using NS.Shared.Core.Types;

namespace NS.API.Functions;

public class TravelHistoryFunction(CosmosMainRepository repo)
{
    [Function("TravelHistoryGet")]
    public async Task<HttpResponseData?> TravelHistoryGet(
        [HttpTrigger(AuthorizationLevel.Anonymous, Method.Get, Route = "travel-history/get")] HttpRequestData req, CancellationToken cancellationToken)
    {
        var userId = await req.GetUserIdAsync(cancellationToken);

        var doc = await repo.ReadItemAsync<TravelHistory>(new MainIdentity(MainType.TravelHistory, userId), cancellationToken);

        return await req.CreateResponse(doc, TtlCache.OneDay, cancellationToken);
    }

    [Function("TravelHistoryAdd")]
    public async Task<TravelHistory?> TravelHistoryAdd(
        [HttpTrigger(AuthorizationLevel.Anonymous, Method.Post, Route = "travel-history/add")] HttpRequestData req, CancellationToken cancellationToken)
    {
        var userId = await req.GetUserIdAsync(cancellationToken);
        var body = await req.GetBody<TravelHistoryEntry>(cancellationToken: cancellationToken);

        var obj = await repo.ReadItemAsync<TravelHistory>(new MainIdentity(MainType.TravelHistory, userId), cancellationToken);

        obj ??= new TravelHistory(userId);

        obj.Items.Add(body);

        return await repo.UpsertItemAsync(obj);
    }

    [Function("TravelHistoryUpdate")]
    public async Task<TravelHistory?> TravelHistoryUpdate(
      [HttpTrigger(AuthorizationLevel.Anonymous, Method.Put, Route = "travel-history/update")] HttpRequestData req, CancellationToken cancellationToken)
    {
        var userId = await req.GetUserIdAsync(cancellationToken);
        var body = await req.GetBody<TravelHistoryEntry>(cancellationToken: cancellationToken);

        var obj = await repo.ReadItemAsync<TravelHistory>(new MainIdentity(MainType.TravelHistory, userId), cancellationToken);

        var dbEntry = obj!.Items.Single(x => x.Id == body.Id);

        dbEntry.StartDate = body.StartDate;
        dbEntry.EndDate = body.EndDate;
        dbEntry.RegionCode = body.RegionCode;
        dbEntry.CityCode = body.CityCode;
        dbEntry.RegionName = body.RegionName;
        dbEntry.CityName = body.CityName;
        dbEntry.RegionRating = body.RegionRating;
        dbEntry.CityRating = body.CityRating;
        dbEntry.Notes = body.Notes;

        return await repo.UpsertItemAsync(obj);
    }

    [Function("TravelHistoryRemove")]
    public async Task<TravelHistory?> TravelHistoryRemove(
        [HttpTrigger(AuthorizationLevel.Anonymous, Method.Post, Route = "travel-history/remove/{id}")] HttpRequestData req, string id, CancellationToken cancellationToken)
    {
        var userId = await req.GetUserIdAsync(cancellationToken);

        var obj = await repo.ReadItemAsync<TravelHistory>(new MainIdentity(MainType.TravelHistory, userId), cancellationToken);

        obj ??= new TravelHistory(userId);

        var item = obj.Items.FirstOrDefault(x => x.Id == id);
        if (item != null)
        {
            obj.Items.Remove(item);
            return await repo.UpsertItemAsync(obj);
        }

        return obj;
    }
}