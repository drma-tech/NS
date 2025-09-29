using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using NS.API.Core.Scraping;
using NS.Shared.Models.Country;

namespace NS.API.Functions;

public class ScrapFunction(CosmosGroupRepository repo, IHttpClientFactory factory)
{
    [Function("ScrapData")]
    public async Task ScrapData(
       [HttpTrigger(AuthorizationLevel.Anonymous, Method.Post, Route = "adm/scrap/{field}")] HttpRequestData req, Field field, CancellationToken cancellationToken)
    {
        try
        {
            var http = factory.CreateClient("generic");
            var modelsToUpdate = new List<CountryData>();
            int totalSuccesses = 0;
            int totalFailures = 0;

            var import = await repo.Get<CountryImport>(DocumentType.Import, field.ToString(), cancellationToken);
            if (import == null)
            {
                import = new CountryImport();
                import.Initialize(field.ToString());
            }

            var countries = await repo.ListAll<CountryData>(DocumentType.Country, cancellationToken);
            var countryDict = countries.ToDictionary(c => c.Id.Split(":")[1], c => c, StringComparer.OrdinalIgnoreCase);

            var scrapData = await ScrapingBasic.GetData(field, http, import);
            var LocalCountries = await req.GetPublicBody<AllCountries>(cancellationToken);

            foreach (var scrap in scrapData)
            {
                var localCountry = LocalCountries.Items.FirstOrDefault(p => p.Name.Equals(scrap.Key, StringComparison.CurrentCultureIgnoreCase));

                if (localCountry == null) //try to find by custom names
                {
                    var code = import.CustomNames.FirstOrDefault(p => p.Value.Equals(scrap.Key, StringComparison.CurrentCultureIgnoreCase)).Key;
                    localCountry = LocalCountries.Items.FirstOrDefault(p => p.Id.Equals(code, StringComparison.CurrentCultureIgnoreCase));
                }

                if (localCountry != null)
                {
                    if (countryDict.TryGetValue(localCountry.Id, out var model))
                    {
                        totalSuccesses++;
                        PopulateField(model, field, scrap.Value);
                        modelsToUpdate.Add(model);
                    }
                    else
                    {
                        totalFailures++;
                        req.LogWarning($"country not registered: {localCountry.Id.ToUpper()}");
                    }
                }
                else
                {
                    totalFailures++;
                    import.CustomNames.Add(scrap.Key, scrap.Key);
                    req.LogWarning($"local country not found: {scrap.Key}");
                }
            }

            await repo.BulkUpsertAsync(modelsToUpdate, cancellationToken);

            import.Events.Add(new ImportEvent { Success = totalSuccesses, Failure = totalFailures });
            await repo.UpsertItemAsync(import, cancellationToken);
        }
        catch (Exception ex)
        {
            req.ProcessException(ex);
            throw;
        }
    }

    private static void PopulateField(CountryData model, Field field, object? value)
    {
        if (value == null) return;

        if (field == Field.Population)
        {
            model.Population = int.Parse(value.ToString()!);
        }
        else if (field == Field.UnMember)
        {
            model.UnMember = bool.Parse(value.ToString()!);
        }
        else if (field == Field.VisaFree)
        {
            model.VisaFree = int.Parse(value.ToString()!);
        }
        else if (field == Field.CorruptionScore)
        {
            model.CorruptionScore = int.Parse(value.ToString()!);
        }
        else if (field == Field.HDI)
        {
            model.HDI = (float)Math.Round(float.Parse(value.ToString()!), 3);
        }
        else if (field == Field.Area)
        {
            model.Area = int.Parse(value.ToString()!);
        }
        else if (field == Field.OECD)
        {
            model.OECD = bool.Parse(value.ToString()!);
        }
        else if (field == Field.TsaSafetyIndex)
        {
            model.TsaSafetyIndex = int.Parse(value.ToString()!);
        }
        else if (field == Field.NumbeoSafetyIndex)
        {
            model.NumbeoSafetyIndex = float.Parse(value.ToString()!);
        }
    }
}