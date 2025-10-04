using System.Text.Json.Serialization;

namespace NS.API.Core.Models
{
    public class CensorshipData
    {
        public Datasets? datasets { get; set; }
    }

    public class Datasets
    {
        [JsonPropertyName("data-3e748aba9b5322a7e86a208c76e18dff")]
        public List<Data3e748aba9b5322a7e86a208c76e18dff> data3e748aba9b5322a7e86a208c76e18dff { get; set; } = [];
    }

    public class Data3e748aba9b5322a7e86a208c76e18dff
    {
        public int level_0 { get; set; }
        public string? country_name_Diff { get; set; }
        public string? alpha_2_code { get; set; }
        public string? alpha_3_code { get; set; }
        public int id { get; set; }

        [JsonPropertyName("Human Development Index (2021)")]
        public string? HumanDevelopmentIndex2021 { get; set; }

        [JsonPropertyName("IMF GDP per capita (2021)")]
        public string? IMFGDPpercapita2021 { get; set; }

        [JsonPropertyName("Population (2021)")]
        public string? Population2021 { get; set; }

        public int? index { get; set; }
        public int? country_id { get; set; }
        public string? country_text_id { get; set; }
        public string? country_name { get; set; }
        public string? region { get; set; }
        public double? rank { get; set; }

        [JsonPropertyName("Digital Rank")]
        public string? DigitalRank { get; set; }

        [JsonPropertyName("Academic Rank")]
        public string? AcademicRank { get; set; }

        [JsonPropertyName("Media Rank")]
        public string? MediaRank { get; set; }

        public int? Overall_Rank { get; set; }

        [JsonPropertyName("Overall Rank")]
        public string? OverallRank { get; set; }

        [JsonPropertyName("Digital Rank_Ordinal")]
        public int? DigitalRank_Ordinal { get; set; }

        [JsonPropertyName("Academic Rank_Ordinal")]
        public int? AcademicRank_Ordinal { get; set; }

        [JsonPropertyName("Media Rank_Ordinal")]
        public int? MediaRank_Ordinal { get; set; }

        public string? country_name_vis { get; set; }
    }
}