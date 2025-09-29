using ExcelDataReader;
using HtmlAgilityPack;
using NS.API.Core.Models;
using NS.Shared.Models.Country;
using System.Diagnostics.Metrics;
using System.Text;
using System.Text.Json;

namespace NS.API.Core.Scraping;

public static class ScrapingBasic
{
    public static async Task<Dictionary<string, object?>> GetData(Field field, HttpClient http, CountryImport import)
    {
        return field switch
        {
            Field.Population => GetPopulation(),
            Field.UnMember => GetUnMember(),
            Field.VisaFree => await GetVisaFree(http),
            Field.CorruptionScore => await GetCorruptionScore(),
            Field.HDI => GetHDI(),
            Field.Area => GetArea(),
            Field.OECD => GetOECD(),
            Field.TsaSafetyIndex => GetTsaSafetyIndex(),
            Field.NumbeoSafetyIndex => GetNumbeoSafetyIndex(import),
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

        return result?.Where(p => p.year == DateTime.Now.Year - 1).ToDictionary(s => s.country!, s => (object?)s.score) ?? [];
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

                        dic.Add(reader.GetString(1), reader.GetDouble(2));
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

            result.Add(name, value);
        }

        return result;
    }

    private static Dictionary<string, object?> GetNumbeoSafetyIndex(CountryImport import)
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

                //var code = import.CustomNames.FirstOrDefault(p => p.Value.Equals(name, StringComparison.CurrentCultureIgnoreCase)).Key;
                //if (import.CustomNames.Count > 0 && code.Empty()) continue; //if processed before, only process previous errors

                var docC = web.Load($"https://www.numbeo.com/crime/{endpoint}");
                var tableC = docC.DocumentNode.SelectNodes("//table[starts-with(@class,'table_indices')]")?.FirstOrDefault();
                var vl = tableC?.Elements("tr").Last().Elements("td").Last().InnerText.Trim();
                if (vl == "?") //no data available
                {
                    result.Add(name, null);
                    continue;
                }

                var success = float.TryParse(vl, out float value);
                if (!success) throw new NotificationException($"parse fail: -{name} -{vl}");

                result.Add(name, value);
            }
        }

        return result;
    }
}