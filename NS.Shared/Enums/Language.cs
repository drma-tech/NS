namespace NS.Shared.Enums;

/// <summary>
///     https://en.wikipedia.org/wiki/List_of_ISO_639_language_codes
///     https://en.wikipedia.org/wiki/List_of_official_languages
///     https://en.wikipedia.org/wiki/List_of_official_languages_by_country_and_territory
///     
///     todo: review and add missing languages
///     https://www.loc.gov/standards/iso639-2/php/code_list.php
/// </summary>
public enum Language
{
    [FieldSettings(nameof(Translations.Enum.Language.Abkhazian), Description = "ab", ResourceType = typeof(Translations.Enum.Language))]
    Abkhazian = 'a' * 1000 + 'b',

    [FieldSettings(nameof(Translations.Enum.Language.Afar), Description = "aa", ResourceType = typeof(Translations.Enum.Language))]
    Afar = 'a' * 1000 + 'a',

    [FieldSettings(nameof(Translations.Enum.Language.Afrikaans), Description = "af", ResourceType = typeof(Translations.Enum.Language))]
    Afrikaans = 'a' * 1000 + 'f',

    [FieldSettings(nameof(Translations.Enum.Language.Akan), Description = "ak", ResourceType = typeof(Translations.Enum.Language))]
    Akan = 'a' * 1000 + 'k',

    [FieldSettings(nameof(Translations.Enum.Language.Albanian), Description = "sq", ResourceType = typeof(Translations.Enum.Language))]
    Albanian = 's' * 1000 + 'q',

    [FieldSettings(nameof(Translations.Enum.Language.Amharic), Description = "am", ResourceType = typeof(Translations.Enum.Language))]
    Amharic = 'a' * 1000 + 'm',

    [FieldSettings(nameof(Translations.Enum.Language.Arabic), Description = "ar", ResourceType = typeof(Translations.Enum.Language))]
    Arabic = 'a' * 1000 + 'r',

    [FieldSettings(nameof(Translations.Enum.Language.Aragonese), Description = "an", ResourceType = typeof(Translations.Enum.Language))]
    Aragonese = 'a' * 1000 + 'n',

    [FieldSettings(nameof(Translations.Enum.Language.Armenian), Description = "hy", ResourceType = typeof(Translations.Enum.Language))]
    Armenian = 'h' * 1000 + 'y',

    [FieldSettings(nameof(Translations.Enum.Language.Assamese), Description = "as", ResourceType = typeof(Translations.Enum.Language))]
    Assamese = 'a' * 1000 + 's',

    [FieldSettings(nameof(Translations.Enum.Language.Avaric), Description = "av", ResourceType = typeof(Translations.Enum.Language))]
    Avaric = 'a' * 1000 + 'v',

    [FieldSettings(nameof(Translations.Enum.Language.Avestan), Description = "ae", ResourceType = typeof(Translations.Enum.Language))]
    Avestan = 'a' * 1000 + 'e',

    [FieldSettings(nameof(Translations.Enum.Language.Aymara), Description = "ay", ResourceType = typeof(Translations.Enum.Language))]
    Aymara = 'a' * 1000 + 'y',

    [FieldSettings(nameof(Translations.Enum.Language.Azerbaijani), Description = "az", ResourceType = typeof(Translations.Enum.Language))]
    Azerbaijani = 'a' * 1000 + 'z',

    [FieldSettings(nameof(Translations.Enum.Language.Bambara), Description = "bm", ResourceType = typeof(Translations.Enum.Language))]
    Bambara = 'b' * 1000 + 'm',

    [FieldSettings(nameof(Translations.Enum.Language.Bashkir), Description = "ba", ResourceType = typeof(Translations.Enum.Language))]
    Bashkir = 'b' * 1000 + 'a',

    [FieldSettings(nameof(Translations.Enum.Language.Basque), Description = "eu", ResourceType = typeof(Translations.Enum.Language))]
    Basque = 'e' * 1000 + 'u',

    [FieldSettings(nameof(Translations.Enum.Language.Belarusian), Description = "be", ResourceType = typeof(Translations.Enum.Language))]
    Belarusian = 'b' * 1000 + 'e',

    [FieldSettings(nameof(Translations.Enum.Language.Bengali), Description = "bn", ResourceType = typeof(Translations.Enum.Language))]
    Bengali = 'b' * 1000 + 'n',

    [FieldSettings(nameof(Translations.Enum.Language.Bislama), Description = "bi", ResourceType = typeof(Translations.Enum.Language))]
    Bislama = 'b' * 1000 + 'i',

    [FieldSettings(nameof(Translations.Enum.Language.Bosnian), Description = "bs", ResourceType = typeof(Translations.Enum.Language))]
    Bosnian = 'b' * 1000 + 's',

