using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using NS.API.Core.Auth;

namespace NS.API.Functions;

public class NextDestinationsFunction(CosmosRepository repo)
{
    [Function("NextDestinationsGet")]
    public async Task<HttpResponseData?> NextDestinationsGet(
        [HttpTrigger(AuthorizationLevel.Anonymous, Method.Get, Route = "next-destinations/get")] HttpRequestData req, CancellationToken cancellationToken)
    {
        try
        {
            var userId = await req.GetUserIdAsync(cancellationToken);
            if (userId.Empty()) throw new InvalidOperationException("GetUserId null");

            var doc = await repo.Get<NextDestinations>(DocumentType.NextDestinations, userId, cancellationToken);

            return await req.CreateResponse(doc, TtlCache.OneDay, cancellationToken);
        }
        catch (Exception ex)
        {
            req.LogError(ex);
            throw;
        }
    }

    [Function("NextDestinationsAdd")]
    public async Task<NextDestinations?> NextDestinationsAdd(
        [HttpTrigger(AuthorizationLevel.Anonymous, Method.Post, Route = "next-destinations/add")] HttpRequestData req, CancellationToken cancellationToken)
    {
        try
        {
            var userId = await req.GetUserIdAsync(cancellationToken);
            if (string.IsNullOrEmpty(userId)) throw new InvalidOperationException("GetUserId null");
            var entry = await req.GetPublicBody<NextDestinationsEntry>(cancellationToken: cancellationToken);

            var obj = await repo.Get<NextDestinations>(DocumentType.NextDestinations, userId, cancellationToken);

            if (obj == null)
            {
                obj = new NextDestinations();

                obj.Initialize(userId);
            }

            obj.Items.Add(entry);

            return await repo.UpsertItemAsync(obj, cancellationToken);
        }
        catch (Exception ex)
        {
            req.LogError(ex);
            throw;
        }
    }

    [Function("NextDestinationsUpdate")]
    public async Task<NextDestinations?> NextDestinationsUpdate(
      [HttpTrigger(AuthorizationLevel.Anonymous, Method.Put, Route = "next-destinations/update")] HttpRequestData req, CancellationToken cancellationToken)
    {
        try
        {
            var userId = await req.GetUserIdAsync(cancellationToken);
            if (string.IsNullOrEmpty(userId)) throw new InvalidOperationException("GetUserId null");
            var entry = await req.GetPublicBody<NextDestinationsEntry>(cancellationToken: cancellationToken);

            var obj = await repo.Get<NextDestinations>(DocumentType.NextDestinations, userId, cancellationToken);

            var dbEntry = obj!.Items.Single(x => x.Id == entry.Id);

            dbEntry.StartDate = entry.StartDate;
            dbEntry.EndDate = entry.EndDate;
            dbEntry.RegionCode = entry.RegionCode;
            dbEntry.CityCode = entry.CityCode;
            dbEntry.RegionName = entry.RegionName;
            dbEntry.CityName = entry.CityName;
            dbEntry.Notes = entry.Notes;

            return await repo.UpsertItemAsync(obj, cancellationToken);
        }
        catch (Exception ex)
        {
            req.LogError(ex);
            throw;
        }
    }

    [Function("NextDestinationsRemove")]
    public async Task<NextDestinations?> NextDestinationsRemove(
        [HttpTrigger(AuthorizationLevel.Anonymous, Method.Post, Route = "next-destinations/remove/{id}")] HttpRequestData req, string id, CancellationToken cancellationToken)
    {
        try
        {
            var userId = await req.GetUserIdAsync(cancellationToken);
            if (string.IsNullOrEmpty(userId)) throw new InvalidOperationException("GetUserId null");

            var obj = await repo.Get<NextDestinations>(DocumentType.NextDestinations, userId, cancellationToken);

            if (obj == null)
            {
                obj = new NextDestinations();

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
        catch (Exception ex)
        {
            req.LogError(ex);
            throw;
        }
    }
}