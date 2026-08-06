using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace NS.WEB.Modules.Profile.Components
{
    public partial class NextDestinationsComponent
    {
        [Parameter, EditorRequired] public string? Culture { get; set; }
        [Parameter] public AllRegions? AllRegions { get; set; }
        [Parameter] public RenderControlState<NextDestinations> Actions { get; set; } = new(obj => obj == null || obj.Items.Empty());

        public NextDestinations? NextDestinations { get; set; }
        public static readonly EventCallbackFactory Factory = new();

        protected override void OnInitialized()
        {
            base.OnInitialized();

            NextDestinationsApi.DataChanged += (data) =>
            {
                NextDestinations = data;
                Actions.CurrentInstance = data;
                _ = Actions.FinishLoading.Invoke(data);
                StateHasChanged();
            };
        }

        protected override async Task LoadAuthenticatedDataAsync(CancellationToken token)
        {
            NextDestinations = await NextDestinationsApi.Get(Actions, token);
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

                var parameters = new DialogParameters<NextDestinationsPopup>
                {
                    { x => x.AllRegions, AllRegions },
                    { x => x.NextDestinations, NextDestinations },
                };

                await DialogService.ShowAsync<NextDestinationsPopup>("Next Destinations", parameters, PopupHelper.Options(MaxWidth.Small));
            }
            catch (Exception ex)
            {
                await ProcessException(ex);
            }
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
                };

                await DialogService.ShowAsync<NextDestinationsPopup>("Next Destinations", parameters, PopupHelper.Options(MaxWidth.Small));
            }
            catch (Exception ex)
            {
                await ProcessException(ex);
            }
        }

        private async Task Delete(NextDestinationsEntry? entry, bool confirm = true)
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
                    NextDestinations = await NextDestinationsApi.Remove(entry?.Id, Cts.Token);
                }
            }
            catch (Exception ex)
            {
                await ProcessException(ex);
            }
        }

        private async Task FinalizeTravel(NextDestinationsEntry? entry)
        {
            try
            {
                if (!AppStateStatic.IsAuthenticated)
                {
                    await ShowWarning(Translations.Notification.YouMustLogged);
                    return;
                }

                var parameters = new DialogParameters<TravelHistoryPopup>
                {
                    { x => x.AllRegions, AllRegions },
                    { x => x.Entry, new  TravelHistoryEntry { RegionCode = entry?.RegionCode, CityCode = entry?.CityCode, Notes = entry?.Notes } },
                    { x => x.PostSave, Factory.Create(new object(), async (TravelHistory? model) => { await Delete(entry, confirm: false); }) },
                };

                await DialogService.ShowAsync<TravelHistoryPopup>("Travel History", parameters, PopupHelper.Options(MaxWidth.Small));
            }
            catch (Exception ex)
            {
                await ProcessException(ex);
            }
        }
    }
}