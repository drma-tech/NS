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
            var code = req.GetQueryParameters()["code"];
            if (string.IsNullOrEmpty(code)) throw new InvalidOperationException("code null");

            var model = await repo.Get<CountryData>(DocumentType.Country, code.ToUpper(), cancellationToken);

            return await req.CreateResponse(model, TtlCache.OneWeek, cancellationToken);
        }
        catch (Exception ex)
        {
            req.ProcessException(ex);
            throw;
        }
    }
}