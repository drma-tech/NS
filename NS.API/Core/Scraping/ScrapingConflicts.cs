using HtmlAgilityPack;
using NS.Shared.Models.GlobalConflicts;

namespace NS.API.Core.Scraping;

public static class ScrapingConflicts
{
    private const string Url = "https://www.cfr.org/global-conflict-tracker";

    public static GlobalConflicts GetConflicts()
    {
        var result = ProcessHtml(Url);

        return result ?? new GlobalConflicts();
    }

    private static GlobalConflicts? ProcessHtml(string path)
    {
        var web = new HtmlWeb();
        var doc = web.Load(path);

        var ul = doc.DocumentNode.SelectNodes("//*[@id=\"main-wrapper\"]/div[1]/ul")?.FirstOrDefault();

        var result = new GlobalConflicts();

        foreach (var node in ul.Elements("li"))
        {
            result.Items.Add(new GlobalConflictsItem
            {
                title = node.SelectSingleNode("h3")?.InnerText.Trim() ?? null,
                type = node.SelectSingleNode("p[2]/text()[2]")?.InnerText.Trim() ?? null,
                //impact = node.SelectSingleNode("p[3]/text()[2]")?.InnerText.Trim() ?? null,
                status = node.SelectSingleNode("p[4]/text()[2]")?.InnerText.Trim() ?? null,
                regions = node.SelectSingleNode("p[5]/text()[3]")?.InnerText.Split(",").Select(p => p.Trim()).ToList() ?? []
            });
        }

        return result;
    }
}