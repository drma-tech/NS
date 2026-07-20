namespace NS.Shared.Enums
{
    public enum Religion
    {
        [FieldSettings(nameof(Translations.Enum.Religion.Christians), ResourceType = typeof(Translations.Enum.Religion))]
        Christians = 1,

        [FieldSettings(nameof(Translations.Enum.Religion.Muslims), ResourceType = typeof(Translations.Enum.Religion))]
        Muslims = 2,

        [FieldSettings(nameof(Translations.Enum.Religion.Unaffiliated), ResourceType = typeof(Translations.Enum.Religion))]
        Unaffiliated = 3,

        [FieldSettings(nameof(Translations.Enum.Religion.Hindus), ResourceType = typeof(Translations.Enum.Religion))]
        Hindus = 4,

        [FieldSettings(nameof(Translations.Enum.Religion.Buddhists), ResourceType = typeof(Translations.Enum.Religion))]
        Buddhists = 5,

        [FieldSettings(nameof(Translations.Enum.Religion.OtherReligions), ResourceType = typeof(Translations.Enum.Religion))]
        OtherReligions = 6,

        [FieldSettings(nameof(Translations.Enum.Religion.Jews), ResourceType = typeof(Translations.Enum.Religion))]
        Jews = 7,
    }
}