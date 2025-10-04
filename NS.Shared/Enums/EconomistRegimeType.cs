namespace NS.Shared.Enums
{
    public enum EconomistRegimeType
    {
        [Custom(Name = "Full Democracy")]
        FullDemocracy = 1,

        [Custom(Name = "Flawed Democracy")]
        FlawedDemocracy = 2,

        [Custom(Name = "Hybrid Regime")]
        HybridRegime = 3,

        [Custom(Name = "Authoritarian")]
        Authoritarian = 4
    }
}