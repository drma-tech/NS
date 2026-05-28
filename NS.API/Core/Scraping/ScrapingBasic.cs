using ExcelDataReader;
using HtmlAgilityPack;
using NS.API.Core.Models;
using NS.Shared.Models.Country;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace NS.API.Core.Scraping;

public static class ScrapingBasic
{
    public static void ProcessData(Field field)
    {
        switch (field)
        {
            case Field.Taxis:
                ProcessTaxis();
                break;
        }
    }

    public static async Task<Dictionary<string, object?>> GetData(Field field, IHttpClientFactory factory, Configurations config, CosmosGroupRepository repo, CancellationToken cancellationToken)
    {
        return field switch
        {
            //Society and Government (100)
            Field.CorruptionScore => await GetCorruptionScore(),
            Field.HDI => GetHDI(),
            Field.DMDemocracyIndex => GetDMDemocracyIndex(2),
            Field.EconomistDemocracyIndex => GetEconomistDemocracyIndex(4),
            Field.FreedomExpressionIndex => GetFreedomExpressionIndex(),
            Field.FreedomScore => GetFreedomScore(),
            Field.CensorshipIndex => await GetCensorshipIndex(),
            Field.HappinessIndex => GetHappinessIndex(),
            //Economy (200)
            Field.OECD => GetOECD(),
            Field.GDP_PPP => GetGDP("ppp"),
            Field.GDP_Nominal => GetGDP("nominal"),
            Field.EconomicFreedomIndex => GetEconomicFreedomIndex(),
            Field.CashlessIndex => GetCashlessIndex(),
            //Security and Peace (300)
            Field.TsaSafetyIndex => GetTsaSafetyIndex(),
            Field.NumbeoSafetyIndex => GetNumbeoSafetyIndex(),
            Field.GlobalTerrorismIndex => GetVisionOfHumanity("visionofhumanity-terrorism.html"),
            Field.GlobalPeaceIndex => GetVisionOfHumanity("visionofhumanity-peace.html"),
            //Environment and Health (400)
            Field.YaleWaterScore => GetYaleWaterScore(),
            Field.NumbeoPollutionIndex => GetNumbeoPollutionIndex(),
            Field.AirQuality => GetAirQuality(),
            Field.HealthCareIndex => GetHealthCareIndex(),
            Field.AnnualTemperature => GetAnnualTemperature(),
            //Mobility and Tourism (500)
            Field.VisaFree => await GetVisaFree(factory),
            Field.TourismIndex => await GetTourismIndex(),
            Field.AirConnectivityIndex => await GetAirConnectivityIndex(),
            Field.SustainableMobilityIndex => await GetSustainableMobilityIndex(),
            //Guide (1000)
            Field.Languages => await GetLanguages(),
            Field.Risks => GetRisks(),
            Field.Tipping => GetTipping(),
            Field.BroadbandSpeed => GetBroadbandSpeed(),
            Field.Tax => GetTax(),
            Field.EmergencyNumbers => GetEmergencyNumbers(),
            Field.Currencies => GetCurrencies(),
            Field.TravelRequirements => await GetTravelRequirements(factory, repo, config.Scraping?.Sherpa, cancellationToken),
            Field.Religions => GetReligions(),
            Field.Electricity => GetElectricity(),
            //Lifestyle (1100)
            Field.Income => GetIncome(),
            Field.AptCityCenter => GetNumbeoRangePrices(),
            Field.AptOutsideCenter => await GetNumbeoPriceScores(repo, cancellationToken),
            //Other (9000)
            Field.GlobalCities => await GetCities(repo, cancellationToken),
            Field.TSACities => GetTsaCities(),
            _ => [],
        };
    }

    private static async Task<Dictionary<string, object?>> GetVisaFree(IHttpClientFactory factory)
    {
        var client = factory.CreateClient("generic");

        var result = await client.GetApiData<HerleyData>("https://api.henleypassportindex.com/api/v3/countries", CancellationToken.None);

        return result?.countries.Where(w => w.visa_free_count > 0).ToDictionary(s => s.country!, s => (object?)s.visa_free_count) ?? [];
    }

    private static async Task<Dictionary<string, object?>> GetCorruptionScore()
    {
        //populate json manually
        //https://www.transparency.org/en/api/latest/cpi

        var path = Path.Combine(Directory.GetCurrentDirectory(), "data", "transparency-org.json");
        var jsonContent = await File.ReadAllTextAsync(path);
        var result = JsonSerializer.Deserialize<TransparencyData[]>(jsonContent);

        return result?.Where(p => p.year == 2025).ToDictionary(s => s.country!, s => (object?)s.score) ?? [];
    }

    private static async Task<Dictionary<string, object?>> GetAirConnectivityIndex()
    {
        //transform pdf to json with bot
        //https://www.iata.org/en/iata-repository/publications/economic-reports/economicsair-connectivity-measuring-the-connections-that-drive-economic-growth/

        var path = Path.Combine(Directory.GetCurrentDirectory(), "data", "air-connectivity-score.json");
        var jsonContent = await File.ReadAllTextAsync(path);
        var result = JsonSerializer.Deserialize<AirConnectivityIndex[]>(jsonContent);

        var countries = result?.ToDictionary(s => s.economy!, s => (double)s.connectivity_score_2019) ?? [];

        var (minPct, maxPct) = DataHelper.GetPercentiles(countries);

        var countryScores = countries.ToDictionary(
            kvp => kvp.Key,
            kvp => (object?)DataHelper.ConvertToScore(kvp.Value, minPct, maxPct, true)
        );

        return countryScores;
    }

    private static async Task<Dictionary<string, object?>> GetSustainableMobilityIndex()
    {
        //transform pdf to json with bot
        //https://www.sum4all.org/gra-tool/country-performance/snapshot
        //https://www.sum4all.org/sites/default/files/d7/mobilityataglancereport-2022-pagebypage_web.pdf

        var path = Path.Combine(Directory.GetCurrentDirectory(), "data", "sustainable-mobility-index.json");
        var jsonContent = await File.ReadAllTextAsync(path);
        var result = JsonSerializer.Deserialize<SustainableMobilityIndex[]>(jsonContent);

        return result?.ToDictionary(s => s.country!, s => (object?)(s.score / 10)) ?? [];
    }

    private static Dictionary<string, object?> GetHDI()
    {
        //download excel from website (Link: Download latest HDI dataset)
        //https://hdr.undp.org/data-center/human-development-index#/indicies/HDI

        var path = Path.Combine(Directory.GetCurrentDirectory(), "data", $"HDR25_Statistical_Annex_HDI_Table.xlsx");

        var dic = new Dictionary<string, object?>();
        using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using var reader = ExcelReaderFactory.CreateReader(stream);
            do
            {
                while (reader.Read())
                {
                    if (reader.IsDBNull(0)) continue; //first column has to be not null
                    if (!short.TryParse(reader.GetValue(0)?.ToString(), out short _)) continue; //first column has to be a valid number

                    dic.Add(reader.GetString(1), reader.GetDouble(2) * 10);
                }
            } while (reader.NextResult());
        }

