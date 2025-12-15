namespace NS.Shared.Enums
{
    public enum ConflictLevel
    {
        [Custom(Name = "Minimal")]
        Minimal = 1, //face-grin-stars

        [Custom(Name = "Low")]
        Low = 2, //face-smile-beam

        [Custom(Name = "Medium")]
        Medium = 3, //face-meh

        [Custom(Name = "High")]
        High = 4, //face-frown
    }

    public enum ConflictForecast
    {
        [Custom(Name = "De-escalation")]
        DeEscalation = 2, //face-smile-beam

        [Custom(Name = "Consistent")]
        Consistent = 3, //face-meh

        [Custom(Name = "Escalation")]
        Escalation = 4, //face-frown
    }
}