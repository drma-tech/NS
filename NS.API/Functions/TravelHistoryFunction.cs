using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using NS.API.Core.Auth;

namespace NS.API.Functions;

public class TravelHistoryFunction(CosmosRepository repo)
{
    [Function("TravelHistoryGet")]
    public async Task<HttpResponseData?> TravelHistoryGet(
        [HttpTrigger(AuthorizationLevel.Anonymous, Method.Get, Route = "travel-history/get")] HttpRequestData req, CancellationToken cancellationToken)
    {
        try
        {
            var userId = await req.GetUserIdAsync(cancellationToken);
            if (userId.Empty()) throw new InvalidOperationException("GetUserId null");

            var doc = await repo.Get<TravelHistory>(DocumentType.TravelHistory, userId, cancellationToken);

            return await req.CreateResponse(doc, TtlCache.OneDay, cancellationToken);
        }
        catch (Exception ex)
        {
            req.LogError(ex);
            throw;
        }
    }

    [Function("TravelHistoryAdd")]
    public async Task<TravelHistory?> TravelHistoryAdd(
        [HttpTrigger(AuthorizationLevel.Anonymous, Method.Post, Route = "travel-history/add")] HttpRequestData req, CancellationToken cancellationToken)
    {
        try
        {
            var userId = await req.GetUserIdAsync(cancellationToken);
            if (string.IsNullOrEmpty(userId)) throw new InvalidOperationException("GetUserId null");
            var entry = await req.GetPublicBody<TravelHistoryEntry>(cancellationToken: cancellationToken);

            var obj = await repo.Get<TravelHistory>(DocumentType.TravelHistory, userId, cancellationToken);

            if (obj == null)
            {
                obj = new TravelHistory();

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

    [Function("TravelHistoryUpdate")]
    public async Task<TravelHistory?> TravelHistoryUpdate(
      [HttpTrigger(AuthorizationLevel.Anonymous, Method.Put, Route = "travel-history/update")] HttpRequestData req, CancellationToken cancellationToken)
    {
        try
        {
            var userId = await req.GetUserIdAsync(cancellationToken);
            if (string.IsNullOrEmpty(userId)) throw new InvalidOperationException("GetUserId null");
            var entry = await req.GetPublicBody<TravelHistoryEntry>(cancellationToken: cancellationToken);

            var obj = await repo.Get<TravelHistory>(DocumentType.TravelHistory, userId, cancellationToken);

            var dbEntry = obj!.Items.Single(x => x.Id == entry.Id);

            dbEntry.StartDate = entry.StartDate;
            dbEntry.EndDate = entry.EndDate;
            dbEntry.RegionCode = entry.RegionCode;
            dbEntry.CityCode = entry.CityCode;
            dbEntry.RegionName = entry.RegionName;
            dbEntry.CityName = entry.CityName;
            dbEntry.RegionRating = entry.RegionRating;
            dbEntry.CityRating = entry.CityRating;
            dbEntry.Notes = entry.Notes;

            return await repo.UpsertItemAsync(obj, cancellationToken);
        }
        catch (Exception ex)
        {
            req.LogError(ex);
            throw;
        }
    }

    [Function("TravelHistoryRemove")]
    public async Task<TravelHistory?> TravelHistoryRemove(
        [HttpTrigger(AuthorizationLevel.Anonymous, Method.Post, Route = "travel-history/remove/{id}")] HttpRequestData req, string id, CancellationToken cancellationToken)
    {
        try
        {
            var userId = await req.GetUserIdAsync(cancellationToken);
            if (string.IsNullOrEmpty(userId)) throw new InvalidOperationException("GetUserId null");

            var obj = await repo.Get<TravelHistory>(DocumentType.TravelHistory, userId, cancellationToken);

            if (obj == null)
            {
                obj = new TravelHistory();

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