        return dic;
    }

    private static Dictionary<string, object?> GetOECD()
    {
        var web = new HtmlWeb { OverrideEncoding = Encoding.UTF8 };
        var doc = web.Load("https://en.wikipedia.org/wiki/OECD");

        var table = doc.DocumentNode.SelectNodes("//table[starts-with(@class,'wikitable sortable mw-datatable')]")?.FirstOrDefault();

        if (table == null) return [];

        var result = new Dictionary<string, object?>();

        foreach (var tr in table.Element("tbody").Elements("tr"))
        {
            if (tr.Elements("th").FirstOrDefault()?.InnerText == "Country") continue;

            var td = tr.Elements("td").First();
            var a = td.Element("a");
            a ??= td.Element("span").Element("a");
            var name = a.InnerText.Trim();

            result.Add(name, true);
        }

        return result;
    }

    private static Dictionary<string, object?> GetTsaSafetyIndex()
    {
        var web = new HtmlWeb { OverrideEncoding = Encoding.UTF8 };
        var doc = web.Load("https://www.travelsafe-abroad.com/countries/");

        var tbodys = doc.DocumentNode.SelectNodes("//table[starts-with(@class,'sortable index-table')]/tbody")?.ToList();

        if (tbodys == null) return [];

        var result = new Dictionary<string, object?>();

        foreach (var tbody in tbodys)
        {
            var tds = tbody.Elements("td").ToList();

            for (int i = 0; i < tds.Count; i += 2)
            {
                var nameNode = tds[i].Element("a");
                var valueNode = tds[i + 1].Element("span");

                if (nameNode == null || valueNode == null)
                    throw new UnhandledException("Unexpected HTML structure");

                var name = nameNode.InnerText.Trim();

                if (!double.TryParse(valueNode.InnerText.Trim(), out double value))
                    throw new UnhandledException($"parse fail: {valueNode.InnerText.Trim()}");

                result.Add(name, value / 10);
            }
        }

        return result;
    }

    private static Dictionary<string, object?> GetTsaCities()
    {
        var web = new HtmlWeb { OverrideEncoding = Encoding.UTF8 };
        var doc = web.Load("https://www.travelsafe-abroad.com/countries/");

        var tbodies = doc.DocumentNode.SelectNodes("//table[starts-with(@class,'sortable index-table')]/tbody");

        if (tbodies == null) return [];

        var result = new Dictionary<string, object?>();

        foreach (var tbody in tbodies)
        {
            foreach (var td in tbody.SelectNodes("td"))
            {
                var a_region = td.Element("a");
                if (a_region == null) continue;
                var link = a_region.GetAttributeValue("href", "").Trim();
                var docC = web.Load(link);
                var divCities = docC.DocumentNode.SelectNodes("//div[starts-with(@class,'more-cities')]")?.FirstOrDefault();
                var ul = divCities?.Element("ul");
                var lis = ul?.Elements("li");

                var cities = new List<string>();
                foreach (var li in lis ?? [])
                {
                    var a = li.Element("a");
                    var name = a.InnerText.Trim();
                    cities.Add(name);
                }

                result[a_region.InnerText.Trim()] = cities;
            }
        }

        return result;
    }

    private static Dictionary<string, object?> GetNumbeoSafetyIndex()
    {
        //todo: 50 pages per month limit
        var result = new Dictionary<string, object?>();
        return result;

        var web = new HtmlWeb { OverrideEncoding = Encoding.UTF8 };
        var doc = web.Load("https://www.numbeo.com/crime/");
        var table = doc.DocumentNode.SelectNodes("//table[starts-with(@class,'related_links')]/tr").Single();

        var tds = table.Elements("td");

        foreach (var t in tds)
        {
            var a = t.Elements("a");

            foreach (var item in a)
            {
                var endpoint = item.GetAttributeValue("href", "");
                var name = item.InnerText.Trim();

                var docC = web.Load($"https://www.numbeo.com/crime/{endpoint}");
                var tableC = docC.DocumentNode.SelectNodes("//table[starts-with(@class,'table_indices')]")?.FirstOrDefault();
                var vl = tableC?.Elements("tr").Last().Elements("td").Last().InnerText.Trim();
                if (vl == "?") //no data available
                {
                    result.Add(name, null);
                    continue;
                }

                var success = double.TryParse(vl, out double value);
                if (!success) throw new UnhandledException($"parse fail: -{name} -{vl}");

                result.Add(name, value / 10);
            }
        }

        return result;
    }

    private static Dictionary<string, object?> GetDMDemocracyIndex(int cellValue)
    {
        var web = new HtmlWeb { OverrideEncoding = Encoding.UTF8 };
        var doc = web.Load("https://www.democracymatrix.com/ranking");

        var tbody = doc.DocumentNode.SelectNodes("//table[starts-with(@class,'ce-table ce-table-striped')]/tbody")?.FirstOrDefault();

        if (tbody == null) return [];

        var result = new Dictionary<string, object?>();

        foreach (var tr in tbody.Elements("tr"))
        {
            var tds = tr.Elements("td").ToList();

            var name = tds[1].InnerText.Trim();

            if (cellValue == 2)
            {
                var success = double.TryParse(tds[cellValue].InnerText.Trim().Replace(",", ""), out double value);
                if (!success) throw new UnhandledException($"parse fail: {tds[cellValue].InnerText.Trim()}");
                result.Add(name, value * 10);
            }
        }

        return result;
    }

    private static Dictionary<string, object?> GetEconomistDemocracyIndex(int cellValue)
    {
        var web = new HtmlWeb { OverrideEncoding = Encoding.UTF8 };
        var doc = web.Load("https://en.wikipedia.org/wiki/The_Economist_Democracy_Index");

        var tbody = doc.DocumentNode.SelectNodes("//*[@id=\"mw-content-text\"]/div[2]/table[5]/tbody")?.FirstOrDefault(); //List by country

        if (tbody == null) return [];

        var result = new Dictionary<string, object?>();

        foreach (var tr in tbody.Elements("tr").Skip(1))
        {
            var tds = tr.Elements("td").ToList();

            int cellBase = 0;
            if (tds[0].GetAttributeValue("rowspan", 0) == 0)
            {
                cellBase--;
            }

            var name = tds[cellBase + 2].SelectNodes("table/tbody/tr/td[2]/a")[0]?.InnerText.Trim();

            if (cellValue == 4)
            {
                var success = decimal.TryParse(tds[cellBase + cellValue].InnerText.Trim().Replace(",", ""), out decimal value);
                if (!success) throw new UnhandledException($"parse fail: {tds[cellBase + cellValue].InnerText.Trim()}");
                result.Add(name!, value);
            }
        }

        return result;
    }

    private static Dictionary<string, object?> GetFreedomExpressionIndex()
    {
        //download csv from website (Link: Data URL (CSV format))
        //https://ourworldindata.org/grapher/freedom-of-expression-index?tab=table&time=earliest..2024&country=&overlay=download-data
        //original: https://www.v-dem.net/data/the-v-dem-dataset/

        var path = Path.Combine(Directory.GetCurrentDirectory(), "data", $"freedom-of-expression-index.csv");

        var dic = new Dictionary<string, object?>();
        using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using var reader = ExcelReaderFactory.CreateCsvReader(stream);
            do
            {
                while (reader.Read())
                {
                    if (reader.GetString(0).ToLower() == "entity") continue; //ignores header
                    if (reader.GetString(1).Empty()) continue; //code not null

                    var year = int.Parse(reader.GetString(2).Trim());
                    var filterYear = 2025;
                    if (year < filterYear || year > filterYear) continue;

                    var index = decimal.Parse(reader.GetString(3).Trim());

                    dic.Add(reader.GetString(0), index * 10);
                }
            } while (reader.NextResult());
        }

        return dic;
    }

    private static Dictionary<string, object?> GetHappinessIndex()
    {
        //download csv from website (Link: Data URL (CSV format))
        //https://www.worldhappiness.report/data-sharing/
        //original: https://data.worldhappiness.report/table

        var path = Path.Combine(Directory.GetCurrentDirectory(), "data", $"WHR26_Data_Figure_2.1.xlsx");

        var dic = new Dictionary<string, object?>();
        using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using var reader = ExcelReaderFactory.CreateReader(stream);
            do
            {
                while (reader.Read())
                {
                    if (reader.GetValue(0) == null) break; //ignores end of file
                    if (reader.GetValue(0).ToString()?.ToLower() == "year") continue; //ignores header

                    var year = int.Parse(reader.GetDouble(0).ToString());
                    var filterYear = 2025;
                    if (year < filterYear || year > filterYear) continue;

                    var index = reader.GetDouble(3);

                    dic.Add(reader.GetString(2), index);
                }
            } while (reader.NextResult());
        }

        return dic;
    }

    private static Dictionary<string, object?> GetGDP(string metric)
    {
        string? url = $"https://www.worldometers.info/gdp/gdp-per-capita/?source=wb&year=2024&metric={metric}&region=worldwide";

        var web = new HtmlWeb { OverrideEncoding = Encoding.UTF8 };
        var doc = web.Load(url);
        //extra1: https://en.wikipedia.org/wiki/List_of_countries_by_GDP_(PPP)_per_capita
        //extra2: https://en.wikipedia.org/wiki/List_of_countries_by_GDP_(nominal)_per_capita

        var tbody = doc.DocumentNode.SelectNodes("//table[starts-with(@class,'datatable')]/tbody")?.FirstOrDefault();

        if (tbody == null) return [];

        var countries = new Dictionary<string, double>();

        foreach (var tr in tbody.Elements("tr"))
        {
            var tds = tr.Elements("td").ToList();

            var name = tds[1].InnerText.Trim();

            var success = double.TryParse(tds[2].Element("span").Elements("span").First().InnerText.Trim().Replace(",", "").Replace("$", ""), out double value);
            if (!success) throw new UnhandledException($"parse fail: {tds[2].Element("span").Elements("span").First().InnerText.Trim()}");
            countries.Add(name, value);
        }

        var (minPct, maxPct) = DataHelper.GetPercentiles(countries);

        var countryScores = countries.ToDictionary(
            kvp => kvp.Key,
            kvp => (object?)DataHelper.ConvertToScore(kvp.Value, minPct, maxPct, true)
        );

        return countryScores;
    }

    private static Dictionary<string, object?> GetEconomicFreedomIndex()
    {
        //download: https://static.heritage.org/index/data/2026/2026_indexofeconomicfreedom_data.xlsx
        var path = Path.Combine(Directory.GetCurrentDirectory(), "data", $"2026_indexofeconomicfreedom_data.xlsx");

        var dic = new Dictionary<string, object?>();
        using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using var reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            do
            {
                while (reader.Read())
                {
                    if (reader.GetString(0).Empty() || reader.GetString(0).Equals("country", StringComparison.CurrentCultureIgnoreCase)) continue; //ignores header

                    try
                    {
                        var value = reader.GetDouble(2);
                        dic.Add(reader.GetString(0).Replace("-", " "), value / 10);
                    }
                    catch (Exception)
                    {
                        dic.Add(reader.GetString(0).Replace("-", " "), null); // -> N/A
                    }
                }
            } while (reader.NextResult());
        }

        return dic;
    }

    private static Dictionary<string, object?> GetCashlessIndex()
    {
        var web = new HtmlWeb { OverrideEncoding = Encoding.UTF8 };
        var doc = web.Load("https://www.forex.se/en/travel/forex-index/cash-index/");

        var ul = doc.DocumentNode.SelectNodes("//*[@id=\"forexindexblock-3670-1-0\"]/ul");

        if (ul == null) return [];

        var result = new Dictionary<string, object?>();

        foreach (var li in ul.Elements("li"))
        {
            var name = li.SelectNodes("div[1]/div[2]/h6").FirstOrDefault()?.InnerText.Trim();
            var value = li.SelectNodes("div[2]/div/div/svg/g/text[2]").FirstOrDefault()?.InnerText.Trim().Replace("%", "");

            var success = double.TryParse(value, out double vl);
            if (!success) throw new UnhandledException($"parse fail: {value}");

            result.Add(name, (vl / 10).Invert());
        }

        return result;
    }

    private static async Task<Dictionary<string, object?>> GetCensorshipIndex()
    {
        //https://www.indexoncensorship.org/campaigns/indexindex/
        //download json (map action - view source)

        var path = Path.Combine(Directory.GetCurrentDirectory(), "data", "indexoncensorship-org.json");

        var jsonContent = await File.ReadAllTextAsync(path);
        var result = JsonSerializer.Deserialize<CensorshipData>(jsonContent);

        return result?.datasets?.data3e748aba9b5322a7e86a208c76e18dff
            .Where(w => w.Overall_Rank.HasValue)
            .ToDictionary(s => s.country_name!, s => (object?)double.Parse(s.OverallRank!.Split(":")[0]).Invert()) ?? [];
    }

    private static Dictionary<string, object?> GetFreedomScore()
    {
        var web = new HtmlWeb { OverrideEncoding = Encoding.UTF8 };
        var doc = web.Load("https://freedomhouse.org/country/scores");

        var tbody = doc.DocumentNode.SelectNodes("//*[@id=\"block-countryscorestable\"]/div/table/tbody")?.FirstOrDefault();

        if (tbody == null) return [];

        var result = new Dictionary<string, object?>();

        foreach (var tr in tbody.Elements("tr"))
        {
            var tds = tr.Elements("td").ToList();

            var a_name = tds[0].Element("a");
            var name = a_name != null ? a_name.InnerText.Trim() : tds[0].InnerText.Trim();

            var a_value = tds[1].Element("a");
            var value = a_value != null ? a_value.Element("div").Element("span").Elements("span").First().InnerText.Trim() : tds[1].Element("span").InnerText.Trim();

            if (value.Equals("not covered", StringComparison.CurrentCultureIgnoreCase))
            {
                result.Add(name, null);
            }
            else
            {
                var success = double.TryParse(value, out double vl);
                if (!success) throw new UnhandledException($"parse fail: {value}");
                result.Add(name, vl / 10);
            }
        }

        return result;
    }

    private static Dictionary<string, object?> GetYaleWaterScore()
    {
        var web = new HtmlWeb { OverrideEncoding = Encoding.UTF8 };
        var doc = web.Load("https://epi.yale.edu/measure/2024/H2O");

        var tbody = doc.DocumentNode.SelectNodes("//*[@id=\"block-system-main\"]/div/div/div/div[2]/table/tbody")?.FirstOrDefault();

        if (tbody == null) return [];

        var result = new Dictionary<string, object?>();

        foreach (var tr in tbody.Elements("tr"))
        {
            var tds = tr.Elements("td").ToList();

            var name = tds[0].Element("a").InnerText.Trim();
            var value = tds[2].InnerText.Trim();

            var success = double.TryParse(value, out double vl);
            if (!success) throw new UnhandledException($"parse fail: {value}");
            result.Add(name, vl / 10);
        }

        return result;
    }

    private static Dictionary<string, object?> GetNumbeoPollutionIndex()
    {
        //todo: 50 pages per month limit
        var result = new Dictionary<string, object?>();
        return result;

        var web = new HtmlWeb { OverrideEncoding = Encoding.UTF8 };
        var doc = web.Load("https://www.numbeo.com/pollution/");
        var table = doc.DocumentNode.SelectNodes("//table[starts-with(@class,'related_links')]/tr").Single();

        var tds = table.Elements("td");

        foreach (var t in tds)
        {
            var a = t.Elements("a");

            foreach (var item in a)
            {
                var endpoint = item.GetAttributeValue("href", "");
                var name = item.InnerText.Trim();

                var docC = web.Load($"https://www.numbeo.com/pollution/{endpoint}");
                var tableC = docC.DocumentNode.SelectNodes("//table[starts-with(@class,'table_indices')]")?.FirstOrDefault();
                var vl = tableC?.Elements("tr").ToList()[1].Elements("td").Last().InnerText.Trim();
                if (vl == "?") //no data available
                {
                    result.Add(name, null);
                    continue;
                }

                var success = float.TryParse(vl, out float value);
                if (!success) throw new UnhandledException($"parse fail: -{name} -{vl}");

                result.Add(name, value.Invert(0, 100) / 10);
            }
        }

        return result;
    }

    private static Dictionary<string, object?> GetAirQuality()
    {
        //https://www.iqair.com/us/world-most-polluted-countries
        //download the report, then convert to text, then to excel
        var path = Path.Combine(Directory.GetCurrentDirectory(), "data", $"air-quality.xlsx");

        var countries = new Dictionary<string, double>();
        using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using var reader = ExcelReaderFactory.CreateReader(stream);
            do
            {
                while (reader.Read())
                {
                    if (reader.GetValue(0) == null) break; //ignores end of file

                    var country = reader.GetString(0).Trim();
                    var value = reader.GetDouble(1);

                    countries.Add(country, value);
                }
            } while (reader.NextResult());
        }

        var (minPct, maxPct) = DataHelper.GetPercentiles(countries);

        var countryScores = countries.ToDictionary(
            kvp => kvp.Key,
            kvp => (object?)DataHelper.ConvertToScore(kvp.Value, minPct, maxPct, false)
        );

        return countryScores;
    }

    private static Dictionary<string, object?> GetHealthCareIndex()
    {
        var web = new HtmlWeb { OverrideEncoding = Encoding.UTF8 };
        var doc = web.Load("https://ceoworld.biz/2025/09/21/countries-with-the-best-health-care-systems-2025/");

        var tbody = doc.DocumentNode.SelectNodes("//*[@id=\"tablepress-670\"]/tbody")?.FirstOrDefault();

        if (tbody == null) return [];

        var result = new Dictionary<string, object?>();

        foreach (var tr in tbody.Elements("tr"))
        {
            var tds = tr.Elements("td").ToList();

            var name = tds[1].InnerText.Trim();
            var value = tds[5].InnerText.Trim();

            var success = double.TryParse(value, out double vl);
            if (!success) throw new UnhandledException($"parse fail: {value}");
            result.Add(name, vl / 10);
        }

        return result;
    }

    private static Dictionary<string, object?> GetAnnualTemperature()
    {
        var web = new HtmlWeb { OverrideEncoding = Encoding.UTF8 };
        var doc = web.Load("https://en.wikipedia.org/wiki/List_of_countries_by_average_yearly_temperature");

        var table = doc.DocumentNode.SelectNodes("//*[@id=\"mw-content-text\"]/div[2]/table")?.FirstOrDefault();

        if (table == null) return [];

        var result = new Dictionary<string, object?>();

        foreach (var tr in table.Element("tbody").Elements("tr"))
        {
            var th = tr.Elements("th").ToList();

            if (th.NotEmpty() && th[1].InnerText.Contains("Country")) continue;

            var td = tr.Elements("td").ToList();

            var aName = td[1].Element("a");
            var name = aName.InnerText.Trim();

            var aValue = td[3].InnerText;
            var text = WebUtility.HtmlDecode(aValue.Split("°C")[0]).Trim().Replace('−', '-');
            var value = double.Parse(text);

            value = value.CalculateThermalComfortScore();

            result.Add(name, value);
        }

        return result;
    }

    private static Dictionary<string, object?> GetVisionOfHumanity(string fileName)
    {
        //https://www.visionofhumanity.org/maps/global-terrorism-index/#/
        //https://www.visionofhumanity.org/maps/#/

        //peace = between 1 and 5
        var web = new HtmlWeb { OverrideEncoding = Encoding.UTF8 };
        var path = Path.Combine(Directory.GetCurrentDirectory(), "data", fileName);
        var doc = web.Load(path);

        var tbody = doc.DocumentNode.SelectNodes("//table/tbody")?.FirstOrDefault();

        if (tbody == null) return [];

        var result = new Dictionary<string, object?>();

        foreach (var tr in tbody.Elements("tr"))
        {
            var tds = tr.Elements("td").ToList();

            var name = tds[1].Element("span").InnerText.Trim();
            var value = tds[2].Element("span").Element("b").InnerText.Trim();

            var success = double.TryParse(value, out double vl);
            if (!success) throw new UnhandledException($"parse fail: {value}");

            if (fileName.Contains("terrorism"))
                result.Add(name, vl.Invert());
            else
                result.Add(name, vl.Rescale(1, 5, 0, 10).Invert());
        }

        return result;
    }

    private static void ProcessTaxis()
    {
        //https://bobthetravelnerd.com/the-best-ride-hailing-app-in-every-country-on-earth/

        var root = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));

        var pathRegion = Path.Combine(Directory.GetCurrentDirectory(), "data", "regions.json");
        var jsonRegions = File.ReadAllText(pathRegion);
        var regions = JsonSerializer.Deserialize<AllRegions>(jsonRegions);

        var pathTaxis = Path.Combine(Directory.GetCurrentDirectory(), "data", "taxis.json");
        var jsonTaxis = File.ReadAllText(pathTaxis);
        var taxis = JsonSerializer.Deserialize<AllTaxis>(jsonTaxis);

        foreach (var taxi in taxis?.Items ?? [])
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "data", "taxi", $"{taxi.name}.txt");
            if (!File.Exists(path)) continue;
            var lines = File.ReadAllLines(path);
            foreach (var line in lines ?? [])
            {
                var region = regions?.Items.FirstOrDefault(f => f.name == line.Trim());

                if (region == null)
                {
                    var pathFolder = Path.Combine(root, "Data", "taxi", "codes");
                    var pathCodes = Path.Combine(root, "data", "taxi", "codes", $"{taxi.name}.txt");

                    if (!Path.Exists(pathFolder))
                    {
                        Directory.CreateDirectory(pathFolder);
                    }

                    if (!File.Exists(pathCodes))
                    {
                        File.WriteAllText(pathCodes, string.Empty);
                    }

                    var codes = File.ReadAllLines(pathCodes);
                    var existingCodes = codes.ToHashSet(StringComparer.OrdinalIgnoreCase);

                    if (!existingCodes.Any(p => p.Contains(line))) //if code not already exists, add to file for later manual processing
                    {
                        File.AppendAllText(pathCodes, line + Environment.NewLine);
                    }
                    else //if code already exists, add corresponding region to taxi
                    {
                        var code = codes.SingleOrDefault(c => c.Contains(line));
                        taxi.regions.Add(code!.Split('=')[1]);
                    }
                }
                else
                {
                    taxi.regions.Add(region.code!);
                }
            }
        }

        File.WriteAllText(Path.Combine(root, "data", "taxis.json"), JsonSerializer.Serialize(taxis, new JsonSerializerOptions { WriteIndented = true }));
    }

    private static Dictionary<string, object?> GetIncome()
    {
        var web = new HtmlWeb { OverrideEncoding = Encoding.UTF8 };
        var doc = web.Load("https://www.worlddata.info/average-income.php?full");

        var table = doc.DocumentNode.SelectNodes("//*[@id=\"tabsort\"]")?.FirstOrDefault();

        if (table == null) return [];

        var result = new Dictionary<string, object?>();

        foreach (var tr in table.Elements("tr"))
        {
            var td = tr.Elements("td").ToList();

            if (td.Empty()) continue;
            if (td[0].InnerText.Contains("Country")) continue;

            var aName = td[0].Element("a");
            var name = aName.InnerText.Trim();

            var aValue = td[1].InnerText;
            var text = WebUtility.HtmlDecode(aValue).Trim().Replace('$', ' ').Trim();
            var value = decimal.Parse(text);

            var income = new Income() { Price = value / 12 };
            result.Add(name, income);
        }

        var countries = result!.ToDictionary(s => s.Key!, s => (double)((Income)s.Value!).Price!.Value) ?? [];

        var (minPct, maxPct) = DataHelper.GetPercentiles(countries);

        foreach (var item in result ?? [])
        {
            var income = item.Value as Income;

            income!.Score = DataHelper.ConvertToScore((double)income.Price!, minPct, maxPct, true);
        }

        return result ?? [];
    }

    private static Dictionary<string, object?> GetReligions()
    {
        //download csv from website (Link: Data URL (CSV format))
        //https://www.pewresearch.org/religion/feature/religious-composition-by-country-2010-2020/

        var path = Path.Combine(Directory.GetCurrentDirectory(), "data", $"Religious Composition 2010-2020 (percentages).csv");

        var dic = new Dictionary<string, object?>();
        using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using var reader = ExcelReaderFactory.CreateCsvReader(stream);
            do
            {
                while (reader.Read())
                {
                    if (reader.GetString(1) == "Country") continue; //ignores header

                    if (reader.GetString(1) == "Country") continue; //ignores header
                    if (reader.GetString(1) == "All World") continue; //ignores header
                    if (reader.GetString(1) == "All Asia-Pacific") continue; //ignores header
                    if (reader.GetString(1) == "All Europe") continue; //ignores header
                    if (reader.GetString(1) == "All Latin America-Caribbean") continue; //ignores header
                    if (reader.GetString(1) == "All Middle East-North Africa") continue; //ignores header
                    if (reader.GetString(1) == "All North America") continue; //ignores header
                    if (reader.GetString(1) == "All Sub-Saharan Africa") continue; //ignores header

                    var year = int.Parse(reader.GetString(2).Trim());
                    var filterYear = 2020;
                    if (year < filterYear || year > filterYear) continue;

                    var Christians = double.Parse(reader.GetString(4).Trim().Replace("%", ""));
                    var Muslims = double.Parse(reader.GetString(5).Trim().Replace("%", ""));
                    var Religiously_unaffiliated = double.Parse(reader.GetString(6).Trim().Replace("%", ""));
                    var Buddhists = double.Parse(reader.GetString(7).Trim().Replace("%", ""));
                    var Hindus = double.Parse(reader.GetString(8).Trim().Replace("%", ""));
                    var Jews = double.Parse(reader.GetString(9).Trim().Replace("%", ""));
                    var Other_religions = double.Parse(reader.GetString(10).Trim().Replace("%", ""));

                    var religions = new HashSet<ReligionData>();

                    if (Christians > 5) religions.Add(new ReligionData() { Religion = Religion.Christians, Percent = double.Round(Christians, 1) });
                    if (Muslims > 5) religions.Add(new ReligionData() { Religion = Religion.Muslims, Percent = double.Round(Muslims, 1) });
                    if (Religiously_unaffiliated > 5) religions.Add(new ReligionData() { Religion = Religion.Unaffiliated, Percent = double.Round(Religiously_unaffiliated, 1) });
                    if (Buddhists > 5) religions.Add(new ReligionData() { Religion = Religion.Buddhists, Percent = double.Round(Buddhists, 1) });
                    if (Hindus > 5) religions.Add(new ReligionData() { Religion = Religion.Hindus, Percent = double.Round(Hindus, 1) });
                    if (Jews > 5) religions.Add(new ReligionData() { Religion = Religion.Jews, Percent = double.Round(Jews, 1) });
                    if (Other_religions > 5) religions.Add(new ReligionData() { Religion = Religion.OtherReligions, Percent = double.Round(Other_religions, 1) });

                    dic.Add(reader.GetString(1), religions);
                }
            } while (reader.NextResult());
        }

        return dic;
    }

    private static Dictionary<string, object?> GetNumbeoRangePrices()
    {
        var result = new Dictionary<string, object?>();

        var web = new HtmlWeb { OverrideEncoding = Encoding.UTF8 };
        var doc = web.Load("https://www.numbeo.com/cost-of-living/");
        var table = doc.DocumentNode.SelectNodes("//table[starts-with(@class,'related_links')]/tr").Single();

        var tds = table.Elements("td");
        var td = tds.ToList()[4]; //here, website blocks requests if too many (so, run each all 5 columns - between 0 and 4)
        var a = td.Elements("a");

        foreach (var item in a)
        {
            var endpoint = item.GetAttributeValue("href", "");
            var name = item.InnerText.Trim();

            var docC = web.Load($"https://www.numbeo.com/cost-of-living/{endpoint}&displayCurrency=USD");
            var tableC = docC.DocumentNode.SelectNodes("//table[starts-with(@class,'data_wide_table')]").Single();

            var expenses = new HashSet<Expense>();

            foreach (var type in EnumHelper.GetArray<ExpenseType>())
            {
                if (type == ExpenseType.MarketWestern)
                {
                    decimal? Price = 0;

                    foreach (var market in EnumHelper.GetArray<WesternMarketExpenseType>())
                    {
                        var att = market.GetMarketCustomAttribute();

                        var reg = GetRegularValue(tableC, market.GetDescription(false));
                        var min = GetMinValue(tableC, market.GetDescription(false));
                        var max = GetMaxValue(tableC, market.GetDescription(false));

                        if (min == null)
                        {
                            Price = null;
                            continue;
                        }

                        Price += reg * (decimal)att.Proportion;
                    }

                    expenses.Add(new Expense() { Type = type, Price = Price });
                }
                else if (type == ExpenseType.MarketAsian)
                {
                    decimal? Price = 0;

                    foreach (var market in EnumHelper.GetArray<AsianMarketExpenseType>())
                    {
                        var att = market.GetMarketCustomAttribute();

                        var reg = GetRegularValue(tableC, market.GetDescription(false));
                        var min = GetMinValue(tableC, market.GetDescription(false));
                        var max = GetMaxValue(tableC, market.GetDescription(false));

                        if (min == null)
                        {
                            Price = null;
                            continue;
                        }

                        Price += reg * (decimal)att.Proportion;
                    }

                    expenses.Add(new Expense() { Type = type, Price = Price });
                }
                else
                {
                    expenses.Add(new Expense() { Type = type, Price = GetRegularValue(tableC, type.GetDescription()) });
                }
            }

            //var success = float.TryParse(vl, out float value);
            //if (!success) throw new NotificationException($"parse fail: -{name} -{vl}");

            result.Add(name, expenses);
        }

        return result;
    }

    private static async Task<Dictionary<string, object?>> GetNumbeoPriceScores(CosmosGroupRepository repo, CancellationToken cancellationToken)
    {
        var result = new Dictionary<string, object?>();

        var regions = await repo.ListAll<RegionData>(DocumentType.Country, cancellationToken);

        var exp01 = new Dictionary<string, double>();
        var exp02 = new Dictionary<string, double>();
        var exp03 = new Dictionary<string, double>();
        var exp04 = new Dictionary<string, double>();
        var exp05 = new Dictionary<string, double>();

        foreach (var item in regions)
        {
            if (item.AptCityCenter != null && item.AptCityCenter.Avg.HasValue) exp01.Add(item.Id.Split(":")[1], (double)item.AptCityCenter.Avg);
            if (item.AptOutsideCenter != null && item.AptOutsideCenter.Avg.HasValue) exp02.Add(item.Id.Split(":")[1], (double)item.AptOutsideCenter.Avg);
            if (item.Meal != null && item.Meal.Avg.HasValue) exp03.Add(item.Id.Split(":")[1], (double)item.Meal.Avg);
            if (item.MarketWestern != null && item.MarketWestern.Avg.HasValue) exp04.Add(item.Id.Split(":")[1], (double)item.MarketWestern.Avg);
            if (item.MarketAsian != null && item.MarketAsian.Avg.HasValue) exp05.Add(item.Id.Split(":")[1], (double)item.MarketAsian.Avg);
        }

        var (minExp01, maxExp01) = DataHelper.GetPercentiles(exp01);
        var (minExp02, maxExp02) = DataHelper.GetPercentiles(exp02);
        var (minExp03, maxExp03) = DataHelper.GetPercentiles(exp03);
        var (minExp04, maxExp04) = DataHelper.GetPercentiles(exp04);
        var (minExp05, maxExp05) = DataHelper.GetPercentiles(exp05);

        var Exp01Scores = exp01.ToDictionary(
            dic => dic.Key,
            dic => DataHelper.ConvertToScore(dic.Value, minExp01, maxExp01, false)
        );

        var Exp02Scores = exp02.ToDictionary(
            dic => dic.Key,
            dic => DataHelper.ConvertToScore(dic.Value, minExp02, maxExp02, false)
        );

        var Exp03Scores = exp03.ToDictionary(
            dic => dic.Key,
            dic => DataHelper.ConvertToScore(dic.Value, minExp03, maxExp03, false)
        );

        var Exp04Scores = exp04.ToDictionary(
            dic => dic.Key,
            dic => DataHelper.ConvertToScore(dic.Value, minExp04, maxExp04, false)
        );

        var Exp05Scores = exp05.ToDictionary(
            dic => dic.Key,
            dic => DataHelper.ConvertToScore(dic.Value, minExp05, maxExp05, false)
        );

        foreach (var item in regions)
        {
            var code = item.Id.Split(":")[1];

            var expenses = new HashSet<Expense>
            {
                new() { Type = ExpenseType.AptCityCenter, Score = Exp01Scores.SingleOrDefault(p => p.Key == code).Value },
                new() { Type = ExpenseType.AptOutsideCenter, Score = Exp02Scores.SingleOrDefault(p => p.Key == code).Value },
                new() { Type = ExpenseType.Meal, Score = Exp03Scores.SingleOrDefault(p => p.Key == code).Value },
                new() { Type = ExpenseType.MarketWestern, Score = Exp04Scores.SingleOrDefault(p => p.Key == code).Value },
                new() { Type = ExpenseType.MarketAsian, Score = Exp05Scores.SingleOrDefault(p => p.Key == code).Value }
            };

            result.Add(item.Id.Split(":")[1], expenses);
        }

        return result;
    }

    private static decimal? GetRegularValue(HtmlNode table, string description)
    {
        try
        {
            if (table == null) throw new ArgumentNullException(nameof(table));

            var value = table.SelectNodes($"//tr[td//text()[contains(., '{description}')]]//td[position()=2]/span").SingleOrDefault()?.InnerText.Split("&")[0];
            if (value.Empty() || value == "?") return null;
            return decimal.Parse(value ?? "0");
        }
        catch (Exception)
        {
            return null;
        }
    }

    private static decimal? GetMinValue(HtmlNode table, string description)
    {
        try
        {
            if (table == null) throw new ArgumentNullException(nameof(table));

            var value = table.SelectNodes($"//tr[td//text()[contains(., '{description}')]]//td[position()=3]/span[position()=1]").SingleOrDefault()?.InnerText;
            if (value.Empty() || value == "?") return null;
            return decimal.Parse(value ?? "0");
        }
        catch (Exception)
        {
            return null;
        }
    }

    private static decimal? GetMaxValue(HtmlNode table, string description)
    {
        try
        {
            if (table == null) throw new ArgumentNullException(nameof(table));

            var value = table.SelectNodes($"//tr[td//text()[contains(., '{description}')]]//td[position()=3]/span[position()=5]").SingleOrDefault()?.InnerText;
            if (value.Empty() || value == "?") return null;
            return decimal.Parse(value ?? "0");
        }
        catch (Exception)
        {
            return null;
        }
    }

    private static async Task<Dictionary<string, object?>> GetTourismIndex()
    {
        //converted to JSON
        //https://www3.weforum.org/docs/WEF_Travel_and_Tourism_Development_Index_2024.pdf (from score 1 to 7)

        var path = Path.Combine(Directory.GetCurrentDirectory(), "data", "ttdi.json");

        var jsonContent = await File.ReadAllTextAsync(path);
        var result = JsonSerializer.Deserialize<TourismIndexData[]>(jsonContent);

        return result?.ToDictionary(s => s.economy!, s => (object?)(s.score.Rescale(1, 7, 0, 10))) ?? [];
    }

    private static async Task<Dictionary<string, object?>> GetLanguages()
    {
        //six first languages by country
        //https://www.cia.gov/the-world-factbook/field/languages/

        //add Montenegrin to Montenegro (ISO 639-2 Code)
        //add Niuean to Niue (ISO 639-2 Code)

        var path = Path.Combine(Directory.GetCurrentDirectory(), "data", "country-languages.json");

        var jsonContent = await File.ReadAllTextAsync(path);
        var result = JsonSerializer.Deserialize<LanguageData>(jsonContent);

        return result?.countries.ToDictionary(s => s.country!, s => (object?)s.languages) ?? [];
    }

    private static Dictionary<string, object?> GetRisks()
    {
        var result = new Dictionary<string, object?>();

        var web = new HtmlWeb { OverrideEncoding = Encoding.UTF8 };
        var doc = web.Load("https://www.travelsafe-abroad.com/countries/");
        var table = doc.DocumentNode.SelectNodes("//table[starts-with(@class,'sortable index-table')]/tbody").Single();

        var trs = table.Elements("tr");

        foreach (var item in trs)
        {
            var a = item.Element("td").Element("a");
            var endpoint = a.GetAttributeValue("href", "");
            var name = a.InnerText.Trim();

            var docC = web.Load(endpoint);

            var transport = docC.DocumentNode.SelectNodes("//h3[@id='transport-and-taxis-risk']")?.SingleOrDefault();
            var pocket = docC.DocumentNode.SelectNodes("//h3[@id='pickpockets-risk']")?.SingleOrDefault();
            var disaster = docC.DocumentNode.SelectNodes("//h3[@id='natural-disasters-risk']")?.SingleOrDefault();
            var mugging = docC.DocumentNode.SelectNodes("//h3[@id='mugging-risk']")?.SingleOrDefault();
            var terrorism = docC.DocumentNode.SelectNodes("//h3[@id='terrorism-risk']")?.SingleOrDefault();
            var scam = docC.DocumentNode.SelectNodes("//h3[@id='scams-risk']")?.SingleOrDefault();
            var women = docC.DocumentNode.SelectNodes("//h3[@id='women-travelers-risk']")?.SingleOrDefault();
            var water = docC.DocumentNode.SelectNodes("//h3[@id='tap-water-risk']")?.SingleOrDefault();

            result.Add(name, new Risks()
            {
                TransportTaxis = transport?.Element("span").InnerText.ParseToEnum<Level>(),
                Pickpockets = pocket?.Element("span").InnerText.ParseToEnum<Level>(),
                NaturalDisasters = disaster?.Element("span").InnerText.ParseToEnum<Level>(),
                Mugging = mugging?.Element("span").InnerText.ParseToEnum<Level>(),
                Terrorism = terrorism?.Element("span").InnerText.ParseToEnum<Level>(),
                Scams = scam?.Element("span").InnerText.ParseToEnum<Level>(),
                WomenTravelers = women?.Element("span").InnerText.ParseToEnum<Level>(),
                TapWater = water?.Element("span").InnerText.ParseToEnum<Level>()
            });
        }

        return result;
    }

    private static Dictionary<string, object?> GetElectricity()
    {
        var result = new Dictionary<string, object?>();

        var web = new HtmlWeb { OverrideEncoding = Encoding.UTF8 };
        var doc = web.Load("https://www.worldstandards.eu/electricity/plug-voltage-by-country/");
        var table = doc.DocumentNode.SelectNodes("//*[@id=\"tablepress-1\"]/tbody").Single();

        var trs = table.Elements("tr");

        foreach (var tr in trs)
        {
            var tds = tr.Elements("td").ToList();

            var a = tds[0].Element("a");
            var name = a.InnerText.Trim();

            var plugsText = tds[1].InnerText.Split("(")[0];
            var plugs = plugsText.Split("/").Select(s => s.Trim());

            var match = Regex.Matches(tds[2].InnerText.Trim(), @"\d* V");
            var voltages = match.Select(m => m.Value);

            result.Add(name, new ElectricityData()
            {
                Plugs = plugs.ToHashSet(),
                Voltages = voltages.ToHashSet(),
            });
        }

        return result;
    }

    private static Dictionary<string, object?> GetTipping()
    {
        //open embbed iframe and scrap table
        //https://www.visualcapitalist.com/cp/mapped-how-much-should-you-tip-in-each-country/

        var path = Path.Combine(Directory.GetCurrentDirectory(), "data", $"tipping.xlsx");

        var dic = new Dictionary<string, object?>();
        using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using var reader = ExcelReaderFactory.CreateReader(stream);
            do
            {
                while (reader.Read())
                {
                    if (reader.GetValue(0) == null) break; //ignores end of file
                    if (reader.GetValue(0).ToString()?.ToLower() == "country") continue; //ignores header
                    var country = reader.GetString(0).Trim();
                    string? restaurant;
                    try
                    {
                        restaurant = reader.GetDouble(1).ToString("0%");
                    }
                    catch
                    {
                        restaurant = reader.GetString(1);
                    }
                    string? hotel;
                    try
                    {
                        hotel = reader.GetDouble(2).ToString("C0");
                    }
                    catch
                    {
                        hotel = reader.GetString(2);
                    }
                    string? driver;
                    try
                    {
                        driver = reader.GetDouble(3).ToString("0%");
                    }
                    catch
                    {
                        driver = reader.GetString(3);
                    }

                    dic.Add(country, new Tipping()
                    {
                        Restaurant = restaurant,
                        Hotel = hotel,
                        Driver = driver,
                    });
                }
            } while (reader.NextResult());
        }

        return dic;
    }

    private static Dictionary<string, object?> GetBroadbandSpeed()
    {
        //download resources
        //https://bestbroadbanddeals.co.uk/broadband/speed/worldwide-speed-league/

        var path = Path.Combine(Directory.GetCurrentDirectory(), "data", $"worldwide_speed_league_data.xlsx");

        var dic = new Dictionary<string, object?>();
        using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using var reader = ExcelReaderFactory.CreateReader(stream);
            do
            {
                while (reader.Read())
                {
                    if (reader.GetValue(0) == null) break; //ignores end of file
                    if (reader.GetValue(0).ToString()?.ToLower() == "position") continue; //ignores header

                    var index = reader.GetDouble(4);

                    dic.Add(reader.GetString(1), index);
                }
            } while (reader.NextResult());
        }

        return dic;
    }

    private static Dictionary<string, object?> GetTax()
    {
        var result = new Dictionary<string, object?>();

        var web = new HtmlWeb { OverrideEncoding = Encoding.UTF8 };
        var doc = web.Load("https://en.wikipedia.org/wiki/List_of_countries_by_tax_rates");
        var table = doc.DocumentNode.SelectNodes("//*[@id=\"mw-content-text\"]/div[2]/table[2]/tbody").Single();

        var trs = table.Elements("tr");

        foreach (var item in trs)
        {
            var header = item.Elements("th")?.FirstOrDefault()?.InnerText.Trim();
            if (header == "Tax jurisdiction") continue;
            if (header == "Lowest") continue;

            var a = item.Element("td").Element("span").Element("a");
            a ??= item.Element("td").Element("a");
            var name = a.InnerText.Trim();

            var corporate = WebUtility.HtmlDecode(item.SelectNodes("td[2]")?.SingleOrDefault()?.InnerText).RemoveDirtyCharacters();
            var incomeLowest = WebUtility.HtmlDecode(item.SelectNodes("td[3]")?.SingleOrDefault()?.InnerText).RemoveDirtyCharacters();
            var incomeHighest = WebUtility.HtmlDecode(item.SelectNodes("td[4]")?.SingleOrDefault()?.InnerText).RemoveDirtyCharacters();
            var capitalGains = WebUtility.HtmlDecode(item.SelectNodes("td[5]")?.SingleOrDefault()?.InnerText).RemoveDirtyCharacters();
            var wealth = WebUtility.HtmlDecode(item.SelectNodes("td[6]")?.SingleOrDefault()?.InnerText).RemoveDirtyCharacters();
            var property = WebUtility.HtmlDecode(item.SelectNodes("td[7]")?.SingleOrDefault()?.InnerText).RemoveDirtyCharacters();
            var inheritanceEstate = WebUtility.HtmlDecode(item.SelectNodes("td[8]")?.SingleOrDefault()?.InnerText).RemoveDirtyCharacters();
            var vATGSTSales = WebUtility.HtmlDecode(item.SelectNodes("td[9]")?.SingleOrDefault()?.InnerText).RemoveDirtyCharacters();

            result.Add(name, new Taxes()
            {
                Corporate = corporate,
                IncomeLowest = incomeLowest,
                IncomeHighest = incomeHighest,
                CapitalGains = capitalGains,
                Wealth = wealth,
                Property = property,
                InheritanceEstate = inheritanceEstate,
                VATGSTSales = vATGSTSales
            });
        }

        return result;
    }

    private static Dictionary<string, object?> GetEmergencyNumbers()
    {
        var result = new Dictionary<string, object?>();

        var web = new HtmlWeb { OverrideEncoding = Encoding.UTF8 };
        var doc = web.Load("https://en.wikipedia.org/wiki/List_of_emergency_telephone_numbers");
        var tables = doc.DocumentNode.SelectNodes("//*[@id=\"mw-content-text\"]/div[2]/table");

        foreach (var table in tables)
        {
            var trs = table.Element("tbody").Elements("tr");

            foreach (var item in trs)
            {
                var header = item.Elements("th")?.FirstOrDefault()?.InnerText.Trim();
                if (header == "Country") continue;

                var a = item.Element("td").Element("span").Element("a");
                a ??= item.Element("td").Element("a");
                var name = a.InnerText.Trim();

                var expandedColumns = ProcessMergeCells(item.Elements("td").Skip(1));

                var police = expandedColumns.ElementAtOrDefault(0);
                var ambulance = expandedColumns.ElementAtOrDefault(1);
                var fire = expandedColumns.ElementAtOrDefault(2);
                var others = expandedColumns.ElementAtOrDefault(3);

                if (name == "Turkey" && result.Any(a => a.Key == "Turkey")) //for some reason, is duplicated
                {
                    result.Remove("Turkey");
                }

                result.Add(name, new EmergencyNumbers()
                {
                    Police = police,
                    Ambulance = ambulance,
                    Fire = fire,
                    Others = others,
                });
            }
        }

        return result;
    }

    private static Dictionary<string, object?> GetCurrencies()
    {
        var web = new HtmlWeb { OverrideEncoding = Encoding.UTF8 };
        var doc = web.Load("https://en.wikipedia.org/wiki/List_of_circulating_currencies");

        var table = doc.DocumentNode.SelectNodes("//*[@id=\"mw-content-text\"]/div[2]/table[2]")?.FirstOrDefault();

        if (table == null) return [];

        var result = new Dictionary<string, object?>();
        string? previousName = null;
        int remainingRowspan = 0;
        var index = 3;

        foreach (var tr in table.Element("tbody").Elements("tr"))
        {
            if (tr.Elements("th").FirstOrDefault()?.InnerText.Contains("State") ?? false) continue;

            var tds = tr.Elements("td").ToList();

            var tdName = tds[0];

            var rowspan = tdName.GetAttributeValue("rowspan", "1");
            if (!int.TryParse(rowspan, out var span)) span = 1;

            var name = tdName.Element("i")?.Element("a")?.InnerText.Trim();
            name ??= tdName.Element("a")?.InnerText.Trim();

            if (remainingRowspan > 0) //get previous name for remaining rowspan
            {
                name = previousName;
                remainingRowspan--;
                index = 2;
            }

            if (span > 1) //store name and count for next iterations
            {
                previousName = name;
                remainingRowspan = span - 1;
            }

            var value = tds[index].InnerText.Trim();
            index = 3;

            if (value == "(none)") continue;
            HashSet<Currency> values = [];
            if (result.TryGetValue(name!, out object? value1)) //duplicated
            {
                values = (HashSet<Currency>)value1!;
                values.Add(value.ParseToEnum<Currency>());
                result[name!] = values;
            }
            else
            {
                values.Add(value.ParseToEnum<Currency>());
                result.Add(name!, values);
            }
        }

        return result;
    }

    private static async Task<Dictionary<string, object?>> GetTravelRequirements(IHttpClientFactory factory, CosmosGroupRepository repo, string? key, CancellationToken cancellationToken)
    {
        var regions = await repo.ListAll<RegionData>(DocumentType.Country, cancellationToken);
        Dictionary<string, object?> result = [];

        foreach (var item in regions)
        {
            var client = factory.CreateClient("generic");

            var region = item.Id.Split(":")[1];
            var loc = region == "BR" ? "USA" : "BRA";
            var date = DateTime.Now.ToString("yyyy-MM-dd");

            var obj = @"{""data"":{""type"":""TRIP"",""attributes"":{""traveller"":{""passports"":[""XLOCX""],""vaccinations"":[{""type"":""COVID_19"",""status"":""FULLY_VACCINATED""}],""travelPurposes"":[""TOURISM""]},""locale"":""en-US"",""travelNodes"":[{""type"":""ORIGIN"",""departure"":{""date"":""XDATEX"",""time"":""00:00""},""locationCode"":""XLOCX""},{""type"":""DESTINATION"",""arrival"":{""date"":""XDATEX"",""time"":""00:00""},""locationCode"":""XREGIONX""}],""currency"":""USD""}}}";
            obj = obj.Replace("XDATEX", date);
            obj = obj.Replace("XLOCX", loc);
            obj = obj.Replace("XREGIONX", region);

            try
            {
                //Increase Web Timeout = TimeSpan.FromSeconds(200)
                var data = await client.PostApiData<SherpaModel>($"https://requirements-api.joinsherpa.com/v3/trips?include=procedure&key={key}&affiliateId=sherpa", obj, key, cancellationToken);

                if (data == null) continue;

                var name = data.data.attributes.travelNodes.FirstOrDefault(p => p.type == "DESTINATION")!.locationName;

                var req = new TravelRequirements()
                {
                    Accommodation = HasRequirement(data.included, "proof of accommodation", "accommodation booking"),
                    HealthInsurance = HasRequirement(data.included, "travel health insurance", "travel insurance", "health insurance"),
                    ReturnTicket = HasRequirement(data.included, "proof of return or onward"),
                    YellowFever = HasRequirement(data.included, "proof of Yellow Fever vaccination"),
                    MinimumFunds = HasRequirement(data.included, "proof of minimum funds"),
                };

                //High-Cost Countries Where Insurance is Essential
                if (new[] { "AU", "CA", "GB", "JP", "NZ", "SG", "US" }.Contains(region))
                {
                    req.HealthInsurance = true;
                }

                result.Add(name, req);
            }
            catch (Exception ex)
            {
                //ignore errors, as some countries may not have data
            }
        }

        return result;
    }

    #region Utils

    private static async Task<Dictionary<string, object?>> GetCities(CosmosGroupRepository repo, CancellationToken cancellationToken)
    {
        var suggestion1 = await repo.Get<Suggestion>(DocumentType.Suggestion, "global-cities-alpha", cancellationToken);
        var suggestion2 = await repo.Get<Suggestion>(DocumentType.Suggestion, "global-cities-beta", cancellationToken);
        var suggestion3 = await repo.Get<Suggestion>(DocumentType.Suggestion, "global-cities-gamma", cancellationToken);
        var suggestion4 = await repo.Get<Suggestion>(DocumentType.Suggestion, "global-cities-sufficiency", cancellationToken);

        Dictionary<string, object?> result = [];

        foreach (var region in suggestion1!.Regions.Concat(suggestion2!.Regions).Concat(suggestion3!.Regions).Concat(suggestion4!.Regions))
        {
            var country = result!.GetValueOrDefault(region.Code);

            if (country == null)
            {
                result.Add(region.Code!, new List<string> { region.Title! });
            }
            else
            {
                var list = (List<string>)country;
                list.Add(region.Title!);
                result[region.Code!] = list.ToList();
            }
        }

        return result;
    }

    private static string? RemoveDirtyCharacters(this string? text)
    {
        if (text == null) return null;

        text = Regex.Replace(text, @"\[[^\]]*\]", " "); // remove [ ... ]
        text = Regex.Replace(text, @"\)", ") "); // create space
        text = Regex.Replace(text, @"\s+", " "); // collapse all whitespace

        return text.Trim();
    }

    private static List<string?> ProcessMergeCells(IEnumerable<HtmlNode> items)
    {
        var expandedColumns = new List<string?>();

        foreach (var td in items)
        {
            var text = WebUtility.HtmlDecode(td.InnerText).RemoveDirtyCharacters()?.Trim();

            var colspanAttr = td.GetAttributeValue("colspan", "1");

            if (!int.TryParse(colspanAttr, out var colspan))
                colspan = 1;

            for (int i = 0; i < colspan; i++)
            {
                expandedColumns.Add(text);
            }
        }

        return expandedColumns;
    }

    private static bool HasRequirement(IEnumerable<Included> items, params string[] titles)
    {
        return items.Any(p =>
            p.attributes.title.NotEmpty() &&
            titles.Any(title => p.attributes.title.Contains(title, StringComparison.OrdinalIgnoreCase))
        );
    }

    #endregion Utils
}