using ExcelDataReader;
using HtmlAgilityPack;
using NS.API.Core.Models;
using System.Text;
using System.Text.Json;

namespace NS.API.Core.Scraping;

public static class ScrapingBasic
{
    public static async Task<Dictionary<string, object?>> GetData(Field field, HttpClient http)
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
            Field.GDP_PPP => GetGDP(2),
            Field.GDP_Nominal => GetGDP(3),
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
            Field.VisaFree => await GetVisaFree(http),
            Field.InternationalArrivals => GetInternationalArrivals(),
            //Demographics and Territory (600)
            Field.UnMember => GetUnMember(),
            Field.Population => GetPopulation(),
            Field.Area => GetArea(),
            _ => [],
        };
    }

    private static Dictionary<string, object?> GetPopulation()
    {
        var web = new HtmlWeb { OverrideEncoding = Encoding.UTF8 };
        var doc = web.Load("https://www.worldometers.info/world-population/population-by-country/");

        var ul = doc.DocumentNode.SelectNodes("//table[starts-with(@class,'datatable')]/tbody")?.FirstOrDefault();

        if (ul == null) return [];

        var result = new Dictionary<string, object?>();

        foreach (var tr in ul.Elements("tr"))
        {
            var tds = tr.Elements("td").ToList();

            var name = tds[1].Element("a").InnerText.Trim();
            var success = int.TryParse(tds[2].InnerText.Trim().Replace(",", ""), out int value);

            if (!success) throw new NotificationException($"parse fail: {tds[2].InnerText.Trim()}");

            result.Add(name, value);
        }

        return result;
    }

    private static Dictionary<string, object?> GetUnMember()
    {
        var web = new HtmlWeb { OverrideEncoding = Encoding.UTF8 };
        var doc = web.Load("https://www.un.org/en/about-us/member-states");

        var div = doc.DocumentNode.SelectNodes("//div[starts-with(@class,'row mb-2 flags050')]")?.FirstOrDefault();

        if (div == null) return [];

        var result = new Dictionary<string, object?>();

        foreach (var tr in div.Elements("div"))
        {
            var tds = tr.Elements("div").ToList();

            if (tds.Count > 0 && tds[0].HasClass("country"))
            {
                var name = tds[0].Element("div").Element("h2").InnerText.Trim();

                result.Add(name, true);
            }
        }

        return result;
    }

    private static async Task<Dictionary<string, object?>> GetVisaFree(HttpClient http)
    {
        var result = await http.GetApiData<HerleyData>("https://api.henleypassportindex.com/api/v3/countries", CancellationToken.None);

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

    private static Dictionary<string, object?> GetArea()
    {
        var web = new HtmlWeb { OverrideEncoding = Encoding.UTF8 };
        var doc = web.Load("https://www.worldometers.info/geography/largest-countries-in-the-world/");

        var ul = doc.DocumentNode.SelectNodes("//table[starts-with(@class,'datatable')]/tbody")?.FirstOrDefault();

        if (ul == null) return [];

        var result = new Dictionary<string, object?>();

        foreach (var tr in ul.Elements("tr"))
        {
            var tds = tr.Elements("td").ToList();

            var name = tds[1].InnerText.Trim();
            var success = int.TryParse(tds[2].InnerText.Trim().Replace(",", ""), out int value);

            if (!success) throw new NotificationException($"parse fail: {tds[2].InnerText.Trim()}");

            result.Add(name, value);
        }

        return result;
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

            if (!success) throw new NotificationException($"parse fail: {tds[1].Element("span").InnerText.Trim()}");

            result.Add(name, value * 10);
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
                if (!success) throw new NotificationException($"parse fail: -{name} -{vl}");

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
                if (!success) throw new NotificationException($"parse fail: {tds[cellValue].InnerText.Trim()}");
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

        var tbody = doc.DocumentNode.SelectNodes("//*[@id=\"mw-content-text\"]/div[1]/div[9]/table/tbody")?.FirstOrDefault();

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
                if (!success) throw new NotificationException($"parse fail: {tds[cellBase + cellValue].InnerText.Trim()}");
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

    private static Dictionary<string, object?> GetGDP(int cellValue)
    {
        var web = new HtmlWeb { OverrideEncoding = Encoding.UTF8 };
        var doc = web.Load("https://www.worldometers.info/gdp/gdp-per-capita/");
        //extra1: https://en.wikipedia.org/wiki/List_of_countries_by_GDP_(PPP)_per_capita
        //extra2: https://en.wikipedia.org/wiki/List_of_countries_by_GDP_(nominal)_per_capita

        var tbody = doc.DocumentNode.SelectNodes("//table[starts-with(@class,'datatable')]/tbody")?.FirstOrDefault();

        if (tbody == null) return [];

        var result = new Dictionary<string, object?>();

        foreach (var tr in tbody.Elements("tr"))
        {
            var tds = tr.Elements("td").ToList();

            var name = tds[1].Element("a").InnerText.Trim();

            var success = int.TryParse(tds[cellValue].InnerText.Trim().Replace(",", "").Replace("$", ""), out int value);
            if (!success) throw new NotificationException($"parse fail: {tds[cellValue].InnerText.Trim()}");
            result.Add(name, value);
        }

        return result;
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

    private static Dictionary<string, object?> GetInternationalArrivals()
    {
        var web = new HtmlWeb { OverrideEncoding = Encoding.UTF8 };
        var doc = web.Load("https://www.indexmundi.com/facts/indicators/ST.INT.ARVL/rankings");

        var tbody = doc.DocumentNode.SelectNodes("//*[@id=\"content\"]/div[2]/table")?.FirstOrDefault();

        if (tbody == null) return [];

        var result = new Dictionary<string, object?>();

        foreach (var tr in tbody.Elements("tr"))
        {
            if (tr.Elements("th").Any()) continue; //ignores header

            var tds = tr.Elements("td").ToList();

            var name = tds[1].Element("a").InnerText.Trim();
            var success = int.TryParse(tds[2].InnerText.Trim().Replace(",", "").Replace(".00", ""), out int value);

            if (!success) throw new NotificationException($"parse fail: {tds[2].InnerText.Trim()}");

            result.Add(name, value);
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
            .ToDictionary(s => s.country_name!, s => (object?)(int.Parse(s.OverallRank!.Split(":")[0]) * 100)) ?? [];
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
                if (!success) throw new NotificationException($"parse fail: {value}");
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
            if (!success) throw new NotificationException($"parse fail: {value}");
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
                if (!success) throw new NotificationException($"parse fail: -{name} -{vl}");

                result.Add(name, value * 10);
            }
        }

        return result;
    }

    private static Dictionary<string, object?> GetVisionOfHumanity(string fileName)
    {
        //https://www.visionofhumanity.org/maps/global-terrorism-index/#/
        //https://www.visionofhumanity.org/maps/#/

        //peace = vai de 1 a 5
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
            if (!success) throw new NotificationException($"parse fail: {value}");

            if (fileName.Contains("terrorism"))
                result.Add(name, vl * 100);
            else
                result.Add(name, Rescale(vl) * 100);
        }

        return result;
    }

    // Rescale from 1-5 to 0-10
    private static double Rescale(double original)
    {
        return (original - 1) * 2.5;
    }
}