namespace NS.Shared.Enums
{
    public enum EconomistRegimeType
    {
        [Custom(Name = "Authoritarian")]
        Authoritarian = 1,

        [Custom(Name = "Hybrid Regime")]
        HybridRegime = 2,

        [Custom(Name = "Flawed Democracy")]
        FlawedDemocracy = 3,

        [Custom(Name = "Full Democracy")]
        FullDemocracy = 4,
    }
}