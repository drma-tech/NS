namespace NS.Shared.Enums
{
    public enum ConflictLevel
    {
        [Custom(Name = "High")]
        High = 3, //face-frown

        [Custom(Name = "Medium")]
        Medium = 5, //face-meh

        [Custom(Name = "Low")]
        Low = 7, //face-smile-beam

        [Custom(Name = "Minimal")]
        Minimal = 9, //face-grin-stars
    }
}