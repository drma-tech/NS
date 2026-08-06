using Microsoft.AspNetCore.Components;
using MudBlazor;
using NS.WEB.Modules.Profile.Components;

namespace NS.WEB.Modules.Profile
{
    public partial class TravelHistoryPage
    {
        private IEnumerable<string?> Continents => AllRegions?.GetContinents() ?? [];
        private AllRegions? AllRegions { get; set; }
        private TravelHistory? TravelHistory { get; set; }

        private Dictionary<string, int> TotRegions = [];
        private readonly Dictionary<string, int> TotVisited = [];
        private readonly Dictionary<string, TimeSpan> TotTime = [];
        public static readonly EventCallbackFactory Factory = new();

        protected override async Task LoadAuthenticatedDataAsync(CancellationToken token)
        {
            if (AllRegions != null) return;

            AllRegions = await LocalJsonApi.GetAllRegions(token);
            TravelHistory = await TravelHistoryApi.Get(actions: null, token);

            TotRegions = AllRegions?.Items.GroupBy(p => p.continent, StringComparer.OrdinalIgnoreCase).ToDictionary(p => p.Key!, p => p.Count(), StringComparer.OrdinalIgnoreCase) ?? [];

            foreach (var key in TotRegions.Select(p => p.Key))
            {
                TotVisited.Add(key, 0);
                TotTime.Add(key, TimeSpan.Zero);
            }

            foreach (var code in TravelHistory?.Items.Select(p => p.RegionCode).Distinct(StringComparer.OrdinalIgnoreCase) ?? [])
            {
                var continent = AllRegions?.GetByCode(code)?.continent;
                if (continent != null)
                {
                    TotVisited[continent]++;
                }
            }

            foreach (var entry in TravelHistory?.Items ?? new HashSet<TravelHistoryEntry>())
            {
                var region = AllRegions?.GetByCode(entry.RegionCode);
                if (region != null)
                {
                    var duration = entry.EndDate.ToDateTime(TimeOnly.MinValue) - entry.StartDate.ToDateTime(TimeOnly.MinValue);
                    TotTime[region.continent!] += duration;
                }
            }
        }

        private static Color GetRatingColor(int? value)
        {
            if (value == null || value == 0) return Color.Dark;

            if (value <= 4) return Color.Error;
            if (value <= 6) return Color.Warning;
            return Color.Success;
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
                    { x => x.PostSave, Factory.Create(new object(), async (TravelHistory? model) => { TravelHistory = model; StateHasChanged(); }) },
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