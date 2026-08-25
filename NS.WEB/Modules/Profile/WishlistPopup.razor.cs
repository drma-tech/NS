using Microsoft.AspNetCore.Components;
using MudBlazor;
using NS.Shared.Models.Country;

namespace NS.WEB.Modules.Profile
{
    public partial class WishlistPopup
    {
        [CascadingParameter] private IMudDialogInstance? MudDialog { get; set; }

        [Parameter] public AllRegions? AllRegions { get; set; }

        [Parameter] public WishList? WishList { get; set; }
        [Parameter] public WishListEntry Entry { get; set; } = new();

        public RegionData? RegionData { get; set; }

        private string? continent;
        private string? newItemCheckList;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            if (Entry.RegionCode.NotEmpty())
            {
                continent = AllRegions?.GetByCode(Entry.RegionCode)?.continent;
                await LoadRegion();
            }
        }

        private async Task LoadRegion()
        {
            RegionData = await RegionsApi.GetRegion(Entry.RegionCode, Cts.Token) ?? new(Entry.RegionCode);
        }

        private async Task Save()
        {
            try
            {
                Entry.RegionName = AllRegions?.GetByCode(Entry.RegionCode)?.name;
                Entry.CityName = RegionData?.Cities.FirstOrDefault(p => string.Equals(p.ToSlug(), Entry.CityCode, StringComparison.OrdinalIgnoreCase));

                var isNew = Entry.Id.Empty();
                if (isNew)
                {
                    Entry.Id = Guid.NewGuid().ToString();
                    await WishListApi.Add(WishList, Entry, AppStateStatic.ActiveProduct, Cts.Token);
                }
                else
                {
                    await WishListApi.Update(Entry, Cts.Token);
                }

                MudDialog?.Close();
                await ShowSuccess(Translations.Notification.OperationCompleted);
            }
            catch (Exception ex)
            {
                await ProcessException(ex);
            }
        }
    }
}