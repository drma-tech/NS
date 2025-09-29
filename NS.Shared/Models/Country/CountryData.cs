namespace NS.Shared.Models.Country
{
    public class CountryData() : ProtectedMainDocument(DocumentType.Country)
    {
        public int? Population { get; set; }
        public bool UnMember { get; set; } = false;
        public int? VisaFree { get; set; }
        public int? CorruptionScore { get; set; }

        //Very high human development >= 0.800
        //High human development >= 0.700
        //Medium human development >= 0.550
        //Low human development < 0.550
        public float? HDI { get; set; }
        public int? Area { get; set; }
        public bool OECD { get; set; } = false;
        public int? TsaSafetyIndex { get; set; }
        public float? NumbeoSafetyIndex { get; set; }

        //Working Democracy
        //Deficient Democracy
        //Hybrid Regime
        //Moderate Autocracy
        //Hard Autocracy
        //https://www.democracymatrix.com/ranking
        public float? DemocracyIndex { get; set; }

        //Full democracies
        //Flawed democracies
        //Hybrid regimes
        //Authoritarian regimes
        //https://en.wikipedia.org/wiki/The_Economist_Democracy_Index
        public float? EconomistDemocracyIndex { get; set; }

        //https://ourworldindata.org/grapher/freedom-of-expression-index?tab=table
        //https://ourworldindata.org/grapher/freedom-of-expression-index?tab=table&time=2014..2024 (when get historical)
        //original: https://www.v-dem.net/data/the-v-dem-dataset/
        public float? FreedomExpressionIndex { get; set; }

        //https://data.worldhappiness.report/table
        public float? HappinessIndex { get; set; }

        //https://data.worldbank.org/indicator/NY.GDP.PCAP.CD
        public decimal? GPD { get; set; }

        //https://www.heritage.org/index/pages/all-country-scores
        public float? EconomicFreedomIndex { get; set; }

        //https://www.indexmundi.com/facts/indicators/ST.INT.ARVL/rankings
        public int? TourismScore { get; set; }

        //"1: Open",
        //"2: Significantly Open",
        //"3: Partially Open",
        //"4: Partially Narrowed",
        //"5: Significantly Narrowed",
        //"6: Narrowed",
        //"7: Partially Restricted",
        //"8: Significantly Restricted",
        //"9: Heavily Restricted",
        //"10: Closed",
        //https://www.indexoncensorship.org/campaigns/indexindex/
        //download json (map action - view source)
        public int? CensorshipIndex { get; set; }

        //Free
        //Partly Free
        //Not Free
        //https://freedomhouse.org/country/scores
        public int? FreedomScore { get; set; }

        //Safe to drink
        //Not safe to drink
        //https://www.qssupplies.co.uk/worlds-most-dangerous-drinking-water.html
        public float? TapWaterScore { get; set; }

        //https://www.visionofhumanity.org/maps/global-terrorism-index/#/
        public float? GlobalTerrorismIndex { get; set; }

        //https://www.visionofhumanity.org/maps/#/
        public float? GlobalPeaceIndex { get; set; }

        ////https://wageindicator.org/salary/minimum-wage/minimum-wages-per-country
        ////https://countryeconomy.com/national-minimum-wage
        //public decimal? MinimalWage { get; set; }
    }
}