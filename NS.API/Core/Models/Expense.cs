namespace NS.API.Core.Models
{
    public class Expense
    {
        public ExpenseType Type { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? Price { get; set; }
        public decimal? MaxPrice { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            return obj is Expense q && q.Type == Type;
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode();
        }
    }

    public enum PriceType
    {
        [Custom(Name = "Minimum Price")]
        Minimum = 1,

        [Custom(Name = "Average Price")]
        Average = 2,

        [Custom(Name = "Maximum Price")]
        Maximum = 3
    }
}