namespace NS.Shared.Enums
{
    public enum DMClassification
    {
        [Custom(Name = "Working Democracy")]
        WorkingDemocracy = 1,

        [Custom(Name = "Deficient Democracy")]
        DeficientDemocracy = 2,

        [Custom(Name = "Hybrid Regime")]
        HybridRegime = 3,

        [Custom(Name = "Moderate Autocracy")]
        ModerateAutocracy = 4,

        [Custom(Name = "Hard Autocracy")]
        HardAutocracy = 5
    }
}