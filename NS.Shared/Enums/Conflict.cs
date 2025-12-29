namespace NS.Shared.Enums
{
    public enum ConflictLevel
    {
        [Custom(Name = "High")]
        High = 1, //face-frown

        [Custom(Name = "Medium")]
        Medium = 2, //face-meh

        [Custom(Name = "Low")]
        Low = 3, //face-smile-beam

        [Custom(Name = "Minimal")]
        Minimal = 4, //face-grin-stars
    }

    public enum ConflictForecast
    {
        [Custom(Name = "Escalation")]
        Escalation = 2, //face-frown

        [Custom(Name = "Consistent")]
        Consistent = 3, //face-meh

        [Custom(Name = "De-escalation")]
        DeEscalation = 4, //face-smile-beam
    }
}