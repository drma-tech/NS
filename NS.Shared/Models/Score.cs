using NS.Shared.Core.Types;

namespace NS.Shared.Models
{
    public class Score(string? id) : GroupDocument(new GroupIdentity(GroupType.Score, id))
    {
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public string? Icon { get; set; }
        public ISet<ScoreDetail> Items { get; set; } = new HashSet<ScoreDetail>();

        protected override object?[] EqualityValues => [Id];
    }

    public class ScoreDetail
    {
        public string? Code { get; set; }
        public double? Value { get; set; }

        public double? GetScore(string id)
        {
            if (string.Equals(id, "visafree", StringComparison.OrdinalIgnoreCase))
            {
                return CalculatePassportIndex();
            }

            return Value;
        }

        public double? CalculatePassportIndex()
        {
            int existingPassports = 200;

            if (Value.HasValue)
            {
                if (Value.Value >= existingPassports)
                {
                    return 10.0;
                }

                var result = Value.Value / existingPassports * 10;
                return Math.Round(result, 1, MidpointRounding.ToEven);
            }
            return null;
        }
    }
}