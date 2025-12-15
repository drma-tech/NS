namespace NS.Shared.Models.Weather
{
    public class WeatherCache : CacheDocument<WeatherModel>
    {
        public WeatherCache()
        {
        }

        public WeatherCache(WeatherModel data, string key) : base(key, data, TtlCache.TwoWeeks)
        {
        }
    }
}