namespace NS.Shared.Enums
{
    public enum Temperature
    {
        [Custom(Name = "Celsius (°C)")]
        Celsius = 1,

        [Custom(Name = "Fahrenheit (°F)")]
        Fahrenheit = 2,
    }
}