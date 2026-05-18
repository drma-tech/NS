namespace NS.Shared.Enums
{
    public enum Religion
    {
        [Custom(Name = "Christians")]
        Christians = 1,

        [Custom(Name = "Muslims")]
        Muslims = 2,

        [Custom(Name = "Unaffiliated")]
        Unaffiliated = 3,

        [Custom(Name = "Hindus")]
        Hindus = 4,

        [Custom(Name = "Buddhists")]
        Buddhists = 5,

        [Custom(Name = "Other religions")]
        OtherReligions = 6,

        [Custom(Name = "Jews")]
        Jews = 7,
    }
}