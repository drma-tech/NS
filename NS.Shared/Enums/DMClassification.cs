namespace NS.Shared.Enums
{
    public enum DMClassification
    {
        [Custom(Name = "Hard Autocracy")]
        HardAutocracy = 1,

        [Custom(Name = "Moderate Autocracy")]
        ModerateAutocracy = 3,

        [Custom(Name = "Hybrid Regime")]
        HybridRegime = 5,

        [Custom(Name = "Deficient Democracy")]
        DeficientDemocracy = 7,

        [Custom(Name = "Working Democracy")]
        WorkingDemocracy = 9,
    }
}