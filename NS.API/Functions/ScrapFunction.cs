using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using NS.API.Core.Scraping;
using NS.Shared.Models.Country;
using System.Globalization;

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

            var scrapData = await ScrapingBasic.GetData(field, http);
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

    private static int? ConvertToInt(object? value)
    {
        return value == null ? null : (int)decimal.Parse(value.ToString()!, CultureInfo.InvariantCulture);
    }

    private static void PopulateField(CountryData model, Field field, object? value)
    {
        if (field == Field.Population)
        {
            model.Population = ConvertToInt(value);
        }
        else if (field == Field.UnMember)
        {
            model.UnMember = value != null && bool.Parse(value.ToString()!);
        }
        else if (field == Field.VisaFree)
        {
            model.VisaFree = ConvertToInt(value);
        }
        else if (field == Field.CorruptionScore)
        {
            model.CorruptionScore = ConvertToInt(value);
        }
        else if (field == Field.HDI)
        {
            model.HDI = ConvertToInt(value);
        }
        else if (field == Field.Area)
        {
            model.Area = ConvertToInt(value);
        }
        else if (field == Field.OECD)
        {
            model.OECD = value != null && bool.Parse(value.ToString()!);
        }
        else if (field == Field.TsaSafetyIndex)
        {
            model.TsaSafetyIndex = ConvertToInt(value);
        }
        else if (field == Field.NumbeoSafetyIndex)
        {
            model.NumbeoSafetyIndex = ConvertToInt(value);
        }
        else if (field == Field.DMDemocracyIndex)
        {
            model.DMDemocracyIndex = ConvertToInt(value);
        }
        else if (field == Field.DMClassification)
        {
            model.DMClassification = value == null ? null : (DMClassification?)value;
        }
        else if (field == Field.EconomistDemocracyIndex)
        {
            model.EconomistDemocracyIndex = ConvertToInt(value);
        }
        else if (field == Field.EconomistRegimeType)
        {
            model.EconomistRegimeType = value == null ? null : (EconomistRegimeType?)value;
        }
        else if (field == Field.FreedomExpressionIndex)
        {
            model.FreedomExpressionIndex = ConvertToInt(value);
        }
        else if (field == Field.HappinessIndex)
        {
            model.HappinessIndex = ConvertToInt(value);
        }
        else if (field == Field.GDP_PPP)
        {
            model.GDP_PPP = value == null ? null : decimal.Parse(value.ToString()!);
        }
        else if (field == Field.GDP_Nominal)
        {
            model.GDP_Nominal = value == null ? null : decimal.Parse(value.ToString()!);
        }
        else if (field == Field.EconomicFreedomIndex)
        {
            model.EconomicFreedomIndex = ConvertToInt(value);
        }
        else if (field == Field.InternationalArrivals)
        {
            model.InternationalArrivals = ConvertToInt(value);
        }
        else if (field == Field.CensorshipIndex)
        {
            model.CensorshipIndex = ConvertToInt(value);
        }
        else if (field == Field.FreedomScore)
        {
            model.FreedomScore = ConvertToInt(value);
        }
        else if (field == Field.YaleWaterScore)
        {
            model.YaleWaterScore = ConvertToInt(value);
        }
        else if (field == Field.NumbeoPollutionIndex)
        {
            model.NumbeoPollutionIndex = ConvertToInt(value);
        }
        else if (field == Field.GlobalTerrorismIndex)
        {
            model.GlobalTerrorismIndex = ConvertToInt(value);
        }
        else if (field == Field.GlobalPeaceIndex)
        {
            model.GlobalPeaceIndex = ConvertToInt(value);
        }
    }
}