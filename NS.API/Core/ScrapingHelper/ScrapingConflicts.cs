using NS.Shared.Models.GlobalConflicts;
using System.Globalization;
using System.Text.Json;

namespace NS.API.Core.ScrapingHelper;

public static class ScrapingConflicts
{
    public static async Task<GlobalConflictsModel?> GetConflicts(this IHttpClientFactory factory)
    {
        var url = string.Create(CultureInfo.InvariantCulture, $"https://warwatch.ch/wp-json/warwatch/v1/all-map-data?date={DateTime.Now:yyyy-MM}");

        var client = factory.CreateClient("generic");

        var json = await client.GetStringAsync(url);

        using var document = JsonDocument.Parse(json);

        var result = new GlobalConflictsModel();

        document.RootElement.TryGetProperty("map_data", out var map_data);
        document.RootElement.TryGetProperty("geography", out var geography);

        map_data.TryGetProperty("countries_data", out var countries_data);
        geography.TryGetProperty("countries", out var countries);

        // build map from country id -> iso (iso_alpha_3 is available under map_data.countries)
        var idToIso = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        foreach (var c in countries.EnumerateArray())
        {
            c.TryGetProperty("iso_alpha_3", out var iso_alpha_3);
            c.TryGetProperty("name", out var name);

            idToIso.Add(name.ToString(), iso_alpha_3.ToString());
        }

        foreach (var country in countries_data.EnumerateObject().Select(p => p.Value))
        {
            country.TryGetProperty("situations", out var situations);

            foreach (var situation in situations.EnumerateArray())
            {
                // collect violations at situation level
                var violationsList = new List<string>();
                if (situation.TryGetProperty("violations", out var violations) && violations.ValueKind == JsonValueKind.Array)
                {
                    foreach (var v in violations.EnumerateArray())
                    {
                        v.TryGetProperty("name", out var vname);
                        violationsList.Add(vname.GetString()?.Trim() ?? string.Empty);
                    }
                }

                if (!situation.TryGetProperty("conflicts", out var conflicts) || conflicts.ValueKind != JsonValueKind.Array)
                    continue;

                foreach (var conflict in conflicts.EnumerateArray())
                {
                    var item = new GlobalConflictsItem();

                    conflict.TryGetProperty("id", out var id);
                    item.id = id.GetInt32();

                    conflict.TryGetProperty("short_title", out var titleProp);
                    item.title = titleProp.GetString();

                    // non_state_parties
                    var nonState = new List<string>();
                    if (conflict.TryGetProperty("non_state_parties", out var nonStateProp) && nonStateProp.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var n in nonStateProp.EnumerateArray())
                        {
                            if (n.ValueKind == JsonValueKind.String)
                                nonState.Add(n.GetString());
                            else if (n.ValueKind == JsonValueKind.Object && n.TryGetProperty("name", out var nname))
                                nonState.Add(nname.GetString());
                        }
                    }

                    item.non_state_parties = nonState;

                    // state_parties
                    var stateList = new List<string>();
                    conflict.TryGetProperty("state_parties", out var statePartiesProp);

                    if (!statePartiesProp.EnumerateArray().Any()) continue;

                    foreach (var s in statePartiesProp.EnumerateArray())
                    {
                        string? name = null;
                        string? iso = null;

                        //there is not code/id relation, so i force compare by name, and some names are not equal

                        if (s.TryGetProperty("name", out var sname))
                            name = sname.GetString();

                        if (idToIso.TryGetValue(name, out var mappedIso))
                        {
                            iso = mappedIso;
                        }
                        else
                        {
                            if (string.Equals(name, "United Arab Emirates (UAE)", StringComparison.OrdinalIgnoreCase))
                                iso = "ARE";
                            else if (string.Equals(name, "United Kingdom (UK)", StringComparison.OrdinalIgnoreCase))
                                iso = "GBR";
                            else if (string.Equals(name, "United States of America (US)", StringComparison.OrdinalIgnoreCase))
                                iso = "USA";
                            else if (string.Equals(name, "the Netherlands", StringComparison.OrdinalIgnoreCase))
                                iso = "NLD";
                            else if (string.Equals(name, "Democratic Republic of Congo", StringComparison.OrdinalIgnoreCase))
                                iso = "COD";
                            else if (string.Equals(name, "Uganda People's Defence Force (UPDF)", StringComparison.OrdinalIgnoreCase))
                                iso = "UGA";
                            else if (string.Equals(name, "Democratic People’s Republic of Korea (North Korea)", StringComparison.OrdinalIgnoreCase))
                                iso = "PRK";
                            else if (string.Equals(name, "United Republic of Tanzania", StringComparison.OrdinalIgnoreCase))
                                iso = "TZA";
                            else if (string.Equals(name, "Myanmar", StringComparison.OrdinalIgnoreCase))
                                iso = "MMR";
                            else if (string.Equals(name, "Senegal", StringComparison.OrdinalIgnoreCase))
                                iso = "SEN";
                            else
                                throw new InvalidOperationException($"country reference not found: {name}");
                        }

                        stateList.Add(iso);
                    }

                    item.state_parties = stateList;

                    item.violations = violationsList;

                    conflict.TryGetProperty("is_active", out var isActiveProp);

                    if (!isActiveProp.GetBoolean()) continue;

                    result.Items.Add(item);
                }
            }
        }

        return result;
    }
}