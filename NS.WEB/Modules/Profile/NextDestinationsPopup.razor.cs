using Microsoft.AspNetCore.Components;
using MudBlazor;
using NS.Shared.Models.Country;

namespace NS.WEB.Modules.Profile
{
    public partial class NextDestinationsPopup
    {
        [CascadingParameter] private IMudDialogInstance? MudDialog { get; set; }

        [Parameter] public AllRegions? AllRegions { get; set; }

        [Parameter] public NextDestinations? NextDestinations { get; set; }
        [Parameter] public NextDestinationsEntry Entry { get; set; } = new();
        [Parameter] public EventCallback<NextDestinations?> PostSave { get; set; }

        public RegionData? RegionData { get; set; }

        private MudDateRangePicker _picker = default!;
        private DateRange? _dateRange;
        private string? continent;
        private string? newItemCheckList;

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

        private async Task Save()
        {
            try
            {
                NextDestinations? model = null;

                Entry.RegionName = AllRegions?.GetByCode(Entry.RegionCode)?.name;
                Entry.CityName = RegionData?.Cities.FirstOrDefault(p => string.Equals(p.ToSlug(), Entry.CityCode, StringComparison.OrdinalIgnoreCase));

                var isNew = Entry.Id.Empty();
                if (isNew)
                {
                    Entry.Id = Guid.NewGuid().ToString();
                    model = await NextDestinationsApi.Add(NextDestinations, Entry, AppStateStatic.ActiveProduct, Cts.Token);
                }
                else
                {
                    model = await NextDestinationsApi.Update(Entry, Cts.Token);
                }

                MudDialog?.Close();
                await PostSave.InvokeAsync(model);
            }
            catch (Exception ex)
            {
                await ProcessException(ex);
            }
        }
    }
}