    [FieldSettings(nameof(Translations.Enum.Language.Breton), Description = "br", ResourceType = typeof(Translations.Enum.Language))]
    Breton = 'b' * 1000 + 'r',

    [FieldSettings(nameof(Translations.Enum.Language.Bulgarian), Description = "bg", ResourceType = typeof(Translations.Enum.Language))]
    Bulgarian = 'b' * 1000 + 'g',

    [FieldSettings(nameof(Translations.Enum.Language.Burmese), Description = "my", ResourceType = typeof(Translations.Enum.Language))]
    Burmese = 'm' * 1000 + 'y',

    [FieldSettings(nameof(Translations.Enum.Language.Catalan), Description = "ca", ResourceType = typeof(Translations.Enum.Language))]
    Catalan = 'c' * 1000 + 'a',

    [FieldSettings(nameof(Translations.Enum.Language.CentralKhmer), Description = "km", ResourceType = typeof(Translations.Enum.Language))]
    CentralKhmer = 'k' * 1000 + 'm',

    [FieldSettings(nameof(Translations.Enum.Language.Chamorro), Description = "ch", ResourceType = typeof(Translations.Enum.Language))]
    Chamorro = 'c' * 1000 + 'h',

    [FieldSettings(nameof(Translations.Enum.Language.Chechen), Description = "ce", ResourceType = typeof(Translations.Enum.Language))]
    Chechen = 'c' * 1000 + 'e',

    [FieldSettings(nameof(Translations.Enum.Language.Chichewa), Description = "ny", ResourceType = typeof(Translations.Enum.Language))]
    Chichewa = 'n' * 1000 + 'y',

    [FieldSettings(nameof(Translations.Enum.Language.Chinese), Description = "zh", ResourceType = typeof(Translations.Enum.Language))]
    Chinese = 'z' * 1000 + 'h',

    [FieldSettings(nameof(Translations.Enum.Language.ChurchSlavonic), Description = "cu", ResourceType = typeof(Translations.Enum.Language))]
    ChurchSlavonic = 'c' * 1000 + 'u',

    [FieldSettings(nameof(Translations.Enum.Language.Chuvash), Description = "cv", ResourceType = typeof(Translations.Enum.Language))]
    Chuvash = 'c' * 1000 + 'v',

    [FieldSettings(nameof(Translations.Enum.Language.Cornish), Description = "kw", ResourceType = typeof(Translations.Enum.Language))]
    Cornish = 'k' * 1000 + 'w',

    [FieldSettings(nameof(Translations.Enum.Language.Corsican), Description = "co", ResourceType = typeof(Translations.Enum.Language))]
    Corsican = 'c' * 1000 + 'o',

    [FieldSettings(nameof(Translations.Enum.Language.Cree), Description = "cr", ResourceType = typeof(Translations.Enum.Language))]
    Cree = 'c' * 1000 + 'r',

    [FieldSettings(nameof(Translations.Enum.Language.Croatian), Description = "hr", ResourceType = typeof(Translations.Enum.Language))]
    Croatian = 'h' * 1000 + 'r',

    [FieldSettings(nameof(Translations.Enum.Language.Czech), Description = "cs", ResourceType = typeof(Translations.Enum.Language))]
    Czech = 'c' * 1000 + 's',

    [FieldSettings(nameof(Translations.Enum.Language.Danish), Description = "da", ResourceType = typeof(Translations.Enum.Language))]
    Danish = 'd' * 1000 + 'a',

    [FieldSettings(nameof(Translations.Enum.Language.Divehi), Description = "dv", ResourceType = typeof(Translations.Enum.Language))]
    Divehi = 'd' * 1000 + 'v',

    [FieldSettings(nameof(Translations.Enum.Language.Dutch), Description = "nl", ResourceType = typeof(Translations.Enum.Language))]
    Dutch = 'n' * 1000 + 'l',

    [FieldSettings(nameof(Translations.Enum.Language.Dzongkha), Description = "dz", ResourceType = typeof(Translations.Enum.Language))]
    Dzongkha = 'd' * 1000 + 'z',

    [FieldSettings(nameof(Translations.Enum.Language.English), Description = "en", Group = "site", ResourceType = typeof(Translations.Enum.Language))]
    English = 'e' * 1000 + 'n',

    [FieldSettings(nameof(Translations.Enum.Language.Esperanto), Description = "eo", ResourceType = typeof(Translations.Enum.Language))]
    Esperanto = 'e' * 1000 + 'o',

    [FieldSettings(nameof(Translations.Enum.Language.Estonian), Description = "et", ResourceType = typeof(Translations.Enum.Language))]
    Estonian = 'e' * 1000 + 't',

