using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using NS.API.Core.Auth;
using NS.API.Core.Models;
using NS.API.Core.Scraping;
using NS.Shared.Models.Country;
using System.Globalization;
using System.Text.Json;

namespace NS.API.Functions;

public class ScrapFunction(CosmosGroupRepository repo, IHttpClientFactory factory)
{
    [Function("ScrapData")]
    public async Task ScrapData(
       [HttpTrigger(AuthorizationLevel.Anonymous, Method.Post, Route = "adm/scrap/{field}")] HttpRequestData req, Field field, CancellationToken cancellationToken)
    {
        var userId = await req.GetUserIdAsync(cancellationToken);
        if (userId.Empty() || (!userId.StartsWith("EPwJHGkTKIYb") && !userId.StartsWith("091382f5")))
        {
            throw new NotificationException("invalid request");
        }

        var modelsToUpdate = new List<RegionData>();
        int totalSuccesses = 0;
        int totalFailures = 0;

        var import = await repo.Get<CountryImport>(DocumentType.Import, field.ToString(), cancellationToken);
        if (import == null)
        {
            import = new CountryImport();
            import.Initialize(field.ToString());
        }

        var regions = await repo.ListAll<RegionData>(DocumentType.Country, cancellationToken);
        var regionDict = regions.ToDictionary(c => c.Id.Split(":")[1], c => c, StringComparer.OrdinalIgnoreCase);

        var scrapData = await ScrapingBasic.GetData(field, factory, ApiStartup.Configurations, repo, cancellationToken);

        var path = Path.Combine(Directory.GetCurrentDirectory(), "data", "regions.json");
        var jsonContent = await File.ReadAllTextAsync(path, cancellationToken);
        var LocalRegions = JsonSerializer.Deserialize<AllRegions>(jsonContent);

        ////reset taxi apps
        //foreach (var item in LocalCountries)
        //{
        //    var localCountry = LocalCountries.FirstOrDefault(p => p.Value.ToString().Equals(item.Value.ToString(), StringComparison.CurrentCultureIgnoreCase));

        //    if (regionDict.TryGetValue(localCountry!.Value.ToString(), out var model))
        //    {
        //        model.TaxiApps.Clear();
        //        modelsToUpdate.Add(model);
        //    }
        //}

        ////reset conflicts
        //foreach (var item in LocalRegions?.Items ?? [])
        //{
        //    var localCountry = LocalRegions?.GetByCode(item.code);

        //    if (regionDict.TryGetValue(localCountry!.code!, out var model))
        //    {
        //        model.ConflictLevel = ConflictLevel.Minimal;
        //        model.ConflictForecast = null;
        //        modelsToUpdate.Add(model);
        //    }
        //}

        ////reset tourism index
        //foreach (var item in LocalRegions?.Items ?? [])
        //{
        //    var localCountry = LocalRegions?.GetByCode(item.code);

        //    if (regionDict.TryGetValue(localCountry!.code!, out var model))
        //    {
        //        model.TourismIndex = null;
        //        modelsToUpdate.Add(model);
        //    }
        //}

        if (field == Field.CapitalCities)
        {
            foreach (var region in LocalRegions!.Items.Where(c => c.capital.NotEmpty()))
            {
                scrapData.Add(region.name!, new List<string>() { region.capital! });
            }
        }

        //temporary - numbeo is limiting access
        if (field == Field.NumbeoSafetyIndex)
        {
            foreach (var item in regions)
            {
                if (!item.NumbeoSafetyIndex.HasValue) continue;
                var region = LocalRegions!.Items.Single(p => p.code!.Equals(item.Id.Split(":")[1], StringComparison.CurrentCultureIgnoreCase));
                scrapData.Add(region.name!, item.NumbeoSafetyIndex);
            }
        }

        //temporary - numbeo is limiting access
        if (field == Field.NumbeoPollutionIndex)
        {
            foreach (var item in regions)
            {
                if (!item.NumbeoPollutionIndex.HasValue) continue;
                var region = LocalRegions!.Items.Single(p => p.code!.Equals(item.Id.Split(":")[1], StringComparison.CurrentCultureIgnoreCase));
                scrapData.Add(region.name!, item.NumbeoPollutionIndex);
            }
        }

        foreach (var scrap in scrapData)
        {
            var localRegion = LocalRegions?.GetByName(scrap.Key);

            if (localRegion == null) //try to find by custom names
            {
                var code = import.CustomNames.FirstOrDefault(p => p.Value.Equals(scrap.Key, StringComparison.CurrentCultureIgnoreCase)).Key;
                localRegion = LocalRegions?.GetByCode(code);
            }

            if (localRegion != null)
            {
                if (regionDict.TryGetValue(localRegion.code!, out var model))
                {
                    totalSuccesses++;
                    PopulateField(model, field, scrap.Value);
                    modelsToUpdate.Add(model);
                }
                else
                {
                    totalFailures++;
                    req.LogWarning($"country not registered: {localRegion.code!.ToUpper()}");
                }
            }
            else
            {
                totalFailures++;
                import.CustomNames.Add(scrap.Key, scrap.Key);
                req.LogWarning($"local country not found: {scrap.Key}");
            }
        }

        if ((int)field < 1000) //do not include Guide, Cost of Living and Other
        {
            var score = new Score()
            {
                Title = field.GetName(false),
                SubTitle = field.GetPlaceholder(false),
                Icon = "chart-simple"
            };

            score.Initialize(field.ToString().ToSlug()!);

            foreach (var model in modelsToUpdate.OrderBy(p => p.Id))
            {
                PopulateScoreDetail(score, field, model);
            }

            await repo.UpsertItemAsync(score, cancellationToken);
        }

        await repo.BulkUpsertAsync(modelsToUpdate, cancellationToken);

        import.Events.Add(new ImportEvent { Success = totalSuccesses, Failure = totalFailures });
        await repo.UpsertItemAsync(import, cancellationToken);
    }

