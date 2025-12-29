namespace NS.Shared.Enums
{
    public enum DMClassification
    {
        [Custom(Name = "Hard Autocracy")]
        HardAutocracy = 1,

        [Custom(Name = "Moderate Autocracy")]
        ModerateAutocracy = 2,

        [Custom(Name = "Hybrid Regime")]
        HybridRegime = 3,

        [Custom(Name = "Deficient Democracy")]
        DeficientDemocracy = 4,

        [Custom(Name = "Working Democracy")]
        WorkingDemocracy = 5,
    }
}