    [FieldSettings(nameof(Translations.Enum.Language.Ewe), Description = "ee", ResourceType = typeof(Translations.Enum.Language))]
    Ewe = 'e' * 1000 + 'e',

    [FieldSettings(nameof(Translations.Enum.Language.Faroese), Description = "fo", ResourceType = typeof(Translations.Enum.Language))]
    Faroese = 'f' * 1000 + 'o',

    [FieldSettings(nameof(Translations.Enum.Language.Fijian), Description = "fj", ResourceType = typeof(Translations.Enum.Language))]
    Fijian = 'f' * 1000 + 'j',

    [FieldSettings(nameof(Translations.Enum.Language.Finnish), Description = "fi", ResourceType = typeof(Translations.Enum.Language))]
    Finnish = 'f' * 1000 + 'i',

    [FieldSettings(nameof(Translations.Enum.Language.French), Description = "fr", ResourceType = typeof(Translations.Enum.Language))]
    French = 'f' * 1000 + 'r',

    [FieldSettings(nameof(Translations.Enum.Language.Fulah), Description = "ff", ResourceType = typeof(Translations.Enum.Language))]
    Fulah = 'f' * 1000 + 'f',

    [FieldSettings(nameof(Translations.Enum.Language.Gaelic), Description = "gd", ResourceType = typeof(Translations.Enum.Language))]
    Gaelic = 'g' * 1000 + 'd',

    [FieldSettings(nameof(Translations.Enum.Language.Galician), Description = "gl", ResourceType = typeof(Translations.Enum.Language))]
    Galician = 'g' * 1000 + 'l',

    [FieldSettings(nameof(Translations.Enum.Language.Ganda), Description = "lg", ResourceType = typeof(Translations.Enum.Language))]
    Ganda = 'l' * 1000 + 'g',

    [FieldSettings(nameof(Translations.Enum.Language.Georgian), Description = "ka", ResourceType = typeof(Translations.Enum.Language))]
    Georgian = 'k' * 1000 + 'a',

    [FieldSettings(nameof(Translations.Enum.Language.German), Description = "de", ResourceType = typeof(Translations.Enum.Language))]
    German = 'd' * 1000 + 'e',

    [FieldSettings(nameof(Translations.Enum.Language.Greek), Description = "el", ResourceType = typeof(Translations.Enum.Language))]
    Greek = 'e' * 1000 + 'l',

    [FieldSettings(nameof(Translations.Enum.Language.Guarani), Description = "gn", ResourceType = typeof(Translations.Enum.Language))]
    Guarani = 'g' * 1000 + 'n',

    [FieldSettings(nameof(Translations.Enum.Language.Gujarati), Description = "gu", ResourceType = typeof(Translations.Enum.Language))]
    Gujarati = 'g' * 1000 + 'u',

    [FieldSettings(nameof(Translations.Enum.Language.Haitian), Description = "ht", ResourceType = typeof(Translations.Enum.Language))]
    Haitian = 'h' * 1000 + 't',

    [FieldSettings(nameof(Translations.Enum.Language.Hausa), Description = "ha", ResourceType = typeof(Translations.Enum.Language))]
    Hausa = 'h' * 1000 + 'a',

    [FieldSettings(nameof(Translations.Enum.Language.Hebrew), Description = "he", ResourceType = typeof(Translations.Enum.Language))]
    Hebrew = 'h' * 1000 + 'e',

    [FieldSettings(nameof(Translations.Enum.Language.Herero), Description = "hz", ResourceType = typeof(Translations.Enum.Language))]
    Herero = 'h' * 1000 + 'z',

    [FieldSettings(nameof(Translations.Enum.Language.Hindi), Description = "hi", ResourceType = typeof(Translations.Enum.Language))]
    Hindi = 'h' * 1000 + 'i',

    [FieldSettings(nameof(Translations.Enum.Language.HiriMotu), Description = "ho", ResourceType = typeof(Translations.Enum.Language))]
    HiriMotu = 'h' * 1000 + 'o',

    [FieldSettings(nameof(Translations.Enum.Language.Hungarian), Description = "hu", ResourceType = typeof(Translations.Enum.Language))]
    Hungarian = 'h' * 1000 + 'u',

    [FieldSettings(nameof(Translations.Enum.Language.Icelandic), Description = "is", ResourceType = typeof(Translations.Enum.Language))]
    Icelandic = 'i' * 1000 + 's',

    [FieldSettings(nameof(Translations.Enum.Language.Ido), Description = "io", ResourceType = typeof(Translations.Enum.Language))]
    Ido = 'i' * 1000 + 'o',

