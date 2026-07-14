namespace NS.Shared.Enums
{
    public enum ConflictLevel
    {
        [FieldSettings("Extreme")]
        Extreme = 3, //face-frown

        [FieldSettings("High")]
        High = 5, //face-meh

        [FieldSettings("Turbulent")]
        Turbulent = 7, //face-smile-beam

        [FieldSettings("Low/Inactive")]
        LowInactive = 9, //face-grin-stars
    }
}