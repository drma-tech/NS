using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace NS.WEB.Modules.Profile.Components
{
    public partial class WishlistComponent
    {
        [Parameter] public AllRegions? AllRegions { get; set; }
        [Parameter] public RenderControlState<WishList> Actions { get; set; } = new(obj => obj == null || obj.Items.Empty());

        public WishList? WishList { get; set; }
        public static readonly EventCallbackFactory Factory = new();

        protected override void OnInitialized()
        {
            base.OnInitialized();

            WishListApi.DataChanged += (data) =>
            {
                WishList = data;
                Actions.CurrentInstance = data;
                _ = Actions.FinishLoading.Invoke(data);
                StateHasChanged();
            };
        }

        protected override async Task LoadAuthenticatedDataAsync(CancellationToken token)
        {
            Console.WriteLine($"{this} - LoadAuthDataAsync ({AppStateStatic.IsAuthenticated})");
            WishList = await WishListApi.Get(Actions, token);
        }

        private async Task Add()
        {
            try
            {
                if (!AppStateStatic.IsAuthenticated)
                {
                    await ShowWarning(Translations.Notification.YouMustLogged);
                    return;
                }

                var parameters = new DialogParameters<WishlistPopup>
                {
                    { x => x.AllRegions, AllRegions },
                    { x => x.WishList, WishList },
                };

                await DialogService.ShowAsync<WishlistPopup>("Wishlist", parameters, PopupHelper.Options(MaxWidth.Small));
            }
            catch (Exception ex)
            {
                await ProcessException(ex);
            }
        }

        private async Task Update(WishListEntry? entry)
        {
            try
            {
                var parameters = new DialogParameters<WishlistPopup>
                {
                    { x => x.AllRegions, AllRegions },
                    { x => x.WishList, WishList },
                    { x => x.Entry, entry?.DeepClone() },
                };

                await DialogService.ShowAsync<WishlistPopup>("Wishlist", parameters, PopupHelper.Options(MaxWidth.Small));
            }
            catch (Exception ex)
            {
                await ProcessException(ex);
            }
        }

        private async Task Delete(WishListEntry? entry, bool confirm = true)
        {
            try
            {
                var confirmed = false;

                if (confirm)
                    confirmed = await DialogService.ShowMessageBoxAsync(AppInfo.Title, Translations.Notification.SureDelete, Translations.Button.Ok, Translations.Button.Cancel) ?? false;
                else
                    confirmed = true;

                if (confirmed)
                {
                    WishList = await WishListApi.Remove(entry?.Id, Cts.Token);
                }
            }
            catch (Exception ex)
            {
                await ProcessException(ex);
            }
        }

        private async Task ConfirmNextDestination(WishListEntry? entry)
        {
            try
            {
                if (!AppStateStatic.IsAuthenticated)
                {
                    await ShowWarning(Translations.Notification.YouMustLogged);
                    return;
                }

                var parameters = new DialogParameters<NextDestinationsPopup>
                {
                    { x => x.AllRegions, AllRegions },
                    { x => x.Entry, new NextDestinationsEntry{ RegionCode = entry?.RegionCode, CityCode = entry?.CityCode, CheckList = entry?.CheckList ?? new HashSet<CheckListItem>() } },
                    { x => x.PostSave, Factory.Create(new object(), async (NextDestinations? model) => { await Delete(entry, confirm: false); }) },
                };

                await DialogService.ShowAsync<NextDestinationsPopup>("Next Destinations", parameters, PopupHelper.Options(MaxWidth.Small));
            }
            catch (Exception ex)
            {
                await ProcessException(ex);
            }
        }
    }
}