    [FieldSettings(nameof(Translations.Enum.Language.Igbo), Description = "ig", ResourceType = typeof(Translations.Enum.Language))]
    Igbo = 'i' * 1000 + 'g',

    [FieldSettings(nameof(Translations.Enum.Language.Indonesian), Description = "id", ResourceType = typeof(Translations.Enum.Language))]
    Indonesian = 'i' * 1000 + 'd',

    [FieldSettings(nameof(Translations.Enum.Language.Interlingua), Description = "ia", ResourceType = typeof(Translations.Enum.Language))]
    Interlingua = 'i' * 1000 + 'a',

    [FieldSettings(nameof(Translations.Enum.Language.Interlingue), Description = "ie", ResourceType = typeof(Translations.Enum.Language))]
    Interlingue = 'i' * 1000 + 'e',

    [FieldSettings(nameof(Translations.Enum.Language.Inuktitut), Description = "iu", ResourceType = typeof(Translations.Enum.Language))]
    Inuktitut = 'i' * 1000 + 'u',

    [FieldSettings(nameof(Translations.Enum.Language.Inupiaq), Description = "ik", ResourceType = typeof(Translations.Enum.Language))]
    Inupiaq = 'i' * 1000 + 'k',

    [FieldSettings(nameof(Translations.Enum.Language.Irish), Description = "ga", ResourceType = typeof(Translations.Enum.Language))]
    Irish = 'g' * 1000 + 'a',

    [FieldSettings(nameof(Translations.Enum.Language.Italian), Description = "it", ResourceType = typeof(Translations.Enum.Language))]
    Italian = 'i' * 1000 + 't',

    [FieldSettings(nameof(Translations.Enum.Language.Japanese), Description = "ja", ResourceType = typeof(Translations.Enum.Language))]
    Japanese = 'j' * 1000 + 'a',

    [FieldSettings(nameof(Translations.Enum.Language.Javanese), Description = "jv", ResourceType = typeof(Translations.Enum.Language))]
    Javanese = 'j' * 1000 + 'v',

    [FieldSettings(nameof(Translations.Enum.Language.Kalaallisut), Description = "kl", ResourceType = typeof(Translations.Enum.Language))]
    Kalaallisut = 'k' * 1000 + 'l',

    [FieldSettings(nameof(Translations.Enum.Language.Kannada), Description = "kn", ResourceType = typeof(Translations.Enum.Language))]
    Kannada = 'k' * 1000 + 'n',

    [FieldSettings(nameof(Translations.Enum.Language.Kanuri), Description = "kr", ResourceType = typeof(Translations.Enum.Language))]
    Kanuri = 'k' * 1000 + 'r',

    [FieldSettings(nameof(Translations.Enum.Language.Kashmiri), Description = "ks", ResourceType = typeof(Translations.Enum.Language))]
    Kashmiri = 'k' * 1000 + 's',

    [FieldSettings(nameof(Translations.Enum.Language.Kazakh), Description = "kk", ResourceType = typeof(Translations.Enum.Language))]
    Kazakh = 'k' * 1000 + 'k',

    [FieldSettings(nameof(Translations.Enum.Language.Kikuyu), Description = "ki", ResourceType = typeof(Translations.Enum.Language))]
    Kikuyu = 'k' * 1000 + 'i',

    [FieldSettings(nameof(Translations.Enum.Language.Kinyarwanda), Description = "rw", ResourceType = typeof(Translations.Enum.Language))]
    Kinyarwanda = 'r' * 1000 + 'w',

    [FieldSettings(nameof(Translations.Enum.Language.Kirghiz), Description = "ky", ResourceType = typeof(Translations.Enum.Language))]
    Kirghiz = 'k' * 1000 + 'y',

    [FieldSettings(nameof(Translations.Enum.Language.Komi), Description = "kv", ResourceType = typeof(Translations.Enum.Language))]
    Komi = 'k' * 1000 + 'v',

    [FieldSettings(nameof(Translations.Enum.Language.Kongo), Description = "kg", ResourceType = typeof(Translations.Enum.Language))]
    Kongo = 'k' * 1000 + 'g',

    [FieldSettings(nameof(Translations.Enum.Language.Korean), Description = "ko", ResourceType = typeof(Translations.Enum.Language))]
    Korean = 'k' * 1000 + 'o',

    [FieldSettings(nameof(Translations.Enum.Language.Kuanyama), Description = "kj", ResourceType = typeof(Translations.Enum.Language))]
    Kuanyama = 'k' * 1000 + 'j',

    [FieldSettings(nameof(Translations.Enum.Language.Kurdish), Description = "ku", ResourceType = typeof(Translations.Enum.Language))]
    Kurdish = 'k' * 1000 + 'u',

