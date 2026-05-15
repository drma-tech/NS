namespace NS.Shared.Core.Helper
{
    public static class DataHelper
    {
        public static string GetResume(this string? text, int count)
        {
            if (string.IsNullOrEmpty(text)) return "";

            return text.Length > count ? string.Concat(text.AsSpan(0, count), "...") : text;
        }

        public static (double minPercentile, double maxPercentile) GetPercentiles(Dictionary<string, double> values)
        {
            var sortedValues = values.Values.OrderBy(v => v).ToList();
            int n = sortedValues.Count;

            int idx5 = (int)Math.Floor(0.05 * (n - 1));
            int idx95 = (int)Math.Floor(0.95 * (n - 1));

            double minPercentile = sortedValues[idx5];
            double maxPercentile = sortedValues[idx95];

            return (minPercentile, maxPercentile);
        }

        public static double? ConvertToScore(double? value, double minPercentile, double maxPercentile, bool higherIsBetter)
        {
            if (!value.HasValue) return null;

            if (higherIsBetter)
            {
                if (value <= minPercentile) return 0.0;
                if (value >= maxPercentile) return 10.0;

                double normalized = (value.Value - minPercentile) / (maxPercentile - minPercentile);
                return Math.Round(normalized * 10, 1);
            }
            else
            {
                if (value <= minPercentile) return 10.0;
                if (value >= maxPercentile) return 0.0;

                double normalized = (maxPercentile - value.Value) / (maxPercentile - minPercentile);
                return Math.Round(normalized * 10, 1);
            }
        }

        public static double CalculateThermalComfortScore(this double value)
        {
            // Comfortable range (human perception sweet spot)
            const double minComfort = 18;
            const double maxComfort = 23;

            // Outside penalties (asymmetric behavior)
            const double coldFactor = 0.04;
            const double hotFactor = 0.04;

            double score;

            // Perfect comfort zone (plateau)
            if (value >= minComfort && value <= maxComfort)
            {
                score = 10;
            }
            else if (value < minComfort)
            {
                double diff = minComfort - value;
                score = 10 - coldFactor * diff * diff;
            }
            else
            {
                double diff = value - maxComfort;
                score = 10 - hotFactor * diff * diff;
            }

            return Math.Round(Math.Clamp(score, 0, 10), 1);
        }
    }
}