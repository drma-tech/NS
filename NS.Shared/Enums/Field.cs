namespace NS.Shared.Enums
{
    public enum Field
    {
        [Custom(Name = "Population")]
        Population = 1,

        [Custom(Name = "UN Member")]
        UnMember = 2,

        [Custom(Name = "Visa Free")]
        VisaFree = 3,

        [Custom(Name = "Corruption Score")]
        CorruptionScore = 4,

        [Custom(Name = "Human Development Index")]
        HDI = 5,

        [Custom(Name = "Total Area (Km²)")]
        Area = 6,

        [Custom(Name = "OECD")]
        OECD = 7,

        [Custom(Name = "TSA Safety Index")]
        TsaSafetyIndex = 8,

        [Custom(Name = "Numbeo Safety Index")]
        NumbeoSafetyIndex = 9,
    }
}