    [FieldSettings(nameof(Translations.Enum.Language.Lao), Description = "lo", ResourceType = typeof(Translations.Enum.Language))]
    Lao = 'l' * 1000 + 'o',

    [FieldSettings(nameof(Translations.Enum.Language.Latin), Description = "la", ResourceType = typeof(Translations.Enum.Language))]
    Latin = 'l' * 1000 + 'a',

    [FieldSettings(nameof(Translations.Enum.Language.Latvian), Description = "lv", ResourceType = typeof(Translations.Enum.Language))]
    Latvian = 'l' * 1000 + 'v',

    [FieldSettings(nameof(Translations.Enum.Language.Limburgan), Description = "li", ResourceType = typeof(Translations.Enum.Language))]
    Limburgan = 'l' * 1000 + 'i',

    [FieldSettings(nameof(Translations.Enum.Language.Lingala), Description = "ln", ResourceType = typeof(Translations.Enum.Language))]
    Lingala = 'l' * 1000 + 'n',

    [FieldSettings(nameof(Translations.Enum.Language.Lithuanian), Description = "lt", ResourceType = typeof(Translations.Enum.Language))]
    Lithuanian = 'l' * 1000 + 't',

    [FieldSettings(nameof(Translations.Enum.Language.LubaKatanga), Description = "lu", ResourceType = typeof(Translations.Enum.Language))]
    LubaKatanga = 'l' * 1000 + 'u',

    [FieldSettings(nameof(Translations.Enum.Language.Luxembourgish), Description = "lb", ResourceType = typeof(Translations.Enum.Language))]
    Luxembourgish = 'l' * 1000 + 'b',

    [FieldSettings(nameof(Translations.Enum.Language.Macedonian), Description = "mk", ResourceType = typeof(Translations.Enum.Language))]
    Macedonian = 'm' * 1000 + 'k',

    [FieldSettings(nameof(Translations.Enum.Language.Malagasy), Description = "mg", ResourceType = typeof(Translations.Enum.Language))]
    Malagasy = 'm' * 1000 + 'g',

    [FieldSettings(nameof(Translations.Enum.Language.Malay), Description = "ms", ResourceType = typeof(Translations.Enum.Language))]
    Malay = 'm' * 1000 + 's',

    [FieldSettings(nameof(Translations.Enum.Language.Malayalam), Description = "ml", ResourceType = typeof(Translations.Enum.Language))]
    Malayalam = 'm' * 1000 + 'l',

    [FieldSettings(nameof(Translations.Enum.Language.Maltese), Description = "mt", ResourceType = typeof(Translations.Enum.Language))]
    Maltese = 'm' * 1000 + 't',

    [FieldSettings(nameof(Translations.Enum.Language.Manx), Description = "gv", ResourceType = typeof(Translations.Enum.Language))]
    Manx = 'g' * 1000 + 'v',

    [FieldSettings(nameof(Translations.Enum.Language.Maori), Description = "mi", ResourceType = typeof(Translations.Enum.Language))]
    Maori = 'm' * 1000 + 'i',

    [FieldSettings(nameof(Translations.Enum.Language.Marathi), Description = "mr", ResourceType = typeof(Translations.Enum.Language))]
    Marathi = 'm' * 1000 + 'r',

    [FieldSettings(nameof(Translations.Enum.Language.Marshallese), Description = "mh", ResourceType = typeof(Translations.Enum.Language))]
    Marshallese = 'm' * 1000 + 'h',

    [FieldSettings(nameof(Translations.Enum.Language.Mongolian), Description = "mn", ResourceType = typeof(Translations.Enum.Language))]
    Mongolian = 'm' * 1000 + 'n',

    [FieldSettings(nameof(Translations.Enum.Language.Nauru), Description = "na", ResourceType = typeof(Translations.Enum.Language))]
    Nauru = 'n' * 1000 + 'a',

    [FieldSettings(nameof(Translations.Enum.Language.Navajo), Description = "nv", ResourceType = typeof(Translations.Enum.Language))]
    Navajo = 'n' * 1000 + 'v',

    [FieldSettings(nameof(Translations.Enum.Language.Ndonga), Description = "ng", ResourceType = typeof(Translations.Enum.Language))]
    Ndonga = 'n' * 1000 + 'g',

    [FieldSettings(nameof(Translations.Enum.Language.Nepali), Description = "ne", ResourceType = typeof(Translations.Enum.Language))]
    Nepali = 'n' * 1000 + 'e',

    [FieldSettings(nameof(Translations.Enum.Language.NorthernSami), Description = "se", ResourceType = typeof(Translations.Enum.Language))]
    NorthernSami = 's' * 1000 + 'e',

