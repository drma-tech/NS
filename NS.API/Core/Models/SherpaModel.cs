namespace NS.API.Core.Models
{
    public class Action
    {
        public string type { get; set; }
        public string title { get; set; }
        public string url { get; set; }
        public string provider { get; set; }
        public string intent { get; set; }
        public string productId { get; set; }
        public Product product { get; set; }
    }

    public class ApplicationDeadline
    {
        public string type { get; set; }
        public int value { get; set; }
        public string text { get; set; }
        public string unit { get; set; }
        public string label { get; set; }
    }

    public class Arrival
    {
        public string date { get; set; }
        public string time { get; set; }
    }

    public class Attributes
    {
        public Traveller traveller { get; set; }
        public string locale { get; set; }
        public List<TravelNode> travelNodes { get; set; }
        public string headline { get; set; }
        public List<InformationGroup> informationGroups { get; set; }
        public string currency { get; set; }
        public string travelOpenness { get; set; }
        public string category { get; set; }
        public string subCategory { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public List<string> more { get; set; }
        public List<Action> actions { get; set; }
        public List<Source> sources { get; set; }
        public DateTime lastUpdatedAt { get; set; }
        public DateTime createdAt { get; set; }
        public object startDate { get; set; }
        public object endDate { get; set; }
        public string enforcement { get; set; }
        public List<string> documentTypes { get; set; }
        public List<string> tags { get; set; }
        public List<object> travelPurposes { get; set; }
        public object lengthOfStay { get; set; }
        public List<Included> included { get; set; }
        public Icon icon { get; set; }
        //public Pricing pricing { get; set; }
        public List<object> notices { get; set; }
        public string programId { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public string subType { get; set; }
        public string shortName { get; set; }
        public string imageUrl { get; set; }
        public string name { get; set; }
        public string shortDescription { get; set; }
        public List<EligibleNationality> eligibleNationalities { get; set; }
        public List<Destination> destinations { get; set; }
        public NumberOfEntries numberOfEntries { get; set; }
        public Validity validity { get; set; }
        public ProcessingTime processingTime { get; set; }
        public ProcessingWindow processingWindow { get; set; }
        public EarliestApplyDate earliestApplyDate { get; set; }
        public ApplicationDeadline applicationDeadline { get; set; }
        public List<Prerequisite> prerequisites { get; set; }
        public Provider provider { get; set; }
        public List<ValueProposition> valuePropositions { get; set; }
        public bool? hasVariants { get; set; }
        public bool? isVariant { get; set; }
        public int? applicationPrefillPercentage { get; set; }
        public string productType { get; set; }
    }

    public class Breakdown
    {
        public string type { get; set; }
        //public Price price { get; set; }
        public string label { get; set; }
        public string description { get; set; }
        //public PriceInUSD priceInUSD { get; set; }
    }

    public class Data
    {
        public string id { get; set; }
        public string type { get; set; }
        public Attributes attributes { get; set; }
        //public Relationships relationships { get; set; }
    }

    public class Departure
    {
        public string date { get; set; }
        public string time { get; set; }
    }

    public class Destination
    {
        public string value { get; set; }
        public string label { get; set; }
    }

    public class EarliestApplyDate
    {
        public int value { get; set; }
        public string unit { get; set; }
        public string label { get; set; }
    }

    public class EligibleNationality
    {
        public string value { get; set; }
        public string label { get; set; }
    }

    public class Grouping
    {
        public string name { get; set; }
        public string enforcement { get; set; }
        //public List<Datum> data { get; set; }
    }

    public class Icon
    {
        public string name { get; set; }
    }

    public class Included
    {
        public string id { get; set; }
        public string type { get; set; }
        public Attributes attributes { get; set; }
        //public Relationships relationships { get; set; }
        public string code { get; set; }
        public string text { get; set; }
        public string locationType { get; set; }
    }

    public class InformationGroup
    {
        public string name { get; set; }
        public string type { get; set; }
        public string tooltip { get; set; }
        public List<Grouping> groupings { get; set; }
        public string enforcement { get; set; }
        public string headline { get; set; }
    }

    //public class Location
    //{
    //    public Data data { get; set; }
    //}

    //public class Meta
    //{
    //    public string copyright { get; set; }
    //    public string version { get; set; }
    //    public int count { get; set; }
    //}

    public class NumberOfEntries
    {
        public string value { get; set; }
        public string label { get; set; }
    }

    public class Prerequisite
    {
        public string title { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public string value { get; set; }
    }

    //public class Price
    //{
    //    public double value { get; set; }
    //    public string currency { get; set; }
    //}

    //public class PriceBreakdown
    //{
    //    public string type { get; set; }
    //    public string name { get; set; }
    //    public Price price { get; set; }
    //}

    //public class PriceInUSD
    //{
    //    public double value { get; set; }
    //    public string currency { get; set; }
    //}

    //public class Pricing
    //{
    //    public List<Breakdown> breakdown { get; set; }
    //    public Price price { get; set; }
    //    public PriceInUSD priceInUSD { get; set; }
    //}

    //public class Procedures
    //{
    //    public List<Datum> data { get; set; }
    //    public Meta meta { get; set; }
    //}

    public class ProcessingTime
    {
        public int value { get; set; }
        public string unit { get; set; }
        public string label { get; set; }
    }

    public class ProcessingWindow
    {
        public int value { get; set; }
        public string unit { get; set; }
        public string label { get; set; }
    }

    public class Product
    {
        public string productId { get; set; }
        public string programId { get; set; }
        public string name { get; set; }
        //public Price price { get; set; }
        //public List<PriceBreakdown> priceBreakdown { get; set; }
        public List<string> destinations { get; set; }
        public List<string> travelPurposes { get; set; }
        public Times times { get; set; }
    }

    //public class Products
    //{
    //    public List<Datum> data { get; set; }
    //    public Meta meta { get; set; }
    //}

    public class Provider
    {
        public string value { get; set; }
        public string label { get; set; }
        public string description { get; set; }
    }

    //public class Relationships
    //{
    //    public Restrictions restrictions { get; set; }
    //    public Procedures procedures { get; set; }
    //    public Products products { get; set; }
    //    public Location location { get; set; }
    //}

    public class Restrictions
    {
        public List<object> data { get; set; }
        //public Meta meta { get; set; }
    }

    public class SherpaModel
    {
        //public Meta meta { get; set; }
        public Data data { get; set; }
        public List<Included> included { get; set; }
    }

    public class Source
    {
        public string type { get; set; }
        public string title { get; set; }
        public string url { get; set; }
    }

    public class Times
    {
        public ApplicationDeadline applicationDeadline { get; set; }
    }

    public class Traveller
    {
        public List<string> passports { get; set; }
        public List<Vaccination> vaccinations { get; set; }
        public List<string> travelPurposes { get; set; }
    }

    public class TravelNode
    {
        public string type { get; set; }
        public Departure departure { get; set; }
        public string locationCode { get; set; }
        public string locationName { get; set; }
        public Arrival arrival { get; set; }
    }

    public class Vaccination
    {
        public string type { get; set; }
        public string status { get; set; }
    }

    public class Validity
    {
        public int value { get; set; }
        public string unit { get; set; }
        public string startsFrom { get; set; }
        public string label { get; set; }
    }

    public class ValueProposition
    {
        public string label { get; set; }
        public string status { get; set; }
        public string value { get; set; }
    }
}