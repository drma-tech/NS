using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace NS.Shared.Core.Helper;

public static class ExtensionMethods
{
    public static bool Empty<TSource>(this IEnumerable<TSource> source)
    {
        return !source.Any();
    }

    public static bool Empty<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        return !source.Any(predicate);
    }

    public static bool Empty([NotNullWhen(false)] this string? value)
    {
        return string.IsNullOrEmpty(value);
    }

    public static bool NotEmpty<TSource>(this IEnumerable<TSource> source)
    {
        return source.Any();
    }

    public static bool NotEmpty([NotNullWhen(true)] this string? value)
    {
        return !string.IsNullOrEmpty(value);
    }

    public static string? Truncate(this string? value, int maxLength, string truncationSuffix = "…")
    {
        return value?.Length > maxLength
            ? value[..maxLength] + truncationSuffix
            : value;
    }

    public static string SimpleEncrypt(this string? url)
    {
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(url ?? ""));
    }

    public static string SimpleDecrypt(this string? obfuscatedUrl)
    {
        return Encoding.UTF8.GetString(Convert.FromBase64String(obfuscatedUrl ?? ""));
    }

    public static string? GetFlag(this string? value) => value.NotEmpty() ? $"https://flagcdn.com/{value.ToLower()}.svg" : null;

    public static T DeepClone<T>(this T instance) where T : class
    {
        var json = JsonSerializer.Serialize(instance);
        return JsonSerializer.Deserialize<T>(json) ?? throw new InvalidOperationException("Clone failed");
    }

    public static double Rescale(this double original, double fromMin, double fromMax, double toMin, double toMax)
    {
        if (fromMax == fromMin) throw new ArgumentException("fromMax and fromMin cannot be the same value.");
        if (toMin == toMax) throw new ArgumentException("toMin and toMax cannot be the same value.");

        double normalized = (original - fromMin) / (fromMax - fromMin);
        return toMin + normalized * (toMax - toMin);
    }

    public static int Invert(this int value, int min = 0, int max = 10)
    {
        return max + min - value;
    }

    public static double Invert(this double value, double min = 0, double max = 10)
    {
        return max + min - value;
    }

    public static float Invert(this float value, float min = 0, float max = 10)
    {
        return max + min - value;
    }

    public static int? ConvertToInt(this object? value)
    {
        return value == null ? null : (int)decimal.Parse(value.ToString()!, CultureInfo.InvariantCulture);
    }

    public static double? ConvertToDouble(this object? value)
    {
        return value == null ? null : double.Parse(value.ToString()!, CultureInfo.InvariantCulture);
    }
}