    [FieldSettings(nameof(Translations.Enum.Language.NorthNdebele), Description = "nd", ResourceType = typeof(Translations.Enum.Language))]
    NorthNdebele = 'n' * 1000 + 'd',

    [FieldSettings(nameof(Translations.Enum.Language.Norwegian), Description = "no", ResourceType = typeof(Translations.Enum.Language))]
    Norwegian = 'n' * 1000 + 'o',

    [FieldSettings(nameof(Translations.Enum.Language.NorwegianBokmål), Description = "nb", ResourceType = typeof(Translations.Enum.Language))]
    NorwegianBokmål = 'n' * 1000 + 'b',

    [FieldSettings(nameof(Translations.Enum.Language.NorwegianNynorsk), Description = "nn", ResourceType = typeof(Translations.Enum.Language))]
    NorwegianNynorsk = 'n' * 1000 + 'n',

    [FieldSettings(nameof(Translations.Enum.Language.Occitan), Description = "oc", ResourceType = typeof(Translations.Enum.Language))]
    Occitan = 'o' * 1000 + 'c',

    [FieldSettings(nameof(Translations.Enum.Language.Ojibwa), Description = "oj", ResourceType = typeof(Translations.Enum.Language))]
    Ojibwa = 'o' * 1000 + 'j',

    [FieldSettings(nameof(Translations.Enum.Language.Oriya), Description = "or", ResourceType = typeof(Translations.Enum.Language))]
    Oriya = 'o' * 1000 + 'r',

    [FieldSettings(nameof(Translations.Enum.Language.Oromo), Description = "om", ResourceType = typeof(Translations.Enum.Language))]
    Oromo = 'o' * 1000 + 'm',

    [FieldSettings(nameof(Translations.Enum.Language.Ossetian), Description = "os", ResourceType = typeof(Translations.Enum.Language))]
    Ossetian = 'o' * 1000 + 's',

    [FieldSettings(nameof(Translations.Enum.Language.Pali), Description = "pi", ResourceType = typeof(Translations.Enum.Language))]
    Pali = 'p' * 1000 + 'i',

    [FieldSettings(nameof(Translations.Enum.Language.Pashto), Description = "ps", ResourceType = typeof(Translations.Enum.Language))]
    Pashto = 'p' * 1000 + 's',

    [FieldSettings(nameof(Translations.Enum.Language.Persian), Description = "fa", ResourceType = typeof(Translations.Enum.Language))]
    Persian = 'f' * 1000 + 'a',

    [FieldSettings(nameof(Translations.Enum.Language.Polish), Description = "pl", ResourceType = typeof(Translations.Enum.Language))]
    Polish = 'p' * 1000 + 'l',

    [FieldSettings(nameof(Translations.Enum.Language.Portuguese), Description = "pt", Group = "site", ResourceType = typeof(Translations.Enum.Language))]
    Portuguese = 'p' * 1000 + 't',

    [FieldSettings(nameof(Translations.Enum.Language.Punjabi), Description = "pa", ResourceType = typeof(Translations.Enum.Language))]
    Punjabi = 'p' * 1000 + 'a',

    [FieldSettings(nameof(Translations.Enum.Language.Quechua), Description = "qu", ResourceType = typeof(Translations.Enum.Language))]
    Quechua = 'q' * 1000 + 'u',

    [FieldSettings(nameof(Translations.Enum.Language.Romanian), Description = "ro", ResourceType = typeof(Translations.Enum.Language))]
    Romanian = 'r' * 1000 + 'o',

    [FieldSettings(nameof(Translations.Enum.Language.Romansh), Description = "rm", ResourceType = typeof(Translations.Enum.Language))]
    Romansh = 'r' * 1000 + 'm',

    [FieldSettings(nameof(Translations.Enum.Language.Rundi), Description = "rn", ResourceType = typeof(Translations.Enum.Language))]
    Rundi = 'r' * 1000 + 'n',

    [FieldSettings(nameof(Translations.Enum.Language.Russian), Description = "ru", ResourceType = typeof(Translations.Enum.Language))]
    Russian = 'r' * 1000 + 'u',

    [FieldSettings(nameof(Translations.Enum.Language.Samoan), Description = "sm", ResourceType = typeof(Translations.Enum.Language))]
    Samoan = 's' * 1000 + 'm',

    [FieldSettings(nameof(Translations.Enum.Language.Sango), Description = "sg", ResourceType = typeof(Translations.Enum.Language))]
    Sango = 's' * 1000 + 'g',

    [FieldSettings(nameof(Translations.Enum.Language.Sanskrit), Description = "sa", ResourceType = typeof(Translations.Enum.Language))]
    Sanskrit = 's' * 1000 + 'a',

