using ExcelDataReader;
using HtmlAgilityPack;
using NS.API.Core.Models;
using NS.Shared.Models.Country;
using System.Text;
using System.Text.Json;

namespace NS.API.Core.Scraping;

public static class ScrapingBasic
{
    public static async Task<Dictionary<string, object?>> GetData(Field field, IHttpClientFactory factory, Configurations config, CosmosGroupRepository repo, CancellationToken cancellationToken)
    {
        return field switch
        {
            //Society and Government (100)
            Field.CorruptionScore => await GetCorruptionScore(),
            Field.HDI => GetHDI(),
            Field.DMDemocracyIndex => GetDMDemocracyIndex(2),
            Field.DMClassification => GetDMDemocracyIndex(3),
            Field.EconomistDemocracyIndex => GetEconomistDemocracyIndex(4),
            Field.EconomistRegimeType => GetEconomistDemocracyIndex(3),
            Field.FreedomExpressionIndex => GetFreedomExpressionIndex(),
            Field.FreedomScore => GetFreedomScore(),
            Field.CensorshipIndex => await GetCensorshipIndex(),
            Field.HappinessIndex => GetHappinessIndex(),
            //Economy (200)
            Field.OECD => GetOECD(),
            Field.GDP_PPP => GetGDP("ppp"),
            Field.GDP_Nominal => GetGDP("nominal"),
            Field.EconomicFreedomIndex => GetEconomicFreedomIndex(),
            //Security and Peace (300)
            Field.TsaSafetyIndex => GetTsaSafetyIndex(),
            Field.NumbeoSafetyIndex => GetNumbeoSafetyIndex(),
            Field.GlobalTerrorismIndex => GetVisionOfHumanity("visionofhumanity-terrorism.html"),
            Field.GlobalPeaceIndex => GetVisionOfHumanity("visionofhumanity-peace.html"),
            //Environment and Health (400)
            Field.YaleWaterScore => GetYaleWaterScore(),
            Field.NumbeoPollutionIndex => GetNumbeoPollutionIndex(),
            //Mobility and Tourism (500)
            Field.VisaFree => await GetVisaFree(factory),
            Field.TourismIndex => await GetTourismIndex(),
            //Guide (1000)
            Field.TaxiApps => GetTaxiApps(),
            Field.Languages => await GetLanguages(),
            Field.Risks => GetRisks(),
            Field.Conflicts => await GetConflicts(factory, config.Parsehub?.Key),
            //Cost Of Living (1100)
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

        return result?.Where(p => p.year == DateTime.Now.Year - 1).ToDictionary(s => s.country!, s => (object?)(s.score * 10)) ?? [];
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
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                do
                {
                    while (reader.Read())
                    {
                        if (reader.IsDBNull(0)) continue; //first column has to be not null
                        if (!short.TryParse(reader.GetValue(0)?.ToString(), out short _)) continue; //first column has to be a valid number

                        dic.Add(reader.GetString(1), reader.GetDouble(2) * 1000);
                    }
                } while (reader.NextResult());
            }
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

        var tbody = doc.DocumentNode.SelectNodes("//table[starts-with(@class,'sortable index-table')]/tbody")?.FirstOrDefault();

        if (tbody == null) return [];

        var result = new Dictionary<string, object?>();

        foreach (var tr in tbody.Elements("tr"))
        {
            var tds = tr.Elements("td").ToList();

            var name = tds[0].Element("a").InnerText.Trim();
            var success = int.TryParse(tds[1].Element("span").InnerText.Trim(), out int value);

            if (!success) throw new UnhandledException($"parse fail: {tds[1].Element("span").InnerText.Trim()}");

            result.Add(name, value * 10);
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
        var result = new Dictionary<string, object?>();

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

                var success = decimal.TryParse(vl, out decimal value);
                if (!success) throw new UnhandledException($"parse fail: -{name} -{vl}");

                result.Add(name, value * 10);
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
                result.Add(name, value * 1000);
            }
            else if (cellValue == 3)
            {
                var parse = Enum.Parse<DMClassification>(tds[cellValue].InnerText.Trim().Replace(" ", ""));
                result.Add(name, parse);
            }
        }

