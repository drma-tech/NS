using Microsoft.AspNetCore.Components;
using MudBlazor;
using NS.Shared.Models.Country;

namespace NS.WEB.Modules.Profile
{
    public partial class TravelHistoryPopup
    {
        [CascadingParameter] private IMudDialogInstance? MudDialog { get; set; }

        [Parameter] public AllRegions? AllRegions { get; set; }

        [Parameter] public TravelHistory? TravelHistory { get; set; }
        [Parameter] public TravelHistoryEntry Entry { get; set; } = new();
        [Parameter] public EventCallback<TravelHistory?> PostSave { get; set; }

        public RegionData? RegionData { get; set; }

        private MudDateRangePicker _picker = default!;
        private DateRange? _dateRange;
        private string? continent;

        private void RegionValueChanged(int? val) => regionTempValue = val;

        private void CityValueChanged(int? val) => cityTempValue = val;

        private int? regionTempValue;
        private int? cityTempValue;

        private static string? LabelText(int? temp, int? selected) => (temp ?? selected) switch
        {
            1 => "Terrible",
            2 => "Very Bad",
            3 => "Bad",
            4 => "Poor",
            5 => "Sufficient",
            6 => "Okay",
            7 => "Good",
            8 => "Very Good",
            9 => "Excellent",
            10 => "Perfect",
            _ => "Rate your experience",
        };

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _dateRange = new DateRange(Entry.StartDate.ToDateTime(TimeOnly.MinValue), Entry.EndDate.ToDateTime(TimeOnly.MinValue));

            if (Entry.RegionCode.NotEmpty())
            {
                continent = AllRegions?.GetByCode(Entry.RegionCode)?.continent;
                await LoadRegion();
            }
        }

        private async Task LoadRegion()
        {
            RegionData = await RegionsApi.GetRegion(Entry.RegionCode, [], Cts.Token) ?? new(Entry.RegionCode);
        }

        private void ContinentChanged()
        {
            Entry.RegionCode = null;
            Entry.CityCode = null;
            Entry.RegionRating = null;
            Entry.CityRating = null;
        }

        private async Task RegionChanged()
        {
            if (Entry.RegionCode.Empty()) { Entry.RegionRating = null; }
            await LoadRegion();
        }

        private void CityChanged()
        {
            if (Entry.CityCode.Empty()) { Entry.CityRating = null; }
        }

        private async Task Save()
        {
            try
            {
                TravelHistory? model = null;

                Entry.StartDate = DateOnly.FromDateTime(_dateRange!.Start!.Value);
                Entry.EndDate = DateOnly.FromDateTime(_dateRange!.End!.Value);

                Entry.RegionName = AllRegions?.GetByCode(Entry.RegionCode)?.name;
                Entry.CityName = RegionData?.Cities.FirstOrDefault(p => string.Equals(p.ToSlug(), Entry.CityCode, StringComparison.OrdinalIgnoreCase));

                var isNew = Entry.Id.Empty();
                if (isNew)
                {
                    Entry.Id = Guid.NewGuid().ToString();
                    model = await TravelHistoryApi.Add(TravelHistory, Entry, AppStateStatic.ActiveProduct, Cts.Token);
                }
                else
                {
                    model = await TravelHistoryApi.Update(Entry, Cts.Token);
                }

                MudDialog?.Close();
                await PostSave.InvokeAsync(model);
            }
            catch (Exception ex)
            {
                await ProcessException(ex);
            }
        }

        private static Color GetRatingColor(int? value)
        {
            if (value == null || value == 0) return Color.Default;

            if (value <= 4) return Color.Error;
            if (value <= 6) return Color.Warning;
            return Color.Success;
        }
    }
}