    [FieldSettings(nameof(Translations.Enum.Language.Sardinian), Description = "sc", ResourceType = typeof(Translations.Enum.Language))]
    Sardinian = 's' * 1000 + 'c',

    [FieldSettings(nameof(Translations.Enum.Language.Serbian), Description = "sr", ResourceType = typeof(Translations.Enum.Language))]
    Serbian = 's' * 1000 + 'r',

    [FieldSettings(nameof(Translations.Enum.Language.Shona), Description = "sn", ResourceType = typeof(Translations.Enum.Language))]
    Shona = 's' * 1000 + 'n',

    [FieldSettings(nameof(Translations.Enum.Language.SichuanYi), Description = "ii", ResourceType = typeof(Translations.Enum.Language))]
    SichuanYi = 'i' * 1000 + 'i',

    [FieldSettings(nameof(Translations.Enum.Language.Sindhi), Description = "sd", ResourceType = typeof(Translations.Enum.Language))]
    Sindhi = 's' * 1000 + 'd',

    [FieldSettings(nameof(Translations.Enum.Language.Sinhala), Description = "si", ResourceType = typeof(Translations.Enum.Language))]
    Sinhala = 's' * 1000 + 'i',

    [FieldSettings(nameof(Translations.Enum.Language.Slovak), Description = "sk", ResourceType = typeof(Translations.Enum.Language))]
    Slovak = 's' * 1000 + 'k',

    [FieldSettings(nameof(Translations.Enum.Language.Slovenian), Description = "sl", ResourceType = typeof(Translations.Enum.Language))]
    Slovenian = 's' * 1000 + 'l',

    [FieldSettings(nameof(Translations.Enum.Language.Somali), Description = "so", ResourceType = typeof(Translations.Enum.Language))]
    Somali = 's' * 1000 + 'o',

    [FieldSettings(nameof(Translations.Enum.Language.SouthernSotho), Description = "st", ResourceType = typeof(Translations.Enum.Language))]
    SouthernSotho = 's' * 1000 + 't',

    [FieldSettings(nameof(Translations.Enum.Language.SouthNdebele), Description = "nr", ResourceType = typeof(Translations.Enum.Language))]
    SouthNdebele = 'n' * 1000 + 'r',

    [FieldSettings(nameof(Translations.Enum.Language.Spanish), Description = "es", Group = "site", ResourceType = typeof(Translations.Enum.Language))]
    Spanish = 'e' * 1000 + 's',

    [FieldSettings(nameof(Translations.Enum.Language.Sundanese), Description = "su", ResourceType = typeof(Translations.Enum.Language))]
    Sundanese = 's' * 1000 + 'u',

    [FieldSettings(nameof(Translations.Enum.Language.Swahili), Description = "sw", ResourceType = typeof(Translations.Enum.Language))]
    Swahili = 's' * 1000 + 'w',

    [FieldSettings(nameof(Translations.Enum.Language.Swati), Description = "ss", ResourceType = typeof(Translations.Enum.Language))]
    Swati = 's' * 1000 + 's',

    [FieldSettings(nameof(Translations.Enum.Language.Swedish), Description = "sv", ResourceType = typeof(Translations.Enum.Language))]
    Swedish = 's' * 1000 + 'v',

    [FieldSettings(nameof(Translations.Enum.Language.Tagalog), Description = "tl", ResourceType = typeof(Translations.Enum.Language))]
    Tagalog = 't' * 1000 + 'l',

    [FieldSettings(nameof(Translations.Enum.Language.Tahitian), Description = "ty", ResourceType = typeof(Translations.Enum.Language))]
    Tahitian = 't' * 1000 + 'y',

    [FieldSettings(nameof(Translations.Enum.Language.Tajik), Description = "tg", ResourceType = typeof(Translations.Enum.Language))]
    Tajik = 't' * 1000 + 'g',

    [FieldSettings(nameof(Translations.Enum.Language.Tamil), Description = "ta", ResourceType = typeof(Translations.Enum.Language))]
    Tamil = 't' * 1000 + 'a',

    [FieldSettings(nameof(Translations.Enum.Language.Tatar), Description = "tt", ResourceType = typeof(Translations.Enum.Language))]
    Tatar = 't' * 1000 + 't',

    [FieldSettings(nameof(Translations.Enum.Language.Telugu), Description = "te", ResourceType = typeof(Translations.Enum.Language))]
    Telugu = 't' * 1000 + 'e',

    [FieldSettings(nameof(Translations.Enum.Language.Thai), Description = "th", ResourceType = typeof(Translations.Enum.Language))]
    Thai = 't' * 1000 + 'h',

    [FieldSettings(nameof(Translations.Enum.Language.Tibetan), Description = "bo", ResourceType = typeof(Translations.Enum.Language))]
    Tibetan = 'b' * 1000 + 'o',

