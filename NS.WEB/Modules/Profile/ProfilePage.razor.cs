namespace NS.WEB.Modules.Profile
{
    public partial class ProfilePage
    {
        private AllRegions? AllRegions { get; set; }

        private readonly RenderControlState<TravelHistory?> TravelHistoryState = new(null, obj => obj == null || obj.Items.Empty());
        private readonly RenderControlState<WishList?> WishListState = new(null, obj => obj == null || obj.Items.Empty());
        private readonly RenderControlState<NextDestinations?> NextDestinationsState = new(null, obj => obj == null || obj.Items.Empty());

        private TravelHistory? TravelHistory { get; set; }
        private WishList? WishList { get; set; }
        private NextDestinations? NextDestinations { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            TravelHistoryState.FinishLoading += async (data) => { TravelHistory = data; StateHasChanged(); };
            WishListState.FinishLoading += async (data) => { WishList = data; StateHasChanged(); };
            NextDestinationsState.FinishLoading += async (data) => { NextDestinations = data; StateHasChanged(); };
        }

        protected override async Task LoadStaticDataAsync()
        {
            AllRegions = await AllRegionsApi.GetAllRegions(Cts.Token);
        }
    }
}