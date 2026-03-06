using System.Text.Json.Serialization;

namespace NS.WEB.Core.Api
{
    [JsonSourceGenerationOptions(PropertyNameCaseInsensitive = true)]
    [JsonSerializable(typeof(Platform?))]
    [JsonSerializable(typeof(AppLanguage?))]
    [JsonSerializable(typeof(bool?))]
    [JsonSerializable(typeof(string))]
    internal partial class JavascriptContext : JsonSerializerContext
    {
    }
}