        return result;
    }

    private static Dictionary<string, object?> GetEconomistDemocracyIndex(int cellValue)
    {
        var web = new HtmlWeb { OverrideEncoding = Encoding.UTF8 };
        var doc = web.Load("https://en.wikipedia.org/wiki/The_Economist_Democracy_Index");

        var tbody = doc.DocumentNode.SelectNodes("//*[@id=\"mw-content-text\"]/div[2]/div[9]/table/tbody")?.FirstOrDefault();

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

            var name = tds[cellBase + 2].Element("a")?.InnerText.Trim();
            if (name.Empty())
            {
                name = tds[cellBase + 2].Element("span").Element("a")?.InnerText.Trim();
            }

            if (cellValue == 4)
            {
                var success = decimal.TryParse(tds[cellBase + cellValue].InnerText.Trim().Replace(",", ""), out decimal value);
                if (!success) throw new UnhandledException($"parse fail: {tds[cellBase + cellValue].InnerText.Trim()}");
                result.Add(name!, value * 100);
            }
            else if (cellValue == 3)
            {
                var parse = Enum.Parse<EconomistRegimeType>(tds[cellBase + cellValue].InnerText.Trim().Replace(" ", ""), true);
                result.Add(name!, parse);
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
            using (var reader = ExcelReaderFactory.CreateCsvReader(stream))
            {
                do
                {
                    while (reader.Read())
                    {
                        if (reader.GetString(0) == "Entity") continue; //ignores header
                        if (reader.GetString(1).Empty()) continue; //code not null

                        var year = int.Parse(reader.GetString(2).Trim());
                        if (year < 2024 || year > 2024) continue; //only year 2024

                        var index = decimal.Parse(reader.GetString(3).Trim());

                        dic.Add(reader.GetString(0), index * 1000);
                    }
                } while (reader.NextResult());
            }
        }

        return dic;
    }

    private static Dictionary<string, object?> GetHappinessIndex()
    {
        //download csv from website (Link: Data URL (CSV format))
        //https://www.worldhappiness.report/data-sharing/
        //original: https://data.worldhappiness.report/table

        var path = Path.Combine(Directory.GetCurrentDirectory(), "data", $"WHR25_Data_Figure_2.1.xlsx");

        var dic = new Dictionary<string, object?>();
        using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                do
                {
                    while (reader.Read())
                    {
                        if (reader.GetValue(0) == null) break; //ignores end of file
                        if (reader.GetValue(0).ToString() == "Year") continue; //ignores header

                        var year = reader.GetDouble(0);
                        if (year is < 2024 or > 2024) continue; //only year 2024

                        var index = reader.GetDouble(3);

                        dic.Add(reader.GetString(2), index * 100);
                    }
                } while (reader.NextResult());
            }
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

        var (minPct, maxPct) = GetPercentiles(countries);

        var countryScores = countries.ToDictionary(
            kvp => kvp.Key,
            kvp => ConvertToScore(kvp.Value, minPct, maxPct, true)
        );

        return countryScores;
    }

    private static Dictionary<string, object?> GetEconomicFreedomIndex()
    {
        //download: https://www.heritage.org/index/assets/data/csv/ef-country-scores.csv
        var path = Path.Combine(Directory.GetCurrentDirectory(), "data", $"ef-country-scores.csv");

        var dic = new Dictionary<string, object?>();
        using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using (var reader = ExcelReaderFactory.CreateCsvReader(stream))
            {
                do
                {
                    while (reader.Read())
                    {
                        if (reader.GetString(0) == "name_web") continue; //ignores header

                        var year = int.Parse(reader.GetString(1).Trim());
                        if (year < 2025 || year > 2025) continue; //only year 2025

                        var value = reader.GetString(2).Trim();

                        if (value == "N/A")
                        {
                            dic.Add(reader.GetString(0).Replace("-", " "), null);
                        }
                        else
                        {
                            dic.Add(reader.GetString(0).Replace("-", " "), decimal.Parse(value) * 10);
                        }
                    }
                } while (reader.NextResult());
            }
        }

        return dic;
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
            .ToDictionary(s => s.country_name!, s => (object?)(int.Parse(s.OverallRank!.Split(":")[0]).Invert() * 100)) ?? [];
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

            if (value == "Not covered")
            {
                result.Add(name, null);
            }
            else
            {
                var success = int.TryParse(value, out int vl);
                if (!success) throw new UnhandledException($"parse fail: {value}");
                result.Add(name, vl * 10);
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

            var success = float.TryParse(value, out float vl);
            if (!success) throw new UnhandledException($"parse fail: {value}");
            result.Add(name, vl * 10);
        }

        return result;
    }

    private static Dictionary<string, object?> GetNumbeoPollutionIndex()
    {
        var result = new Dictionary<string, object?>();

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

                result.Add(name, value.Invert(0, 100) * 10);
            }
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
                result.Add(name, vl.Invert() * 100);
            else
                result.Add(name, vl.Rescale(1, 5, 0, 10).Invert() * 100);
        }

        return result;
    }

    

    private static Dictionary<string, object?> GetTaxiApps()
    {
        var result = new Dictionary<string, object?>();

        ////uber
        //var path = Path.Combine(Directory.GetCurrentDirectory(), "data", "uber.txt");
        //var lines = File.ReadAllLines(path);

        //foreach (var line in lines)
        //{
        //    result.Add(line, TaxiApp.Uber);
        //}

        ////bolt
        //var web = new HtmlWeb { OverrideEncoding = Encoding.UTF8 };
        //var path = Path.Combine(Directory.GetCurrentDirectory(), "data", "bolt.html");
        //var docBolt = web.Load(path);
        //var tableBolt = docBolt.DocumentNode.SelectNodes("//section").Single();
        //var tdsBolt = tableBolt.Elements("div"); //continents

        //foreach (var t in tdsBolt)
        //{
        //    foreach (var div in t.Element("div").Elements("h4")) //countries
        //    {
        //        var name = div.InnerText.Trim();

        //        result.Add(name, TaxiApp.Bolt);
        //    }
        //}

        ////indrive
        //var path = Path.Combine(Directory.GetCurrentDirectory(), "data", "indrive.txt");
        //var lines = File.ReadAllLines(path);

        //foreach (var line in lines)
        //{
        //    result.Add(line, TaxiApp.Indrive);
        //}

        ////Maxim
        //var path = Path.Combine(Directory.GetCurrentDirectory(), "data", "taximaxim.txt");
        //var lines = File.ReadAllLines(path);

        //foreach (var line in lines)
        //{
        //    result.Add(line, TaxiApp.Maxim);
        //}

        ////DiDi
        //var path = Path.Combine(Directory.GetCurrentDirectory(), "data", "didi.txt");
        //var lines = File.ReadAllLines(path);

        //foreach (var line in lines)
        //{
        //    result.Add(line, TaxiApp.DiDi);
        //}

        ////Careem
        //var path = Path.Combine(Directory.GetCurrentDirectory(), "data", "careem.txt");
        //var lines = File.ReadAllLines(path);

        //foreach (var line in lines)
        //{
        //    result.Add(line, TaxiApp.Careem);
        //}

        ////Freenow
        //var path = Path.Combine(Directory.GetCurrentDirectory(), "data", "freenow.txt");
        //var lines = File.ReadAllLines(path);

        //foreach (var line in lines)
        //{
        //    result.Add(line, TaxiApp.Freenow);
        //}

        ////Grab
        //var path = Path.Combine(Directory.GetCurrentDirectory(), "data", "grab.txt");
        //var lines = File.ReadAllLines(path);

        //foreach (var line in lines)
        //{
        //    result.Add(line, TaxiApp.Grab);
        //}

        ////Gozem
        //var path = Path.Combine(Directory.GetCurrentDirectory(), "data", "gozem.txt");
        //var lines = File.ReadAllLines(path);

        //foreach (var line in lines)
        //{
        //    result.Add(line, TaxiApp.Gozem);
        //}

        //Yassir
        var path = Path.Combine(Directory.GetCurrentDirectory(), "data", "yassir.txt");
        var lines = File.ReadAllLines(path);

        foreach (var line in lines)
        {
            result.Add(line, TaxiApp.Yassir);
        }

        return result;
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
                    decimal? MinPrice = 0;
                    decimal? MaxPrice = 0;

                    foreach (var market in EnumHelper.GetArray<WesternMarketExpenseType>())
                    {
                        var att = market.GetMarketCustomAttribute();

                        var reg = GetRegularValue(tableC, market.GetDescription());
                        var min = GetMinValue(tableC, market.GetDescription());
                        var max = GetMaxValue(tableC, market.GetDescription());

                        if (min == null)
                        {
                            Price = null;
                            MinPrice = null;
                            MaxPrice = null;
                            continue;
                        }

                        Price += reg * (decimal)att.Proportion;
                        MinPrice += min * (decimal)att.Proportion;
                        MaxPrice += max * (decimal)att.Proportion;
                    }

                    expenses.Add(new Expense() { Type = type, Price = Price, MinPrice = MinPrice, MaxPrice = MaxPrice });
                }
                else if (type == ExpenseType.MarketAsian)
                {
                    decimal? Price = 0;
                    decimal? MinPrice = 0;
                    decimal? MaxPrice = 0;

                    foreach (var market in EnumHelper.GetArray<AsianMarketExpenseType>())
                    {
                        var att = market.GetMarketCustomAttribute();

                        var reg = GetRegularValue(tableC, market.GetDescription());
                        var min = GetMinValue(tableC, market.GetDescription());
                        var max = GetMaxValue(tableC, market.GetDescription());

                        if (min == null)
                        {
                            Price = null;
                            MinPrice = null;
                            MaxPrice = null;
                            continue;
                        }

                        Price += reg * (decimal)att.Proportion;
                        MinPrice += min * (decimal)att.Proportion;
                        MaxPrice += max * (decimal)att.Proportion;
                    }

                    expenses.Add(new Expense() { Type = type, Price = Price, MinPrice = MinPrice, MaxPrice = MaxPrice });
                }
                else
                {
                    expenses.Add(new Expense() { Type = type, Price = GetRegularValue(tableC, type.GetDescription()), MinPrice = GetMinValue(tableC, type.GetDescription()), MaxPrice = GetMaxValue(tableC, type.GetDescription()) });
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

        var (minExp01, maxExp01) = GetPercentiles(exp01);
        var (minExp02, maxExp02) = GetPercentiles(exp02);
        var (minExp03, maxExp03) = GetPercentiles(exp03);
        var (minExp04, maxExp04) = GetPercentiles(exp04);
        var (minExp05, maxExp05) = GetPercentiles(exp05);

        var Exp01Scores = exp01.ToDictionary(
            dic => dic.Key,
            dic => ConvertToScore(dic.Value, minExp01, maxExp01, false)
        );

        var Exp02Scores = exp02.ToDictionary(
            dic => dic.Key,
            dic => ConvertToScore(dic.Value, minExp02, maxExp02, false)
        );

        var Exp03Scores = exp03.ToDictionary(
            dic => dic.Key,
            dic => ConvertToScore(dic.Value, minExp03, maxExp03, false)
        );

        var Exp04Scores = exp04.ToDictionary(
            dic => dic.Key,
            dic => ConvertToScore(dic.Value, minExp04, maxExp04, false)
        );

        var Exp05Scores = exp05.ToDictionary(
            dic => dic.Key,
            dic => ConvertToScore(dic.Value, minExp05, maxExp05, false)
        );

        foreach (var item in regions)
        {
            var code = item.Id.Split(":")[1];

            var expenses = new HashSet<Expense>
            {
                new() { Type = ExpenseType.AptCityCenter, Score = (double?)Exp01Scores.SingleOrDefault(p => p.Key == code).Value },
                new() { Type = ExpenseType.AptOutsideCenter, Score = (double?)Exp02Scores.SingleOrDefault(p => p.Key == code).Value },
                new() { Type = ExpenseType.Meal, Score = (double?)Exp03Scores.SingleOrDefault(p => p.Key == code).Value },
                new() { Type = ExpenseType.MarketWestern, Score = (double?)Exp04Scores.SingleOrDefault(p => p.Key == code).Value },
                new() { Type = ExpenseType.MarketAsian, Score = (double?)Exp05Scores.SingleOrDefault(p => p.Key == code).Value }
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

        return result?.ToDictionary(s => s.economy!, s => (object?)(s.score.Rescale(1, 7, 0, 10) * 100)) ?? [];
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
                TransportTaxis = transport == null ? null : Enum.Parse<Level>(transport.Element("span").InnerText, true),
                Pickpockets = pocket == null ? null : Enum.Parse<Level>(pocket.Element("span").InnerText, true),
                NaturalDisasters = disaster == null ? null : Enum.Parse<Level>(disaster.Element("span").InnerText, true),
                Mugging = mugging == null ? null : Enum.Parse<Level>(mugging.Element("span").InnerText, true),
                Terrorism = terrorism == null ? null : Enum.Parse<Level>(terrorism.Element("span").InnerText, true),
                Scams = scam == null ? null : Enum.Parse<Level>(scam.Element("span").InnerText, true),
                WomenTravelers = women == null ? null : Enum.Parse<Level>(women.Element("span").InnerText, true),
                TapWater = water == null ? null : Enum.Parse<Level>(water.Element("span").InnerText, true)
            });
        }

        return result;
    }

    private static async Task<Dictionary<string, object?>> GetConflicts(IHttpClientFactory factory, string? key)
    {
        var client = factory.CreateClient("parsehub-gzip");

        var result = await client.GetApiData<ConflictData>($"https://parsehub.com/api/v2/projects/t7aAtOT6TZcY/last_ready_run/data?api_key={key}", CancellationToken.None);

        return result?.rows.ToDictionary(s => s.country!, s => (object?)$"{s.level}|{s.forecast}") ?? [];
    }

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

    public static (double minPercentile, double maxPercentile) GetPercentiles(Dictionary<string, double> values)
    {
        var sortedValues = values.Values.OrderBy(v => v).ToList();
        int n = sortedValues.Count;

        int idx5 = (int)Math.Floor(0.05 * (n - 1));
        int idx95 = (int)Math.Floor(0.95 * (n - 1));

        double minPercentile = sortedValues[idx5];
        double maxPercentile = sortedValues[idx95];

        return (minPercentile, maxPercentile);
    }

    public static object? ConvertToScore(double value, double minPercentile, double maxPercentile, bool higherIsBetter)
    {
        if (higherIsBetter)
        {
            if (value <= minPercentile) return 0.0;
            if (value >= maxPercentile) return 10.0;

            double normalized = (value - minPercentile) / (maxPercentile - minPercentile);
            return Math.Round(normalized * 10, 2);
        }
        else
        {
            if (value <= minPercentile) return 10.0;
            if (value >= maxPercentile) return 0.0;

            double normalized = (maxPercentile - value) / (maxPercentile - minPercentile);
            return Math.Round(normalized * 10, 2);
        }
    }
}