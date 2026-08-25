using Microsoft.AspNetCore.Components;
using MudBlazor;
using NS.Shared.Models.Country;
using NS.WEB.Modules.Profile;

namespace NS.WEB.Modules.Country
{
    public partial class CountryDetailsPage
    {
        [Parameter] public string Code { get; set; }

        public RenderControlState<RegionData?> RegionDataState { get; set; } = new(null, obj => obj == null);
        public RegionData? RegionData { get; set; }

        public RenderControlState<AllRegions?> AllRegionsState { get; set; } = new(null, obj => obj == null);
        private AllRegions? AllRegions { get; set; }
        private RegionModel? Region { get; set; }

        private AllTaxis? AllTaxis { get; set; }
        private IEnumerable<TaxiModel> FilteredTaxis { get; set; } = [];

        public WishList? WishList { get; set; }

        private IEnumerable<RegionModel> NearbyCountries { get; set; } = [];
        public string? Country { get; set; }

        private static bool IsDesktop => AppStateStatic.Breakpoint > Breakpoint.Xs;

        protected override async Task LoadStaticDataAsync()
        {
            await AllRegionsState.StartLoading.Invoke(null);
            AllRegions = await AllRegionsApi.GetAllRegions(Cts.Token);
            await AllRegionsState.FinishLoading.Invoke(AllRegions);

            AllTaxis = await AllTaxisApi.GetAllTaxis(Cts.Token);
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
            await RegionDataState.StartLoading.Invoke(null);

            RegionData = await RegionsApi.GetRegion(Code, Cts.Token);
            Region = AllRegions?.Items.SingleOrDefault(r => r.code!.Equals(Code, StringComparison.OrdinalIgnoreCase));
            FilteredTaxis = AllTaxis?.Items.Where(t => t.regions?.Contains(Code, StringComparer.OrdinalIgnoreCase) == true) ?? [];
            NearbyCountries = AllRegions?.GetList(Region.continent, Region.subcontinent).Where(p => p.code != Code) ?? [];

            await RegionDataState.FinishLoading.Invoke(RegionData);
        }

        protected override async Task LoadAuthenticatedDataAsync(CancellationToken token)
        {
            WishList = await WishListApi.Get(states: [], token);
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

                WishList ??= await WishListApi.Get(states: [], Cts.Token);

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

        private bool HideFlag(string? code)
        {
            if (string.Equals(Country, "cn", StringComparison.OrdinalIgnoreCase) && string.Equals(code, "tw", StringComparison.OrdinalIgnoreCase))
            {
                return true; //hide Taiwan flag for users from China or when country is unknown
            }

            return false;
        }
    }
}