    private static void PopulateScoreDetail(Score score, Field field, RegionData model)
    {
        var code = model.Id.Split(":")[1];

        if (field == Field.CorruptionScore)
        {
            score.Items.Add(new ScoreDetail { Code = code, Value = model.CorruptionScore });
        }
        else if (field == Field.HDI)
        {
            score.Items.Add(new ScoreDetail { Code = code, Value = model.HDI });
        }
        else if (field == Field.DMDemocracyIndex)
        {
            score.Items.Add(new ScoreDetail { Code = code, Value = model.DMDemocracyIndex });
        }
        else if (field == Field.EconomistDemocracyIndex)
        {
            score.Items.Add(new ScoreDetail { Code = code, Value = model.EconomistDemocracyIndex });
        }
        else if (field == Field.FreedomExpressionIndex)
        {
            score.Items.Add(new ScoreDetail { Code = code, Value = model.FreedomExpressionIndex });
        }
        else if (field == Field.FreedomScore)
        {
            if (model.FreedomScore.HasValue)
            {
                score.Items.Add(new ScoreDetail { Code = code, Value = model.FreedomScore });
            }
        }
        else if (field == Field.CensorshipIndex)
        {
            score.Items.Add(new ScoreDetail { Code = code, Value = model.CensorshipIndex });
        }
        else if (field == Field.HappinessIndex)
        {
            score.Items.Add(new ScoreDetail { Code = code, Value = model.HappinessIndex });
        }
        else if (field == Field.OECD)
        {
            score.Items.Add(new ScoreDetail { Code = code, Value = null });
        }
        else if (field == Field.GDP_PPP)
        {
            score.Items.Add(new ScoreDetail { Code = code, Value = model.GDP_PPP });
        }
        else if (field == Field.GDP_Nominal)
        {
            score.Items.Add(new ScoreDetail { Code = code, Value = model.GDP_Nominal });
        }
        else if (field == Field.EconomicFreedomIndex)
        {
            if (model.EconomicFreedomIndex.HasValue)
            {
                score.Items.Add(new ScoreDetail { Code = code, Value = model.EconomicFreedomIndex });
            }
        }
        else if (field == Field.CashlessIndex)
        {
            score.Items.Add(new ScoreDetail { Code = code, Value = model.CashlessIndex });
        }
        else if (field == Field.TsaSafetyIndex)
        {
            score.Items.Add(new ScoreDetail { Code = code, Value = model.TsaSafetyIndex });
        }
        else if (field == Field.NumbeoSafetyIndex)
        {
            score.Items.Add(new ScoreDetail { Code = code, Value = model.NumbeoSafetyIndex });
        }
        else if (field == Field.GlobalTerrorismIndex)
        {
            score.Items.Add(new ScoreDetail { Code = code, Value = model.GlobalTerrorismIndex });
        }
        else if (field == Field.GlobalPeaceIndex)
        {
            score.Items.Add(new ScoreDetail { Code = code, Value = model.GlobalPeaceIndex });
        }
        else if (field == Field.YaleWaterScore)
        {
            score.Items.Add(new ScoreDetail { Code = code, Value = model.YaleWaterScore });
        }
        else if (field == Field.NumbeoPollutionIndex)
        {
            score.Items.Add(new ScoreDetail { Code = code, Value = model.NumbeoPollutionIndex });
        }
        else if (field == Field.AirQuality)
        {
            score.Items.Add(new ScoreDetail { Code = code, Value = model.AirQuality });
        }
        else if (field == Field.HealthCareIndex)
        {
            score.Items.Add(new ScoreDetail { Code = code, Value = model.HealthCareIndex });
        }
        else if (field == Field.AnnualTemperature)
        {
            score.Items.Add(new ScoreDetail { Code = code, Value = model.AnnualTemperature });
        }
        else if (field == Field.VisaFree)
        {
            score.Items.Add(new ScoreDetail { Code = code, Value = model.VisaFree });
        }
        else if (field == Field.TourismIndex)
        {
            score.Items.Add(new ScoreDetail { Code = code, Value = model.TourismIndex });
        }
        else if (field == Field.AirConnectivityIndex)
        {
            score.Items.Add(new ScoreDetail { Code = code, Value = model.AirConnectivityIndex });
        }
        else if (field == Field.SustainableMobilityIndex)
        {
            score.Items.Add(new ScoreDetail { Code = code, Value = model.SustainableMobilityIndex });
        }
    }

