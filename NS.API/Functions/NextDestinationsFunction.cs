using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using NS.API.Core.Auth;
using NS.Shared.Core.Types;

namespace NS.API.Functions;

public class NextDestinationsFunction(CosmosMainRepository repo)
{
    [Function("NextDestinationsGet")]
    public async Task<HttpResponseData?> NextDestinationsGet(
        [HttpTrigger(AuthorizationLevel.Anonymous, Method.Get, Route = "next-destinations/get")] HttpRequestData req, CancellationToken cancellationToken)
    {
        var userId = await req.GetUserIdAsync(cancellationToken);

        var doc = await repo.ReadItemAsync<NextDestinations>(new MainIdentity(MainType.NextDestinations, userId), cancellationToken);

        return await req.CreateResponse(doc, TtlCache.OneDay, cancellationToken);
    }

    [Function("NextDestinationsAdd")]
    public async Task<NextDestinations?> NextDestinationsAdd(
        [HttpTrigger(AuthorizationLevel.Anonymous, Method.Post, Route = "next-destinations/add")] HttpRequestData req, CancellationToken cancellationToken)
    {
        var userId = await req.GetUserIdAsync(cancellationToken);
        var body = await req.GetBody<NextDestinationsEntry>(cancellationToken: cancellationToken);

        var obj = await repo.ReadItemAsync<NextDestinations>(new MainIdentity(MainType.NextDestinations, userId), cancellationToken);

        obj ??= new NextDestinations(userId);

        obj.Items.Add(body);

        return await repo.UpsertItemAsync(obj);
    }

    [Function("NextDestinationsUpdate")]
    public async Task<NextDestinations?> NextDestinationsUpdate(
      [HttpTrigger(AuthorizationLevel.Anonymous, Method.Put, Route = "next-destinations/update")] HttpRequestData req, CancellationToken cancellationToken)
    {
        var userId = await req.GetUserIdAsync(cancellationToken);
        var body = await req.GetBody<NextDestinationsEntry>(cancellationToken: cancellationToken);

        var obj = await repo.ReadItemAsync<NextDestinations>(new MainIdentity(MainType.NextDestinations, userId), cancellationToken);

        var dbEntry = obj!.Items.Single(x => x.Id == body.Id);

        dbEntry.StartDate = body.StartDate;
        dbEntry.EndDate = body.EndDate;
        dbEntry.RegionCode = body.RegionCode;
        dbEntry.CityCode = body.CityCode;
        dbEntry.RegionName = body.RegionName;
        dbEntry.CityName = body.CityName;
        dbEntry.Notes = body.Notes;

        return await repo.UpsertItemAsync(obj);
    }

    [Function("NextDestinationsRemove")]
    public async Task<NextDestinations?> NextDestinationsRemove(
        [HttpTrigger(AuthorizationLevel.Anonymous, Method.Post, Route = "next-destinations/remove/{id}")] HttpRequestData req, string id, CancellationToken cancellationToken)
    {
        var userId = await req.GetUserIdAsync(cancellationToken);

        var obj = await repo.ReadItemAsync<NextDestinations>(new MainIdentity(MainType.NextDestinations, userId), cancellationToken);

        obj ??= new NextDestinations(userId);

        var item = obj.Items.FirstOrDefault(x => x.Id == id);
        if (item != null)
        {
            obj.Items.Remove(item);
            return await repo.UpsertItemAsync(obj);
        }

        return obj;
    }
}