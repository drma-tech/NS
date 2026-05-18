namespace NS.Shared.Enums
{
    public enum ConflictLevel
    {
        [Custom(Name = "Extreme")]
        Extreme = 3, //face-frown

        [Custom(Name = "High")]
        High = 5, //face-meh

        [Custom(Name = "Turbulent")]
        Turbulent = 7, //face-smile-beam

        [Custom(Name = "Low/Inactive")]
        LowInactive = 9, //face-grin-stars
    }
}