    private static void PopulateField(RegionData model, Field field, object? value)
    {
        if (field == Field.VisaFree)
        {
            model.VisaFree = value.ConvertToInt();
        }
        else if (field == Field.CorruptionScore)
        {
            model.CorruptionScore = value.ConvertToDouble() / 10;
        }
        else if (field == Field.HDI)
        {
            model.HDI = value.ConvertToDouble();
        }
        else if (field == Field.OECD)
        {
            model.OECD = value != null && bool.Parse(value.ToString()!);
        }
        else if (field == Field.TsaSafetyIndex)
        {
            model.TsaSafetyIndex = value.ConvertToDouble();
        }
        else if (field == Field.NumbeoSafetyIndex)
        {
            model.NumbeoSafetyIndex = value.ConvertToDouble();
        }
        else if (field == Field.DMDemocracyIndex)
        {
            model.DMDemocracyIndex = value.ConvertToDouble();
        }
        else if (field == Field.EconomistDemocracyIndex)
        {
            model.EconomistDemocracyIndex = value.ConvertToDouble();
        }
        else if (field == Field.FreedomExpressionIndex)
        {
            model.FreedomExpressionIndex = value.ConvertToDouble();
        }
        else if (field == Field.CashlessIndex)
        {
            model.CashlessIndex = value.ConvertToDouble();
        }
        else if (field == Field.HappinessIndex)
        {
            model.HappinessIndex = value.ConvertToDouble();
        }
        else if (field == Field.AirQuality)
        {
            model.AirQuality = value.ConvertToDouble();
        }
        else if (field == Field.HealthCareIndex)
        {
            model.HealthCareIndex = value.ConvertToDouble();
        }
        else if (field == Field.AnnualTemperature)
        {
            model.AnnualTemperature = value.ConvertToDouble();
        }
        else if (field == Field.GDP_PPP)
        {
            model.GDP_PPP = value.ConvertToDouble();
        }
        else if (field == Field.GDP_Nominal)
        {
            model.GDP_Nominal = value.ConvertToDouble();
        }
        else if (field == Field.EconomicFreedomIndex)
        {
            model.EconomicFreedomIndex = value.ConvertToDouble();
        }
        else if (field == Field.CensorshipIndex)
        {
            model.CensorshipIndex = value.ConvertToDouble();
        }
        else if (field == Field.FreedomScore)
        {
            model.FreedomScore = value.ConvertToDouble();
        }
        else if (field == Field.YaleWaterScore)
        {
            model.YaleWaterScore = value.ConvertToDouble();
        }
        else if (field == Field.NumbeoPollutionIndex)
        {
            model.NumbeoPollutionIndex = value.ConvertToDouble();
        }
        else if (field == Field.GlobalTerrorismIndex)
        {
            model.GlobalTerrorismIndex = value.ConvertToDouble();
        }
        else if (field == Field.GlobalPeaceIndex)
        {
            model.GlobalPeaceIndex = value.ConvertToDouble();
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
        else if (field == Field.AptOutsideCenter)
        {
            var expenses = (HashSet<Expense>)value!;

            var center = expenses.FirstOrDefault(p => p.Type == ExpenseType.AptCityCenter);
            if (model.AptCityCenter != null) model.AptCityCenter.Score = center?.Score;

            var outside = expenses.FirstOrDefault(p => p.Type == ExpenseType.AptOutsideCenter);
            if (model.AptOutsideCenter != null) model.AptOutsideCenter.Score = outside?.Score;

            var meal = expenses.FirstOrDefault(p => p.Type == ExpenseType.Meal);
            if (model.Meal != null) model.Meal.Score = meal?.Score;

            var western = expenses.FirstOrDefault(p => p.Type == ExpenseType.MarketWestern);
            if (model.MarketWestern != null) model.MarketWestern.Score = western?.Score;

            var asian = expenses.FirstOrDefault(p => p.Type == ExpenseType.MarketAsian);
            if (model.MarketAsian != null) model.MarketAsian.Score = asian?.Score;
        }
        else if (field == Field.TourismIndex)
        {
            model.TourismIndex = value.ConvertToDouble();
        }
        else if (field == Field.AirConnectivityIndex)
        {
            model.AirConnectivityIndex = value.ConvertToDouble();
        }
        else if (field == Field.SustainableMobilityIndex)
        {
            model.SustainableMobilityIndex = value.ConvertToDouble();
        }
        else if (field == Field.Languages)
        {
            var languages = (HashSet<string>)value!;

            model.Languages = languages.Select(s => Enum.Parse<Language>(s.Replace(" ", ""))).ToHashSet();
        }
        else if (field == Field.Currencies)
        {
            model.Currencies = (HashSet<string>)value!;
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
        else if (field == Field.Tax)
        {
            var tax = (Taxes)value!;
            model.Taxes ??= new Taxes();

            model.Taxes.Corporate = tax.Corporate.NotEmpty() ? tax.Corporate : null;
            model.Taxes.IncomeLowest = tax.IncomeLowest.NotEmpty() ? tax.IncomeLowest : null;
            model.Taxes.IncomeHighest = tax.IncomeHighest.NotEmpty() ? tax.IncomeHighest : null;
            model.Taxes.CapitalGains = tax.CapitalGains.NotEmpty() ? tax.CapitalGains : null;
            model.Taxes.Wealth = tax.Wealth.NotEmpty() ? tax.Wealth : null;
            model.Taxes.Property = tax.Property.NotEmpty() ? tax.Property : null;
            model.Taxes.InheritanceEstate = tax.InheritanceEstate.NotEmpty() ? tax.InheritanceEstate : null;
            model.Taxes.VATGSTSales = tax.VATGSTSales.NotEmpty() ? tax.VATGSTSales : null;
        }
        else if (field == Field.EmergencyNumbers)
        {
            var emergencyNumbers = (EmergencyNumbers)value!;
            model.EmergencyNumbers ??= new EmergencyNumbers();

            model.EmergencyNumbers.Police = emergencyNumbers.Police.NotEmpty() ? emergencyNumbers.Police : null;
            model.EmergencyNumbers.Ambulance = emergencyNumbers.Ambulance.NotEmpty() ? emergencyNumbers.Ambulance : null;
            model.EmergencyNumbers.Fire = emergencyNumbers.Fire.NotEmpty() ? emergencyNumbers.Fire : null;
            model.EmergencyNumbers.Others = emergencyNumbers.Others.NotEmpty() ? emergencyNumbers.Others : null;
        }
        else if (field == Field.Conflicts)
        {
            var values = value?.ToString();

            model.ConflictLevel = EnumHelper.GetArray<ConflictLevel>().SingleOrDefault(p => p.GetName() == values);
        }
        else if (field == Field.GlobalCities)
        {
            var cities = (List<string>?)value ?? [];

            model.Cities = new HashSet<string>(cities);
        }
        else if (field == Field.TSACities || field == Field.CapitalCities)
        {
            var cities = (List<string>?)value ?? [];

            var newCities = new HashSet<string>(cities);
            var compareInfo = CultureInfo.InvariantCulture.CompareInfo;

            foreach (var city in newCities)
            {
                var exists = model.Cities.Any(existing => compareInfo.Compare(existing, city, CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) == 0);

                if (!exists)
                {
                    model.Cities.Add(city);
                }
            }
        }
        else if (field == Field.Tipping)
        {
            var risks = (Tipping)value!;
            model.Tipping ??= new Tipping();

            model.Tipping.Restaurant = risks.Restaurant;
            model.Tipping.Hotel = risks.Hotel;
            model.Tipping.Driver = risks.Driver;
        }
        else if (field == Field.BroadbandSpeed)
        {
            model.BroadbandSpeed = value.ConvertToDouble();
        }
    }
}