namespace NS.WEB.Modules.Profile
{
    public partial class ProfilePage
    {
        private AllRegions? AllRegions { get; set; }

        private readonly RenderControlState<TravelHistory> TravelHistoryActions = new(obj => obj == null || obj.Items.Empty());
        private readonly RenderControlState<WishList> WishListActions = new(obj => obj == null || obj.Items.Empty());
        private readonly RenderControlState<NextDestinations> NextDestinationsActions = new(obj => obj == null || obj.Items.Empty());

        private TravelHistory? TravelHistory { get; set; }
        private WishList? WishList { get; set; }
        private NextDestinations? NextDestinations { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            TravelHistoryActions.FinishLoading += async (data) => { TravelHistory = data; StateHasChanged(); };
            WishListActions.FinishLoading += async (data) => { WishList = data; StateHasChanged(); };
            NextDestinationsActions.FinishLoading += async (data) => { NextDestinations = data; StateHasChanged(); };
        }

        protected override async Task LoadStaticDataAsync()
        {
            AllRegions = await LocalJsonApi.GetAllRegions(Cts.Token);
        }
    }
}