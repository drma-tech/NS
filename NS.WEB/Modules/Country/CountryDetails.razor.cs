using Microsoft.AspNetCore.Components;
using MudBlazor;
using NS.Shared.Models.Country;
using NS.WEB.Modules.Profile.Components;

namespace NS.WEB.Modules.Country
{
    public partial class CountryDetails
    {
        [Parameter] public required string Code { get; set; }

        public RenderControlState<RegionData> ActionsRegionData { get; set; } = new(obj => obj == null);
        public RegionData? RegionData { get; set; }

        public RenderControlState<AllRegions> ActionsAllRegions { get; set; } = new(obj => obj == null);
        private AllRegions? AllRegions { get; set; }
        private RegionModel? Region => AllRegions?.Items.SingleOrDefault(r => r.code!.Equals(Code, StringComparison.OrdinalIgnoreCase));

        private AllTaxis? AllTaxis { get; set; }
        private IEnumerable<TaxiModel> FilteredTaxis => AllTaxis?.Items.Where(t => t.regions?.Contains(Code, StringComparer.OrdinalIgnoreCase) == true) ?? [];

        public WishList? WishList { get; set; }

        private static bool IsDesktop => AppStateStatic.Breakpoint > Breakpoint.Xs;

        protected override async Task LoadStaticDataAsync()
        {
            await ActionsAllRegions.StartLoading.Invoke(null);
            AllRegions = await LocalJsonApi.GetAllRegions(Cts.Token);
            await ActionsAllRegions.FinishLoading.Invoke(AllRegions);

            AllTaxis = await LocalJsonApi.GetAllTaxis(Cts.Token);
        }

        protected override IReadOnlyList<string?> GetParameterKey()
        {
            return
            [
                Code,
            ];
        }

        protected override async Task LoadParameterDataAsync()
        {
            await ActionsRegionData.StartLoading.Invoke(null);
            RegionData = await RegionsApi.GetRegion(Code, Cts.Token);
            await ActionsRegionData.FinishLoading.Invoke(RegionData);
        }

        protected override async Task LoadAuthenticatedDataAsync(CancellationToken token)
        {
            WishList = await WishListApi.Get(actions: null, token);
        }

        public static Color GetColorRisk(Level? level)
        {
            if (level == Level.Low) return Color.Success;

            if (level == Level.Medium) return Color.Warning;

            if (level == Level.High) return Color.Error;

            return Color.Default;
        }

        public static Color GetColorRestrictions(bool? value, bool warning = false)
        {
            if (value == true)
            {
                if (warning)
                    return Color.Warning;

                return Color.Success;
            }

            if (value == false)
                return Color.Error;

            return Color.Default;
        }

        public static string? GetBoolString(bool? value, bool warning = false)
        {
            if (value == true)
            {
                if (warning)
                    return Translations.Module.Country.MaybeRequirement;

                return Translations.Module.Country.Yes;
            }

            if (value == false) return Translations.Module.Country.No;

            return null;
        }

        public static Color GetColorTax(string? value)
        {
            if (value.NotEmpty()) return Color.Info;

            return Color.Default;
        }

        private static string GetIntIcon(double? value)
        {
            if (value == null) return IconsFA.Solid.Icon("xmark").Font;

            if (value >= 8) return IconsFA.Solid.Icon("face-grin-stars").Font;

            if (value >= 6) return IconsFA.Solid.Icon("face-smile-beam").Font;

            if (value >= 4) return IconsFA.Solid.Icon("face-meh").Font;

            if (value >= 2) return IconsFA.Solid.Icon("face-frown").Font;

            return IconsFA.Solid.Icon("face-dizzy").Font;
        }

        private static Color GetColorIcon(double? value)
        {
            if (value == null) return Color.Default;

            if (value >= 8) return Color.Success;

            if (value >= 6) return Color.Success;

            if (value >= 4) return Color.Warning;

            if (value >= 2) return Color.Error;

            return Color.Error;
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

        private async Task AddWishlist(string code)
        {
            try
            {
                if (!AppStateStatic.IsAuthenticated)
                {
                    await ShowWarning(Translations.Notification.YouMustLogged);
                    return;
                }

                WishList ??= await WishListApi.Get(actions: null, Cts.Token);

                var parameters = new DialogParameters<WishlistPopup>
                {
                    { x => x.AllRegions, AllRegions },
                    { x => x.WishList, WishList },
                    { x => x.Entry, new WishListEntry { RegionCode = code }  },
                };

                await DialogService.ShowAsync<WishlistPopup>("Wishlist", parameters, PopupHelper.Options(MaxWidth.Small));
            }
            catch (Exception ex)
            {
                await ProcessException(ex);
            }
        }

        private async Task Remove()
        {
            try
            {
                if (!AppStateStatic.IsAuthenticated)
                {
                    await ShowWarning(Translations.Notification.YouMustLogged);
                    return;
                }

                WishList = await WishListApi.Remove(Region?.code, Cts.Token);
            }
            catch (Exception ex)
            {
                await ProcessException(ex);
            }
        }
    }
}