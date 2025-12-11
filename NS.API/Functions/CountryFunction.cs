using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using NS.Shared.Models.Country;

namespace NS.API.Functions;

public class CountryFunction(CosmosGroupRepository repo)
{
    [Function("CountryGet")]
    public async Task<HttpResponseData?> CountryGet(
        [HttpTrigger(AuthorizationLevel.Anonymous, Method.Get, Route = "public/country/get")] HttpRequestData req, CancellationToken cancellationToken)
    {
        try
        {
            var region = req.GetQueryParameters()["region"];
            if (string.IsNullOrEmpty(region)) throw new InvalidOperationException("region null");

            var model = await repo.Get<CountryData>(DocumentType.Country, region.ToUpper(), cancellationToken);

            return await req.CreateResponse(model, TtlCache.OneWeek, cancellationToken);
        }
        catch (Exception ex)
        {
            req.LogError(ex);
            throw;
        }
    }
}