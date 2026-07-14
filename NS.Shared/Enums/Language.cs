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
    [FieldSettings("Abkhazian", Description = "ab", ResourceType = typeof(Translations.Enum.Language))]
    Abkhazian = 'a' * 1000 + 'b',

    [FieldSettings("Afar", Description = "aa", ResourceType = typeof(Translations.Enum.Language))]
    Afar = 'a' * 1000 + 'a',

    [FieldSettings("Afrikaans", Description = "af", ResourceType = typeof(Translations.Enum.Language))]
    Afrikaans = 'a' * 1000 + 'f',

    [FieldSettings("Akan", Description = "ak", ResourceType = typeof(Translations.Enum.Language))]
    Akan = 'a' * 1000 + 'k',

    [FieldSettings("Albanian", Description = "sq", ResourceType = typeof(Translations.Enum.Language))]
    Albanian = 's' * 1000 + 'q',

    [FieldSettings("Amharic", Description = "am", ResourceType = typeof(Translations.Enum.Language))]
    Amharic = 'a' * 1000 + 'm',

    [FieldSettings("Arabic", Description = "ar", ResourceType = typeof(Translations.Enum.Language))]
    Arabic = 'a' * 1000 + 'r',

    [FieldSettings("Aragonese", Description = "an", ResourceType = typeof(Translations.Enum.Language))]
    Aragonese = 'a' * 1000 + 'n',

    [FieldSettings("Armenian", Description = "hy", ResourceType = typeof(Translations.Enum.Language))]
    Armenian = 'h' * 1000 + 'y',

    [FieldSettings("Assamese", Description = "as", ResourceType = typeof(Translations.Enum.Language))]
    Assamese = 'a' * 1000 + 's',

    [FieldSettings("Avaric", Description = "av", ResourceType = typeof(Translations.Enum.Language))]
    Avaric = 'a' * 1000 + 'v',

    [FieldSettings("Avestan", Description = "ae", ResourceType = typeof(Translations.Enum.Language))]
    Avestan = 'a' * 1000 + 'e',

    [FieldSettings("Aymara", Description = "ay", ResourceType = typeof(Translations.Enum.Language))]
    Aymara = 'a' * 1000 + 'y',

    [FieldSettings("Azerbaijani", Description = "az", ResourceType = typeof(Translations.Enum.Language))]
    Azerbaijani = 'a' * 1000 + 'z',

    [FieldSettings("Bambara", Description = "bm", ResourceType = typeof(Translations.Enum.Language))]
    Bambara = 'b' * 1000 + 'm',

    [FieldSettings("Bashkir", Description = "ba", ResourceType = typeof(Translations.Enum.Language))]
    Bashkir = 'b' * 1000 + 'a',

    [FieldSettings("Basque", Description = "eu", ResourceType = typeof(Translations.Enum.Language))]
    Basque = 'e' * 1000 + 'u',

    [FieldSettings("Belarusian", Description = "be", ResourceType = typeof(Translations.Enum.Language))]
    Belarusian = 'b' * 1000 + 'e',

    [FieldSettings("Bengali", Description = "bn", ResourceType = typeof(Translations.Enum.Language))]
    Bengali = 'b' * 1000 + 'n',

    [FieldSettings("Bislama", Description = "bi", ResourceType = typeof(Translations.Enum.Language))]
    Bislama = 'b' * 1000 + 'i',

    [FieldSettings("Bosnian", Description = "bs", ResourceType = typeof(Translations.Enum.Language))]
    Bosnian = 'b' * 1000 + 's',

    [FieldSettings("Breton", Description = "br", ResourceType = typeof(Translations.Enum.Language))]
    Breton = 'b' * 1000 + 'r',

    [FieldSettings("Bulgarian", Description = "bg", ResourceType = typeof(Translations.Enum.Language))]
    Bulgarian = 'b' * 1000 + 'g',

    [FieldSettings("Burmese", Description = "my", ResourceType = typeof(Translations.Enum.Language))]
    Burmese = 'm' * 1000 + 'y',

    [FieldSettings("Catalan", Description = "ca", ResourceType = typeof(Translations.Enum.Language))]
    Catalan = 'c' * 1000 + 'a',

    [FieldSettings("CentralKhmer", Description = "km", ResourceType = typeof(Translations.Enum.Language))]
    CentralKhmer = 'k' * 1000 + 'm',

    [FieldSettings("Chamorro", Description = "ch", ResourceType = typeof(Translations.Enum.Language))]
    Chamorro = 'c' * 1000 + 'h',

    [FieldSettings("Chechen", Description = "ce", ResourceType = typeof(Translations.Enum.Language))]
    Chechen = 'c' * 1000 + 'e',

    [FieldSettings("Chichewa", Description = "ny", ResourceType = typeof(Translations.Enum.Language))]
    Chichewa = 'n' * 1000 + 'y',

    [FieldSettings("Chinese", Description = "zh", ResourceType = typeof(Translations.Enum.Language))]
    Chinese = 'z' * 1000 + 'h',

    [FieldSettings("ChurchSlavonic", Description = "cu", ResourceType = typeof(Translations.Enum.Language))]
    ChurchSlavonic = 'c' * 1000 + 'u',

    [FieldSettings("Chuvash", Description = "cv", ResourceType = typeof(Translations.Enum.Language))]
    Chuvash = 'c' * 1000 + 'v',

    [FieldSettings("Cornish", Description = "kw", ResourceType = typeof(Translations.Enum.Language))]
    Cornish = 'k' * 1000 + 'w',

    [FieldSettings("Corsican", Description = "co", ResourceType = typeof(Translations.Enum.Language))]
    Corsican = 'c' * 1000 + 'o',

    [FieldSettings("Cree", Description = "cr", ResourceType = typeof(Translations.Enum.Language))]
    Cree = 'c' * 1000 + 'r',

    [FieldSettings("Croatian", Description = "hr", ResourceType = typeof(Translations.Enum.Language))]
    Croatian = 'h' * 1000 + 'r',

    [FieldSettings("Czech", Description = "cs", ResourceType = typeof(Translations.Enum.Language))]
    Czech = 'c' * 1000 + 's',

    [FieldSettings("Danish", Description = "da", ResourceType = typeof(Translations.Enum.Language))]
    Danish = 'd' * 1000 + 'a',

    [FieldSettings("Divehi", Description = "dv", ResourceType = typeof(Translations.Enum.Language))]
    Divehi = 'd' * 1000 + 'v',

    [FieldSettings("Dutch", Description = "nl", ResourceType = typeof(Translations.Enum.Language))]
    Dutch = 'n' * 1000 + 'l',

    [FieldSettings("Dzongkha", Description = "dz", ResourceType = typeof(Translations.Enum.Language))]
    Dzongkha = 'd' * 1000 + 'z',

    [FieldSettings("English", Description = "en", Group = "site", ResourceType = typeof(Translations.Enum.Language))]
    English = 'e' * 1000 + 'n',

    [FieldSettings("Esperanto", Description = "eo", ResourceType = typeof(Translations.Enum.Language))]
    Esperanto = 'e' * 1000 + 'o',

    [FieldSettings("Estonian", Description = "et", ResourceType = typeof(Translations.Enum.Language))]
    Estonian = 'e' * 1000 + 't',

    [FieldSettings("Ewe", Description = "ee", ResourceType = typeof(Translations.Enum.Language))]
    Ewe = 'e' * 1000 + 'e',

    [FieldSettings("Faroese", Description = "fo", ResourceType = typeof(Translations.Enum.Language))]
    Faroese = 'f' * 1000 + 'o',

    [FieldSettings("Fijian", Description = "fj", ResourceType = typeof(Translations.Enum.Language))]
    Fijian = 'f' * 1000 + 'j',

    [FieldSettings("Finnish", Description = "fi", ResourceType = typeof(Translations.Enum.Language))]
    Finnish = 'f' * 1000 + 'i',

    [FieldSettings("French", Description = "fr", ResourceType = typeof(Translations.Enum.Language))]
    French = 'f' * 1000 + 'r',

    [FieldSettings("Fulah", Description = "ff", ResourceType = typeof(Translations.Enum.Language))]
    Fulah = 'f' * 1000 + 'f',

    [FieldSettings("Gaelic", Description = "gd", ResourceType = typeof(Translations.Enum.Language))]
    Gaelic = 'g' * 1000 + 'd',

    [FieldSettings("Galician", Description = "gl", ResourceType = typeof(Translations.Enum.Language))]
    Galician = 'g' * 1000 + 'l',

    [FieldSettings("Ganda", Description = "lg", ResourceType = typeof(Translations.Enum.Language))]
    Ganda = 'l' * 1000 + 'g',

    [FieldSettings("Georgian", Description = "ka", ResourceType = typeof(Translations.Enum.Language))]
    Georgian = 'k' * 1000 + 'a',

    [FieldSettings("German", Description = "de", ResourceType = typeof(Translations.Enum.Language))]
    German = 'd' * 1000 + 'e',

    [FieldSettings("Greek", Description = "el", ResourceType = typeof(Translations.Enum.Language))]
    Greek = 'e' * 1000 + 'l',

    [FieldSettings("Guarani", Description = "gn", ResourceType = typeof(Translations.Enum.Language))]
    Guarani = 'g' * 1000 + 'n',

    [FieldSettings("Gujarati", Description = "gu", ResourceType = typeof(Translations.Enum.Language))]
    Gujarati = 'g' * 1000 + 'u',

    [FieldSettings("Haitian", Description = "ht", ResourceType = typeof(Translations.Enum.Language))]
    Haitian = 'h' * 1000 + 't',

    [FieldSettings("Hausa", Description = "ha", ResourceType = typeof(Translations.Enum.Language))]
    Hausa = 'h' * 1000 + 'a',

    [FieldSettings("Hebrew", Description = "he", ResourceType = typeof(Translations.Enum.Language))]
    Hebrew = 'h' * 1000 + 'e',

    [FieldSettings("Herero", Description = "hz", ResourceType = typeof(Translations.Enum.Language))]
    Herero = 'h' * 1000 + 'z',

    [FieldSettings("Hindi", Description = "hi", ResourceType = typeof(Translations.Enum.Language))]
    Hindi = 'h' * 1000 + 'i',

    [FieldSettings("HiriMotu", Description = "ho", ResourceType = typeof(Translations.Enum.Language))]
    HiriMotu = 'h' * 1000 + 'o',

    [FieldSettings("Hungarian", Description = "hu", ResourceType = typeof(Translations.Enum.Language))]
    Hungarian = 'h' * 1000 + 'u',

    [FieldSettings("Icelandic", Description = "is", ResourceType = typeof(Translations.Enum.Language))]
    Icelandic = 'i' * 1000 + 's',

    [FieldSettings("Ido", Description = "io", ResourceType = typeof(Translations.Enum.Language))]
    Ido = 'i' * 1000 + 'o',

    [FieldSettings("Igbo", Description = "ig", ResourceType = typeof(Translations.Enum.Language))]
    Igbo = 'i' * 1000 + 'g',

    [FieldSettings("Indonesian", Description = "id", ResourceType = typeof(Translations.Enum.Language))]
    Indonesian = 'i' * 1000 + 'd',

    [FieldSettings("Interlingua", Description = "ia", ResourceType = typeof(Translations.Enum.Language))]
    Interlingua = 'i' * 1000 + 'a',

    [FieldSettings("Interlingue", Description = "ie", ResourceType = typeof(Translations.Enum.Language))]
    Interlingue = 'i' * 1000 + 'e',

    [FieldSettings("Inuktitut", Description = "iu", ResourceType = typeof(Translations.Enum.Language))]
    Inuktitut = 'i' * 1000 + 'u',

    [FieldSettings("Inupiaq", Description = "ik", ResourceType = typeof(Translations.Enum.Language))]
    Inupiaq = 'i' * 1000 + 'k',

    [FieldSettings("Irish", Description = "ga", ResourceType = typeof(Translations.Enum.Language))]
    Irish = 'g' * 1000 + 'a',

    [FieldSettings("Italian", Description = "it", ResourceType = typeof(Translations.Enum.Language))]
    Italian = 'i' * 1000 + 't',

    [FieldSettings("Japanese", Description = "ja", ResourceType = typeof(Translations.Enum.Language))]
    Japanese = 'j' * 1000 + 'a',

    [FieldSettings("Javanese", Description = "jv", ResourceType = typeof(Translations.Enum.Language))]
    Javanese = 'j' * 1000 + 'v',

    [FieldSettings("Kalaallisut", Description = "kl", ResourceType = typeof(Translations.Enum.Language))]
    Kalaallisut = 'k' * 1000 + 'l',

    [FieldSettings("Kannada", Description = "kn", ResourceType = typeof(Translations.Enum.Language))]
    Kannada = 'k' * 1000 + 'n',

    [FieldSettings("Kanuri", Description = "kr", ResourceType = typeof(Translations.Enum.Language))]
    Kanuri = 'k' * 1000 + 'r',

    [FieldSettings("Kashmiri", Description = "ks", ResourceType = typeof(Translations.Enum.Language))]
    Kashmiri = 'k' * 1000 + 's',

    [FieldSettings("Kazakh", Description = "kk", ResourceType = typeof(Translations.Enum.Language))]
    Kazakh = 'k' * 1000 + 'k',

    [FieldSettings("Kikuyu", Description = "ki", ResourceType = typeof(Translations.Enum.Language))]
    Kikuyu = 'k' * 1000 + 'i',

    [FieldSettings("Kinyarwanda", Description = "rw", ResourceType = typeof(Translations.Enum.Language))]
    Kinyarwanda = 'r' * 1000 + 'w',

    [FieldSettings("Kirghiz", Description = "ky", ResourceType = typeof(Translations.Enum.Language))]
    Kirghiz = 'k' * 1000 + 'y',

    [FieldSettings("Komi", Description = "kv", ResourceType = typeof(Translations.Enum.Language))]
    Komi = 'k' * 1000 + 'v',

    [FieldSettings("Kongo", Description = "kg", ResourceType = typeof(Translations.Enum.Language))]
    Kongo = 'k' * 1000 + 'g',

    [FieldSettings("Korean", Description = "ko", ResourceType = typeof(Translations.Enum.Language))]
    Korean = 'k' * 1000 + 'o',

    [FieldSettings("Kuanyama", Description = "kj", ResourceType = typeof(Translations.Enum.Language))]
    Kuanyama = 'k' * 1000 + 'j',

    [FieldSettings("Kurdish", Description = "ku", ResourceType = typeof(Translations.Enum.Language))]
    Kurdish = 'k' * 1000 + 'u',

    [FieldSettings("Lao", Description = "lo", ResourceType = typeof(Translations.Enum.Language))]
    Lao = 'l' * 1000 + 'o',

    [FieldSettings("Latin", Description = "la", ResourceType = typeof(Translations.Enum.Language))]
    Latin = 'l' * 1000 + 'a',

    [FieldSettings("Latvian", Description = "lv", ResourceType = typeof(Translations.Enum.Language))]
    Latvian = 'l' * 1000 + 'v',

    [FieldSettings("Limburgan", Description = "li", ResourceType = typeof(Translations.Enum.Language))]
    Limburgan = 'l' * 1000 + 'i',

    [FieldSettings("Lingala", Description = "ln", ResourceType = typeof(Translations.Enum.Language))]
    Lingala = 'l' * 1000 + 'n',

    [FieldSettings("Lithuanian", Description = "lt", ResourceType = typeof(Translations.Enum.Language))]
    Lithuanian = 'l' * 1000 + 't',

    [FieldSettings("LubaKatanga", Description = "lu", ResourceType = typeof(Translations.Enum.Language))]
    LubaKatanga = 'l' * 1000 + 'u',

    [FieldSettings("Luxembourgish", Description = "lb", ResourceType = typeof(Translations.Enum.Language))]
    Luxembourgish = 'l' * 1000 + 'b',

    [FieldSettings("Macedonian", Description = "mk", ResourceType = typeof(Translations.Enum.Language))]
    Macedonian = 'm' * 1000 + 'k',

    [FieldSettings("Malagasy", Description = "mg", ResourceType = typeof(Translations.Enum.Language))]
    Malagasy = 'm' * 1000 + 'g',

    [FieldSettings("Malay", Description = "ms", ResourceType = typeof(Translations.Enum.Language))]
    Malay = 'm' * 1000 + 's',

    [FieldSettings("Malayalam", Description = "ml", ResourceType = typeof(Translations.Enum.Language))]
    Malayalam = 'm' * 1000 + 'l',

    [FieldSettings("Maltese", Description = "mt", ResourceType = typeof(Translations.Enum.Language))]
    Maltese = 'm' * 1000 + 't',

    [FieldSettings("Manx", Description = "gv", ResourceType = typeof(Translations.Enum.Language))]
    Manx = 'g' * 1000 + 'v',

    [FieldSettings("Maori", Description = "mi", ResourceType = typeof(Translations.Enum.Language))]
    Maori = 'm' * 1000 + 'i',

    [FieldSettings("Marathi", Description = "mr", ResourceType = typeof(Translations.Enum.Language))]
    Marathi = 'm' * 1000 + 'r',

    [FieldSettings("Marshallese", Description = "mh", ResourceType = typeof(Translations.Enum.Language))]
    Marshallese = 'm' * 1000 + 'h',

    [FieldSettings("Mongolian", Description = "mn", ResourceType = typeof(Translations.Enum.Language))]
    Mongolian = 'm' * 1000 + 'n',

    [FieldSettings("Nauru", Description = "na", ResourceType = typeof(Translations.Enum.Language))]
    Nauru = 'n' * 1000 + 'a',

    [FieldSettings("Navajo", Description = "nv", ResourceType = typeof(Translations.Enum.Language))]
    Navajo = 'n' * 1000 + 'v',

    [FieldSettings("Ndonga", Description = "ng", ResourceType = typeof(Translations.Enum.Language))]
    Ndonga = 'n' * 1000 + 'g',

    [FieldSettings("Nepali", Description = "ne", ResourceType = typeof(Translations.Enum.Language))]
    Nepali = 'n' * 1000 + 'e',

    [FieldSettings("NorthernSami", Description = "se", ResourceType = typeof(Translations.Enum.Language))]
    NorthernSami = 's' * 1000 + 'e',

    [FieldSettings("NorthNdebele", Description = "nd", ResourceType = typeof(Translations.Enum.Language))]
    NorthNdebele = 'n' * 1000 + 'd',

    [FieldSettings("Norwegian", Description = "no", ResourceType = typeof(Translations.Enum.Language))]
    Norwegian = 'n' * 1000 + 'o',

    [FieldSettings("NorwegianBokmål", Description = "nb", ResourceType = typeof(Translations.Enum.Language))]
    NorwegianBokmål = 'n' * 1000 + 'b',

    [FieldSettings("NorwegianNynorsk", Description = "nn", ResourceType = typeof(Translations.Enum.Language))]
    NorwegianNynorsk = 'n' * 1000 + 'n',

    [FieldSettings("Occitan", Description = "oc", ResourceType = typeof(Translations.Enum.Language))]
    Occitan = 'o' * 1000 + 'c',

    [FieldSettings("Ojibwa", Description = "oj", ResourceType = typeof(Translations.Enum.Language))]
    Ojibwa = 'o' * 1000 + 'j',

    [FieldSettings("Oriya", Description = "or", ResourceType = typeof(Translations.Enum.Language))]
    Oriya = 'o' * 1000 + 'r',

    [FieldSettings("Oromo", Description = "om", ResourceType = typeof(Translations.Enum.Language))]
    Oromo = 'o' * 1000 + 'm',

    [FieldSettings("Ossetian", Description = "os", ResourceType = typeof(Translations.Enum.Language))]
    Ossetian = 'o' * 1000 + 's',

    [FieldSettings("Pali", Description = "pi", ResourceType = typeof(Translations.Enum.Language))]
    Pali = 'p' * 1000 + 'i',

    [FieldSettings("Pashto", Description = "ps", ResourceType = typeof(Translations.Enum.Language))]
    Pashto = 'p' * 1000 + 's',

    [FieldSettings("Persian", Description = "fa", ResourceType = typeof(Translations.Enum.Language))]
    Persian = 'f' * 1000 + 'a',

    [FieldSettings("Polish", Description = "pl", ResourceType = typeof(Translations.Enum.Language))]
    Polish = 'p' * 1000 + 'l',

    [FieldSettings("Portuguese", Description = "pt", Group = "site", ResourceType = typeof(Translations.Enum.Language))]
    Portuguese = 'p' * 1000 + 't',

    [FieldSettings("Punjabi", Description = "pa", ResourceType = typeof(Translations.Enum.Language))]
    Punjabi = 'p' * 1000 + 'a',

    [FieldSettings("Quechua", Description = "qu", ResourceType = typeof(Translations.Enum.Language))]
    Quechua = 'q' * 1000 + 'u',

    [FieldSettings("Romanian", Description = "ro", ResourceType = typeof(Translations.Enum.Language))]
    Romanian = 'r' * 1000 + 'o',

    [FieldSettings("Romansh", Description = "rm", ResourceType = typeof(Translations.Enum.Language))]
    Romansh = 'r' * 1000 + 'm',

    [FieldSettings("Rundi", Description = "rn", ResourceType = typeof(Translations.Enum.Language))]
    Rundi = 'r' * 1000 + 'n',

    [FieldSettings("Russian", Description = "ru", ResourceType = typeof(Translations.Enum.Language))]
    Russian = 'r' * 1000 + 'u',

    [FieldSettings("Samoan", Description = "sm", ResourceType = typeof(Translations.Enum.Language))]
    Samoan = 's' * 1000 + 'm',

    [FieldSettings("Sango", Description = "sg", ResourceType = typeof(Translations.Enum.Language))]
    Sango = 's' * 1000 + 'g',

    [FieldSettings("Sanskrit", Description = "sa", ResourceType = typeof(Translations.Enum.Language))]
    Sanskrit = 's' * 1000 + 'a',

    [FieldSettings("Sardinian", Description = "sc", ResourceType = typeof(Translations.Enum.Language))]
    Sardinian = 's' * 1000 + 'c',

    [FieldSettings("Serbian", Description = "sr", ResourceType = typeof(Translations.Enum.Language))]
    Serbian = 's' * 1000 + 'r',

    [FieldSettings("Shona", Description = "sn", ResourceType = typeof(Translations.Enum.Language))]
    Shona = 's' * 1000 + 'n',

    [FieldSettings("SichuanYi", Description = "ii", ResourceType = typeof(Translations.Enum.Language))]
    SichuanYi = 'i' * 1000 + 'i',

    [FieldSettings("Sindhi", Description = "sd", ResourceType = typeof(Translations.Enum.Language))]
    Sindhi = 's' * 1000 + 'd',

    [FieldSettings("Sinhala", Description = "si", ResourceType = typeof(Translations.Enum.Language))]
    Sinhala = 's' * 1000 + 'i',

    [FieldSettings("Slovak", Description = "sk", ResourceType = typeof(Translations.Enum.Language))]
    Slovak = 's' * 1000 + 'k',

    [FieldSettings("Slovenian", Description = "sl", ResourceType = typeof(Translations.Enum.Language))]
    Slovenian = 's' * 1000 + 'l',

    [FieldSettings("Somali", Description = "so", ResourceType = typeof(Translations.Enum.Language))]
    Somali = 's' * 1000 + 'o',

    [FieldSettings("SouthernSotho", Description = "st", ResourceType = typeof(Translations.Enum.Language))]
    SouthernSotho = 's' * 1000 + 't',

    [FieldSettings("SouthNdebele", Description = "nr", ResourceType = typeof(Translations.Enum.Language))]
    SouthNdebele = 'n' * 1000 + 'r',

    [FieldSettings("Spanish", Description = "es", Group = "site", ResourceType = typeof(Translations.Enum.Language))]
    Spanish = 'e' * 1000 + 's',

    [FieldSettings("Sundanese", Description = "su", ResourceType = typeof(Translations.Enum.Language))]
    Sundanese = 's' * 1000 + 'u',

    [FieldSettings("Swahili", Description = "sw", ResourceType = typeof(Translations.Enum.Language))]
    Swahili = 's' * 1000 + 'w',

    [FieldSettings("Swati", Description = "ss", ResourceType = typeof(Translations.Enum.Language))]
    Swati = 's' * 1000 + 's',

    [FieldSettings("Swedish", Description = "sv", ResourceType = typeof(Translations.Enum.Language))]
    Swedish = 's' * 1000 + 'v',

    [FieldSettings("Tagalog", Description = "tl", ResourceType = typeof(Translations.Enum.Language))]
    Tagalog = 't' * 1000 + 'l',

    [FieldSettings("Tahitian", Description = "ty", ResourceType = typeof(Translations.Enum.Language))]
    Tahitian = 't' * 1000 + 'y',

    [FieldSettings("Tajik", Description = "tg", ResourceType = typeof(Translations.Enum.Language))]
    Tajik = 't' * 1000 + 'g',

    [FieldSettings("Tamil", Description = "ta", ResourceType = typeof(Translations.Enum.Language))]
    Tamil = 't' * 1000 + 'a',

    [FieldSettings("Tatar", Description = "tt", ResourceType = typeof(Translations.Enum.Language))]
    Tatar = 't' * 1000 + 't',

    [FieldSettings("Telugu", Description = "te", ResourceType = typeof(Translations.Enum.Language))]
    Telugu = 't' * 1000 + 'e',

    [FieldSettings("Thai", Description = "th", ResourceType = typeof(Translations.Enum.Language))]
    Thai = 't' * 1000 + 'h',

    [FieldSettings("Tibetan", Description = "bo", ResourceType = typeof(Translations.Enum.Language))]
    Tibetan = 'b' * 1000 + 'o',

    [FieldSettings("Tigrinya", Description = "ti", ResourceType = typeof(Translations.Enum.Language))]
    Tigrinya = 't' * 1000 + 'i',

    [FieldSettings("Tonga", Description = "to", ResourceType = typeof(Translations.Enum.Language))]
    Tonga = 't' * 1000 + 'o',

    [FieldSettings("Tsonga", Description = "ts", ResourceType = typeof(Translations.Enum.Language))]
    Tsonga = 't' * 1000 + 's',

    [FieldSettings("Tswana", Description = "tn", ResourceType = typeof(Translations.Enum.Language))]
    Tswana = 't' * 1000 + 'n',

    [FieldSettings("Turkish", Description = "tr", ResourceType = typeof(Translations.Enum.Language))]
    Turkish = 't' * 1000 + 'r',

    [FieldSettings("Turkmen", Description = "tk", ResourceType = typeof(Translations.Enum.Language))]
    Turkmen = 't' * 1000 + 'k',

    [FieldSettings("Twi", Description = "tw", ResourceType = typeof(Translations.Enum.Language))]
    Twi = 't' * 1000 + 'w',

    [FieldSettings("Uighur", Description = "ug", ResourceType = typeof(Translations.Enum.Language))]
    Uighur = 'u' * 1000 + 'g',

    [FieldSettings("Ukrainian", Description = "uk", ResourceType = typeof(Translations.Enum.Language))]
    Ukrainian = 'u' * 1000 + 'k',

    [FieldSettings("Urdu", Description = "ur", ResourceType = typeof(Translations.Enum.Language))]
    Urdu = 'u' * 1000 + 'r',

    [FieldSettings("Uzbek", Description = "uz", ResourceType = typeof(Translations.Enum.Language))]
    Uzbek = 'u' * 1000 + 'z',

    [FieldSettings("Venda", Description = "ve", ResourceType = typeof(Translations.Enum.Language))]
    Venda = 'v' * 1000 + 'e',

    [FieldSettings("Vietnamese", Description = "vi", ResourceType = typeof(Translations.Enum.Language))]
    Vietnamese = 'v' * 1000 + 'i',

    [FieldSettings("Volapük", Description = "vo", ResourceType = typeof(Translations.Enum.Language))]
    Volapük = 'v' * 1000 + 'o',

    [FieldSettings("Walloon", Description = "wa", ResourceType = typeof(Translations.Enum.Language))]
    Walloon = 'w' * 1000 + 'a',

    [FieldSettings("Welsh", Description = "cy", ResourceType = typeof(Translations.Enum.Language))]
    Welsh = 'c' * 1000 + 'y',

    [FieldSettings("WesternFrisian", Description = "fy", ResourceType = typeof(Translations.Enum.Language))]
    WesternFrisian = 'f' * 1000 + 'y',

    [FieldSettings("Wolof", Description = "wo", ResourceType = typeof(Translations.Enum.Language))]
    Wolof = 'w' * 1000 + 'o',

    [FieldSettings("Xhosa", Description = "xh", ResourceType = typeof(Translations.Enum.Language))]
    Xhosa = 'x' * 1000 + 'h',

    [FieldSettings("Yiddish", Description = "yi", ResourceType = typeof(Translations.Enum.Language))]
    Yiddish = 'y' * 1000 + 'i',

    [FieldSettings("Yoruba", Description = "yo", ResourceType = typeof(Translations.Enum.Language))]
    Yoruba = 'y' * 1000 + 'o',

    [FieldSettings("Zhuang", Description = "za", ResourceType = typeof(Translations.Enum.Language))]
    Zhuang = 'z' * 1000 + 'a',

    [FieldSettings("Zulu", Description = "zu", ResourceType = typeof(Translations.Enum.Language))]
    Zulu = 'z' * 1000 + 'u'
}