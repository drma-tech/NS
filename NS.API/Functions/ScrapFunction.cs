using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using NS.API.Core.Models;
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
            var LocalCountries = EnumHelper.GetListCountry<Shared.Enums.Country>();

            ////reset taxi apps
            //foreach (var item in LocalCountries)
            //{
            //    var localCountry = LocalCountries.FirstOrDefault(p => p.Value.ToString().Equals(item.Value.ToString(), StringComparison.CurrentCultureIgnoreCase));

            //    if (countryDict.TryGetValue(localCountry!.Value.ToString(), out var model))
            //    {
            //        model.TaxiApps.Clear();
            //        modelsToUpdate.Add(model);
            //    }
            //}

            foreach (var scrap in scrapData)
            {
                var localCountry = LocalCountries.FirstOrDefault(p => p.Name.Equals(scrap.Key, StringComparison.CurrentCultureIgnoreCase));

                if (localCountry == null) //try to find by custom names
                {
                    var code = import.CustomNames.FirstOrDefault(p => p.Value.Equals(scrap.Key, StringComparison.CurrentCultureIgnoreCase)).Key;
                    localCountry = LocalCountries.FirstOrDefault(p => p.Value.ToString().Equals(code, StringComparison.CurrentCultureIgnoreCase));
                }

                if (localCountry != null)
                {
                    if (countryDict.TryGetValue(localCountry.Value.ToString(), out var model))
                    {
                        totalSuccesses++;
                        PopulateField(model, field, scrap.Value);
                        modelsToUpdate.Add(model);
                    }
                    else
                    {
                        totalFailures++;
                        req.LogWarning($"country not registered: {localCountry.Value.ToString().ToUpper()}");
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
        if (field == Field.VisaFree)
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
        else if (field == Field.TaxiApps)
        {
            model.TaxiApps.Add((TaxiApp)value!);
        }
        else if (field == Field.AptCityCenter)
        {
            var expenses = (HashSet<Expense>)value!;

            var center = expenses.FirstOrDefault(p => p.Type == ExpenseType.AptCityCenter);
            model.AptCityCenter = new PriceRange { Min = center?.MinPrice, Avg = center?.Price, Max = center?.MaxPrice };

            var outside = expenses.FirstOrDefault(p => p.Type == ExpenseType.AptOutsideCenter);
            model.AptOutsideCenter = new PriceRange { Min = outside?.MinPrice, Avg = outside?.Price, Max = outside?.MaxPrice };

            var meal = expenses.FirstOrDefault(p => p.Type == ExpenseType.Meal);
            model.Meal = new PriceRange { Min = meal?.MinPrice, Avg = meal?.Price, Max = meal?.MaxPrice };

            var western = expenses.FirstOrDefault(p => p.Type == ExpenseType.MarketWestern);
            model.MarketWestern = new PriceRange { Min = western?.MinPrice, Avg = western?.Price, Max = western?.MaxPrice };

            var asian = expenses.FirstOrDefault(p => p.Type == ExpenseType.MarketAsian);
            model.MarketAsian = new PriceRange { Min = asian?.MinPrice, Avg = asian?.Price, Max = asian?.MaxPrice };
        }
        else if (field == Field.TourismIndex)
        {
            model.TourismIndex = ConvertToInt(value);
        }
        else if (field == Field.Languages)
        {
            var languages = (HashSet<string>)value!;

            model.Languages = languages.Select(s => Enum.Parse<Language>(s.Replace(" ", ""))).ToHashSet();
        }
        else if (field == Field.Risks)
        {
            var risks = (Risks)value!;
            model.Risks ??= new Risks();

            model.Risks.TransportTaxis = risks.TransportTaxis;
            model.Risks.Pickpockets = risks.Pickpockets;
            model.Risks.NaturalDisasters = risks.NaturalDisasters;
            model.Risks.Mugging = risks.Mugging;
            model.Risks.Terrorism = risks.Terrorism;
            model.Risks.Scams = risks.Scams;
            model.Risks.WomenTravelers = risks.WomenTravelers;
            model.Risks.TapWater = risks.TapWater;
        }
    }
}