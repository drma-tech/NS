using System.Text.Json.Serialization;

namespace NS.WEB.Core.Api
{
    [JsonSourceGenerationOptions(PropertyNameCaseInsensitive = true)]
    [JsonSerializable(typeof(bool?))]
    [JsonSerializable(typeof(string))]
    [JsonSerializable(typeof(Platform?))]
    [JsonSerializable(typeof(AppLanguage?))]
    [JsonSerializable(typeof(AuthProvider))]
    [JsonSerializable(typeof(Temperature?))]
    [JsonSerializable(typeof(HashSet<DateTime>))]
    internal sealed partial class JavascriptContext : JsonSerializerContext
    {
    }
}