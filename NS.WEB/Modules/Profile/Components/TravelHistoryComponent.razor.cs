using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace NS.WEB.Modules.Profile.Components
{
    public partial class TravelHistoryComponent
    {
        [Parameter, EditorRequired] public string? Culture { get; set; }
        [Parameter] public AllRegions? AllRegions { get; set; }
        [Parameter] public RenderControlState<TravelHistory> Actions { get; set; } = new(obj => obj == null || obj.Items.Empty());

        public TravelHistory? TravelHistory { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            TravelHistoryApi.DataChanged += (data) =>
            {
                TravelHistory = data;
                Actions.CurrentInstance = data;
                _ = Actions.FinishLoading.Invoke(data);
                StateHasChanged();
            };
        }

        protected override async Task LoadAuthenticatedDataAsync(CancellationToken token)
        {
            TravelHistory = await TravelHistoryApi.Get(Actions, token);
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

                var parameters = new DialogParameters<TravelHistoryPopup>
                {
                    { x => x.AllRegions, AllRegions },
                    { x => x.TravelHistory, TravelHistory },
                };

                await DialogService.ShowAsync<TravelHistoryPopup>("Travel History", parameters, PopupHelper.Options(MaxWidth.Small));
            }
            catch (Exception ex)
            {
                await ProcessException(ex);
            }
        }

        private async Task Update(TravelHistoryEntry? entry)
        {
            try
            {
                var parameters = new DialogParameters<TravelHistoryPopup>
                {
                    { x => x.AllRegions, AllRegions },
                    { x => x.TravelHistory, TravelHistory },
                    { x => x.Entry, entry?.DeepClone() },
                };

                await DialogService.ShowAsync<TravelHistoryPopup>("Travel History", parameters, PopupHelper.Options(MaxWidth.Small));
            }
            catch (Exception ex)
            {
                await ProcessException(ex);
            }
        }

        private async Task Delete(TravelHistoryEntry? entry)
        {
            try
            {
                if (await DialogService.ShowMessageBoxAsync(AppInfo.Title, Translations.Notification.SureDelete, Translations.Button.Ok, Translations.Button.Cancel) ?? false)
                {
                    TravelHistory = await TravelHistoryApi.Remove(entry?.Id, Cts.Token);
                }
            }
            catch (Exception ex)
            {
                await ProcessException(ex);
            }
        }
    }
}