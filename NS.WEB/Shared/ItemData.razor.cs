using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace NS.WEB.Shared
{
    public partial class ItemData<TValue>
    {
        [Parameter] public TValue? Value { get; set; }

        /// <summary>
        /// only when the field is a decimal (money value)
        /// </summary>
        [Parameter] public double? Score { get; set; }

        [Parameter] public bool ForceScore { get; set; } = false;
        [Parameter] public bool ShowLabel { get; set; } = true;
        [Parameter] public bool OnlyLabel { get; set; } = false;

        [Parameter] public string? Name { get; set; }
        [Parameter] public string? Placeholder { get; set; }
        [Parameter] public string? Description { get; set; }

        private readonly CultureInfo UsCulture = CultureInfo.CreateSpecificCulture("en-US");

        private static string? GetIntIcon(double? value)
        {
            if (value == null) return IconsFA.Solid.Icon("xmark").Font;

            if (value >= 8) return IconsFA.Solid.Icon("face-grin-stars").Font;

            if (value >= 6) return IconsFA.Solid.Icon("face-smile-beam").Font;

            if (value >= 4) return IconsFA.Solid.Icon("face-meh").Font;

            if (value >= 2) return IconsFA.Solid.Icon("face-frown").Font;

            return IconsFA.Solid.Icon("face-dizzy").Font;
        }

        private static string GetColorStyle(double? value, double min = 0, double max = 10)
        {
            if (value == null) return "color: inherit;";

            var v = value.Value;
            var range = max - min;
            var step = range / 5.0;

            int bucket = (int)((v - min) / step);
            bucket = Math.Clamp(bucket, 0, 4);

            var colors = new[]
            {
                "rgb(255, 63, 95)", //mud-error-text
                "rgb(255, 122, 82)",
                "rgb(255, 181, 69)", //mud-warning-text
                "rgb(173, 192, 88)",
                "rgb(61, 203, 108)", //mud-success-text
            };

            return $"color: {colors[bucket]};";
        }
    }
}