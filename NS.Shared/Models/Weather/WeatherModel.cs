namespace NS.Shared.Models.Weather
{
    public class WeatherModel
    {
        public MonthlyWeather? Current { get; set; }
        public MonthlyWeather? Month1 { get; set; }
        public MonthlyWeather? Month2 { get; set; }
    }

    public class MonthlyWeather
    {
        public double? temp_c { get; set; }
        public double? temp_f { get; set; }
        public double? feels_like_c { get; set; }
        public double? feels_like_f { get; set; }
        public string? condition_text { get; set; }
        public string? condition_icon { get; set; }
    }
}