using NS.Shared.Models.Auth;
using NS.Shared.Models.Country;
using NS.Shared.Models.GlobalConflicts;
using NS.Shared.Models.Holiday;
using NS.Shared.Models.News;
using NS.Shared.Models.Subscription;
using NS.Shared.Models.Weather;
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
    internal partial class JavascriptContext : JsonSerializerContext
    {
    }

    [JsonSourceGenerationOptions(PropertyNameCaseInsensitive = true)]
    [JsonSerializable(typeof(PaymentConfigurations))]
    [JsonSerializable(typeof(AuthPrincipal))]
    [JsonSerializable(typeof(AuthLogin))]
    [JsonSerializable(typeof(AuthSubscription))]
    [JsonSerializable(typeof(WishList))]
    [JsonSerializable(typeof(RegionData))]
    [JsonSerializable(typeof(Suggestion))]
    [JsonSerializable(typeof(Score))]
    [JsonSerializable(typeof(NextDestinations))]
    [JsonSerializable(typeof(TravelHistory))]
    [JsonSerializable(typeof(CacheDocument<NewsModel>))]
    [JsonSerializable(typeof(CacheDocument<WeatherModel>))]
    [JsonSerializable(typeof(CacheDocument<HolidayModel>))]
    [JsonSerializable(typeof(CacheDocument<GlobalConflicts>))]
    [JsonSerializable(typeof(AllRegions))]
    [JsonSerializable(typeof(AllTaxis))]
    internal partial class ApiContext : JsonSerializerContext
    {
    }
}