using Microsoft.AspNetCore.Components;
using MudBlazor;
using NS.WEB.Modules.Profile.Components;

namespace NS.WEB.Modules.Profile
{
    public partial class NextDestinationsPage
    {
        private AllRegions? AllRegions { get; set; }
        private NextDestinations? NextDestinations { get; set; }
        public static readonly EventCallbackFactory Factory = new();

        protected override async Task LoadAuthenticatedDataAsync(CancellationToken token)
        {
            if (AllRegions != null) return;

            AllRegions = await LocalJsonApi.GetAllRegions(token);
            NextDestinations = await NextDestinationsApi.Get(actions: null, token);
        }

        private async Task Update(NextDestinationsEntry? entry)
        {
            try
            {
                var parameters = new DialogParameters<NextDestinationsPopup>
                {
                    { x => x.AllRegions, AllRegions },
                    { x => x.NextDestinations, NextDestinations },
                    { x => x.Entry, entry?.DeepClone() },
                    { x => x.PostSave, Factory.Create(new object(), async (NextDestinations? model) => { NextDestinations = model; StateHasChanged(); }) },
                };

                await DialogService.ShowAsync<NextDestinationsPopup>("Next Destinations", parameters, PopupHelper.Options(MaxWidth.Small));
            }
            catch (Exception ex)
            {
                await ProcessException(ex);
            }
        }

        private async Task Delete(NextDestinationsEntry? entry)
        {
            try
            {
                if (await DialogService.ShowMessageBoxAsync(AppInfo.Title, Translations.Notification.SureDelete, Translations.Button.Ok, Translations.Button.Cancel) ?? false)
                {
                    NextDestinations = await NextDestinationsApi.Remove(entry?.Id, Cts.Token);
                }
            }
            catch (Exception ex)
            {
                await ProcessException(ex);
            }
        }
    }
}