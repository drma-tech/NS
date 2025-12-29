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

    public static List<EnumObjectCountry<TEnum>> GetListRegion<TEnum>(bool translate = true) where TEnum : struct, Enum
    {
        var result = new List<EnumObjectCountry<TEnum>>();
        foreach (var val in GetArray<TEnum>())
        {
            var attr = val.GetCustomCountryAttribute(translate) ?? throw new InvalidOperationException($"Enum {typeof(TEnum).Name} is missing CountryAttribute on value {val}");
            result.Add(new EnumObjectCountry<TEnum>(val, attr.Continent, attr.Subcontinent, attr.Name, attr.FullName, attr.Capital, attr.Description));
        }
        return result;
    }

    public static List<EnumObjectCountry<TEnum>> GetListRegion<TEnum>(IEnumerable<TEnum>? regions, bool translate = true) where TEnum : struct, Enum
    {
        var result = new List<EnumObjectCountry<TEnum>>();
        foreach (var val in regions ?? [])
        {
            var attr = val.GetCustomCountryAttribute(translate) ?? throw new InvalidOperationException($"Enum {typeof(TEnum).Name} is missing CountryAttribute on value {val}");
            result.Add(new EnumObjectCountry<TEnum>(val, attr.Continent, attr.Subcontinent, attr.Name, attr.FullName, attr.Capital, attr.Description));
        }
        return result;
    }

    public static List<EnumObjectCountry<TEnum>> GetRegions<TEnum>(this List<EnumObjectCountry<TEnum>> items, string? continent, string? subcontinent) where TEnum : struct, Enum
    {
        if (continent.NotEmpty() && subcontinent.NotEmpty())
            return items.Where(w => w.Continent == continent && w.Subcontinent == subcontinent).OrderBy(o => o.Name).ToList();
        if (continent.NotEmpty())
            return items.Where(w => w.Continent == continent).OrderBy(o => o.Name).ToList();
        else
            return items.OrderBy(o => o.Name).ToList();
    }

    public static List<string> GetContinents<TEnum>(this List<EnumObjectCountry<TEnum>> items) where TEnum : struct, Enum
    {
        return items.Select(s => s.Continent).Distinct().Order().ToList();
    }

    public static List<string?> GetSubContinents<TEnum>(this List<EnumObjectCountry<TEnum>> items, string? continent = null) where TEnum : struct, Enum
    {
        if (continent.Empty())
            return [];
        else
            return items.Where(w => w.Continent == continent).Select(s => s.Subcontinent).Distinct().Order().ToList();
    }
}

public class EnumObject<TEnum>(TEnum value, string? name, string? description, string? group) where TEnum : struct, Enum
{
    public TEnum Value { get; set; } = value;
    public string? Name { get; set; } = name;
    public string? Description { get; set; } = description;
    public string? Group { get; set; } = group;
}

public class EnumObjectCountry<TEnum>(TEnum value, string continent, string? subcontinent, string name, string fullName, string capital, string? description) where TEnum : struct, Enum
{
    public TEnum Value { get; set; } = value;
    public string Continent { get; set; } = continent;
    public string? Subcontinent { get; set; } = subcontinent;
    public string Name { get; set; } = name;
    public string FullName { get; set; } = fullName;
    public string Capital { get; set; } = capital;
    public string? Description { get; set; } = description;

    public string? CustomName => Subcontinent.NotEmpty() ? $"{Continent} | {Subcontinent} | {FullName}" : $"{Continent} | {FullName}";
}