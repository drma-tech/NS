namespace NS.Shared.Models.Country
{
    public class CountryData() : ProtectedMainDocument(DocumentType.Country)
    {
        //description
        //https://en.wikivoyage.org/api/rest_v1/page/summary/United_States_of_America

        //translations

        //China
        //United States
        //Germany
        //United Kingdom
        //France
        //Australia
        //Canada
        //Russia
        //Italy
        //India

        #region Scores

        //Society and Government (100)

        public int? CorruptionScore { get; set; }
        public int? HDI { get; set; }
        public int? DMDemocracyIndex { get; set; }
        public DMClassification? DMClassification { get; set; }
        public int? EconomistDemocracyIndex { get; set; }
        public EconomistRegimeType? EconomistRegimeType { get; set; }
        public int? FreedomExpressionIndex { get; set; }
        public int? FreedomScore { get; set; }
        public int? CensorshipIndex { get; set; }
        public int? HappinessIndex { get; set; }
        //LGBTQ Equality Index //https://www.equaldex.com/equality-index
        //Global Gender Gap Report 2025 //https://reports.weforum.org/docs/WEF_GGGR_2025.pdf (extracted to json)
        //RuleOfLawIndex(World Justice Project)
        //PressFreedomIndex //https://rsf.org/en/index
        //https://www.transparency.org/en/cpi/2023

        //Economy (200)

        public bool OECD { get; set; } = false;
        public decimal? GDP_PPP { get; set; } //pra quem ganha e gasta na moeda interna
        public decimal? GDP_Nominal { get; set; } //pra quem ganha em moeda externa, investe em outro pais ou simplesmente quer comparar o pais a nivel global
        public int? EconomicFreedomIndex { get; set; }
        //GiniIndex(World Bank – inequality measure) //https://data.worldbank.org/indicator/SI.POV.GINI //https://worldpopulationreview.com/country-rankings/gini-coefficient-by-country
        //CompetitivenessIndex(World Economic Forum – Global Competitiveness Report) //https://en.wikipedia.org/wiki/WEF_Global_Competitiveness_Report
        //EaseOfDoingBusiness(World Bank — OBS: descontinuado em 2021, mas substituído pelo Business Ready Project) o novo, tem poucos paises, o velho nao tem um index (apenas um ranking)
        //https://www.numbeo.com/cost-of-living/

        ////https://wageindicator.org/salary/minimum-wage/minimum-wages-per-country
        ////https://countryeconomy.com/national-minimum-wage
        //public decimal? MinimalWage { get; set; }

        //Security and Peace (300)

        public int? TsaSafetyIndex { get; set; }

        //tsa tips
        public int? NumbeoSafetyIndex { get; set; }

        public int? GlobalTerrorismIndex { get; set; }
        public int? GlobalPeaceIndex { get; set; }
        //CrimeIndex(Numbeo – atualizado 2x/ano) //https://www.numbeo.com/crime/ not necessary - (its just the safet index oposite)
        //ConflictRiskIndex(ACLED datasets ou PRIO conflict data) //https://acleddata.com/platform/weekly-conflict-index (teoricamente atualizado toda semana)

        //Environment and Health (400)

        public int? YaleWaterScore { get; set; }
        public int? NumbeoPollutionIndex { get; set; }
        //AirQualityIndex(IQAir – anual por país) //https://www.iqair.com/us/world-most-polluted-countries
        //ClimateRiskIndex(Germanwatch – Global Climate Risk Index) - mortes por temperatura, nao eh importante.
        //HealthcareIndex(Numbeo ou The Lancet Healthcare Access & Quality Index) //https://www.numbeo.com/health-care/

        //Mobility and Tourism (500)

        public int? VisaFree { get; set; }
        public int? InternationalArrivals { get; set; }

        //TourismCompetitivenessIndex(World Economic Forum – Travel & Tourism Competitiveness Report, bianual mas confiável) //https://www3.weforum.org/docs/WEF_Travel_and_Tourism_Development_Index_2024.pdf
        //AirConnectivityIndex(IATA – yearly data) - pesquisar depois. dificil de achar algo com sentido
        //HotelPriceIndex(Numbeo / HPI from Hotels.com, anual) //whatever, vou usar o preco do numbeo msm

        #endregion Scores

        #region Guide

        public HashSet<TaxiApp> TaxiApps { get; set; } = [];

        //https://www.uber.com/us/en/r/cities/ - 71
        //https://bolt.eu/en/cities/ - 50
        //https://indrive.com/ - 47 (click on the language setting)
        //https://taxi.yandex.com/ - 12 (click on the language settings)
        //https://taximaxim.com/ - 21 //https://en.wikipedia.org/wiki/Taxi_Maxim
        //https://web.didiglobal.com/ - 18 //https://en.wikipedia.org/wiki/DiDi#cite_note-DidiMX-1
        //https://www.careem.com/ - 10
        //https://www.free-now.com/uk/ride/cities/ - 9
        //https://www.grab.com/sg/locations/ - 8
        //https://gozem.co/en/ - 8
        //https://yassir.com/ride-hailing - 5
        //https://help.cabify.com/hc/en-us/articles/115000996089-In-which-cities-can-I-find-Cabify - 6
        //https://www.lyft.com/driver/cities - 2
        //https://www.gojek.com/en-id - 2
        //https://www.olacabs.com/cities - 1

        //https://bobthetravelnerd.com/the-best-ride-hailing-app-in-every-country-on-earth/
        //https://johnnyafrica.com/ride-hailing-apps-in-all-countries/?utm_source=chatgpt.com

        #endregion Guide
    }
}