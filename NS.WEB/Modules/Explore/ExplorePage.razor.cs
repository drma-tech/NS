using MudBlazor;

namespace NS.WEB.Modules.Explore
{
    public partial class ExplorePage
    {
        public RenderControlState<AllRegions> State { get; set; } = new(obj => obj == null || obj.Items.Empty());
        private AllRegions? AllRegions { get; set; }
        private IEnumerable<RegionModel> FilteredRegions => 
            AllRegions?.GetList(continent, subcontinent).Where(p => !name.NotEmpty() || p.name!.Contains(name, StringComparison.InvariantCultureIgnoreCase)) 
            ?? [];

        private string? continent;
        private string? subcontinent;
        private string? name;
        private string? country;

        private int index;

        protected override async Task LoadStaticDataAsync()
        {
            await State.StartLoading.Invoke(null);
            AllRegions = await LocalJsonApi.GetAllRegions(Cts.Token);
            await State.FinishLoading.Invoke(AllRegions);
        }

        protected override async Task<bool> LoadInteropDataAsync(Microsoft.JSInterop.IJSRuntime JsRuntime)
        {
            country = await AppStateStatic.GetCountry(IpInfoApi, JsRuntime, Cts.Token);
            return true;
        }

        private bool HideFlag(string? code)
        {
            if ((country.Empty() || string.Equals(country, "cn", StringComparison.OrdinalIgnoreCase)) && string.Equals(code, "tw", StringComparison.OrdinalIgnoreCase))
            {
                return true; //hide Taiwan flag for users from China or when country is unknown
            }

            return false;
        }
    }
}