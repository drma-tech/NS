using NS.Common.Models.Auth;
using NS.Common.Models.Country;
using NS.Common.Models.GlobalConflicts;
using NS.Common.Models.Holiday;
using NS.Common.Models.News;
using NS.Common.Models.Subscription;
using NS.Common.Models.Weather;
using System.Text.Json.Serialization;

namespace NS.WEB.Core.Context
{
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
    [JsonSerializable(typeof(NewsCache))]
    [JsonSerializable(typeof(WeatherCache))]
    [JsonSerializable(typeof(HolidayCache))]
    [JsonSerializable(typeof(GlobalConflictsCache))]
    [JsonSerializable(typeof(AllRegions))]
    [JsonSerializable(typeof(AllTaxis))]
    internal sealed partial class ApiContext : JsonSerializerContext
    {
    }
}