    [FieldSettings(nameof(Translations.Enum.Language.Tigrinya), Description = "ti", ResourceType = typeof(Translations.Enum.Language))]
    Tigrinya = 't' * 1000 + 'i',

    [FieldSettings(nameof(Translations.Enum.Language.Tonga), Description = "to", ResourceType = typeof(Translations.Enum.Language))]
    Tonga = 't' * 1000 + 'o',

    [FieldSettings(nameof(Translations.Enum.Language.Tsonga), Description = "ts", ResourceType = typeof(Translations.Enum.Language))]
    Tsonga = 't' * 1000 + 's',

    [FieldSettings(nameof(Translations.Enum.Language.Tswana), Description = "tn", ResourceType = typeof(Translations.Enum.Language))]
    Tswana = 't' * 1000 + 'n',

    [FieldSettings(nameof(Translations.Enum.Language.Turkish), Description = "tr", ResourceType = typeof(Translations.Enum.Language))]
    Turkish = 't' * 1000 + 'r',

    [FieldSettings(nameof(Translations.Enum.Language.Turkmen), Description = "tk", ResourceType = typeof(Translations.Enum.Language))]
    Turkmen = 't' * 1000 + 'k',

    [FieldSettings(nameof(Translations.Enum.Language.Twi), Description = "tw", ResourceType = typeof(Translations.Enum.Language))]
    Twi = 't' * 1000 + 'w',

    [FieldSettings(nameof(Translations.Enum.Language.Uighur), Description = "ug", ResourceType = typeof(Translations.Enum.Language))]
    Uighur = 'u' * 1000 + 'g',

    [FieldSettings(nameof(Translations.Enum.Language.Ukrainian), Description = "uk", ResourceType = typeof(Translations.Enum.Language))]
    Ukrainian = 'u' * 1000 + 'k',

    [FieldSettings(nameof(Translations.Enum.Language.Urdu), Description = "ur", ResourceType = typeof(Translations.Enum.Language))]
    Urdu = 'u' * 1000 + 'r',

    [FieldSettings(nameof(Translations.Enum.Language.Uzbek), Description = "uz", ResourceType = typeof(Translations.Enum.Language))]
    Uzbek = 'u' * 1000 + 'z',

    [FieldSettings(nameof(Translations.Enum.Language.Venda), Description = "ve", ResourceType = typeof(Translations.Enum.Language))]
    Venda = 'v' * 1000 + 'e',

    [FieldSettings(nameof(Translations.Enum.Language.Vietnamese), Description = "vi", ResourceType = typeof(Translations.Enum.Language))]
    Vietnamese = 'v' * 1000 + 'i',

    [FieldSettings(nameof(Translations.Enum.Language.Volapük), Description = "vo", ResourceType = typeof(Translations.Enum.Language))]
    Volapük = 'v' * 1000 + 'o',

    [FieldSettings(nameof(Translations.Enum.Language.Walloon), Description = "wa", ResourceType = typeof(Translations.Enum.Language))]
    Walloon = 'w' * 1000 + 'a',

    [FieldSettings(nameof(Translations.Enum.Language.Welsh), Description = "cy", ResourceType = typeof(Translations.Enum.Language))]
    Welsh = 'c' * 1000 + 'y',

    [FieldSettings(nameof(Translations.Enum.Language.WesternFrisian), Description = "fy", ResourceType = typeof(Translations.Enum.Language))]
    WesternFrisian = 'f' * 1000 + 'y',

    [FieldSettings(nameof(Translations.Enum.Language.Wolof), Description = "wo", ResourceType = typeof(Translations.Enum.Language))]
    Wolof = 'w' * 1000 + 'o',

    [FieldSettings(nameof(Translations.Enum.Language.Xhosa), Description = "xh", ResourceType = typeof(Translations.Enum.Language))]
    Xhosa = 'x' * 1000 + 'h',

    [FieldSettings(nameof(Translations.Enum.Language.Yiddish), Description = "yi", ResourceType = typeof(Translations.Enum.Language))]
    Yiddish = 'y' * 1000 + 'i',

    [FieldSettings(nameof(Translations.Enum.Language.Yoruba), Description = "yo", ResourceType = typeof(Translations.Enum.Language))]
    Yoruba = 'y' * 1000 + 'o',

    [FieldSettings(nameof(Translations.Enum.Language.Zhuang), Description = "za", ResourceType = typeof(Translations.Enum.Language))]
    Zhuang = 'z' * 1000 + 'a',

    [FieldSettings(nameof(Translations.Enum.Language.Zulu), Description = "zu", ResourceType = typeof(Translations.Enum.Language))]
    Zulu = 'z' * 1000 + 'u'
}
