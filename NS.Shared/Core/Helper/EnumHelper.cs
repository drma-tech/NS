namespace NS.Shared.Core.Helper;

public static class EnumHelper
{
    public static TEnum[] GetArray<TEnum>() where TEnum : struct, Enum
    {
        return Enum.GetValues<TEnum>();
    }

    public static List<EnumObject<TEnum>> GetList<TEnum>(bool translate = true) where TEnum : struct, Enum
    {
        var result = new List<EnumObject<TEnum>>();
        foreach (var val in GetArray<TEnum>())
        {
            var attr = val.GetCustomAttribute(translate);

            result.Add(new EnumObject<TEnum>(val, attr?.Name, attr?.Description, attr?.Group));
        }
        return result;
    }

    public static List<EnumObjectCountry<TEnum>> GetListCountry<TEnum>(bool translate = true) where TEnum : struct, Enum
    {
        var result = new List<EnumObjectCountry<TEnum>>();
        foreach (var val in GetArray<TEnum>())
        {
            var attr = val.GetCustomCountryAttribute(translate) ?? throw new InvalidOperationException($"Enum {typeof(TEnum).Name} is missing CountryAttribute on value {val}");
            result.Add(new EnumObjectCountry<TEnum>(val, attr.Region, attr.Subregion, attr.Name, attr.FullName, attr.Capital, attr.Description));
        }
        return result;
    }

    public static List<EnumObjectCountry<TEnum>> GetCountries<TEnum>(this List<EnumObjectCountry<TEnum>> items, string? region, string? subregion) where TEnum : struct, Enum
    {
        if (region.NotEmpty() && subregion.NotEmpty())
            return items.Where(w => w.Region == region && w.Subregion == subregion).OrderBy(o => o.Name).ToList();
        if (region.NotEmpty())
            return items.Where(w => w.Region == region).OrderBy(o => o.Name).ToList();
        else
            return items.OrderBy(o => o.Name).ToList();
    }

    public static List<string> GetRegions<TEnum>(this List<EnumObjectCountry<TEnum>> items) where TEnum : struct, Enum
    {
        return items.Select(s => s.Region).Distinct().Order().ToList();
    }

    public static List<string?> GetSubRegions<TEnum>(this List<EnumObjectCountry<TEnum>> items, string? region = null) where TEnum : struct, Enum
    {
        if (region.Empty())
            return [];
        else
            return items.Where(w => w.Region == region).Select(s => s.Subregion).Distinct().Order().ToList();
    }
}

public class EnumObject<TEnum>(TEnum value, string? name, string? description, string? group) where TEnum : struct, Enum
{
    public TEnum Value { get; set; } = value;
    public string? Name { get; set; } = name;
    public string? Description { get; set; } = description;
    public string? Group { get; set; } = group;
}

public class EnumObjectCountry<TEnum>(TEnum value, string region, string? subregion, string name, string fullName, string capital, string? description) where TEnum : struct, Enum
{
    public TEnum Value { get; set; } = value;
    public string Region { get; set; } = region;
    public string? Subregion { get; set; } = subregion;
    public string Name { get; set; } = name;
    public string FullName { get; set; } = fullName;
    public string Capital { get; set; } = capital;
    public string? Description { get; set; } = description;

    public string? Flag => $"https://flagcdn.com/{Value.ToString().ToLower()}.svg";
    public string? CustomName => Subregion.NotEmpty() ? $"{Region} | {Subregion} | {FullName}" : $"{Region} | {FullName}";
}