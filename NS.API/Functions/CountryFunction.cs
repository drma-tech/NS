using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using NS.Shared.Models.Country;

namespace NS.API.Functions;

public class CountryFunction(CosmosGroupRepository repo)
{
    [Function("CountryStart")]
    public async Task CountryStart(
       [HttpTrigger(AuthorizationLevel.Anonymous, Method.Post, Route = "adm/country/start")] HttpRequestData req, CancellationToken cancellationToken)
    {
        try
        {
            var body = await req.GetPublicBody<AllCountries>(cancellationToken);

            var modelsToUpdate = new List<CountryData>();

            foreach (var item in body.Items.OrderBy(o => o.Id))
            {
                var country = new CountryData();
                country.Initialize(item.Id);

                modelsToUpdate.Add(country);
            }

            await repo.BulkUpsertAsync(modelsToUpdate, cancellationToken);
        }
        catch (Exception ex)
        {
            req.ProcessException(ex);
            throw;
        }
    }

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