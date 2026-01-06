namespace NS.Shared.Enums
{
    public enum EconomistRegimeType
    {
        [Custom(Name = "Authoritarian")]
        Authoritarian = 1,

        [Custom(Name = "Hybrid Regime")]
        HybridRegime = 5,

        [Custom(Name = "Flawed Democracy")]
        FlawedDemocracy = 7,

        [Custom(Name = "Full Democracy")]
        FullDemocracy = 9,
    }
}