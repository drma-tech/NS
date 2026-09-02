using Microsoft.AspNetCore.Components;
using MudBlazor;
using NS.WEB.Modules.Auth;
using NS.WEB.Modules.Help;
using NS.WEB.Modules.Subscription;
using NS.WEB.Shared;

namespace NS.WEB.Core.Helper;

public static class PopupHelper
{
    public static readonly EventCallbackFactory Factory = new();

    public static async Task SettingsPopup(this IDialogService service)
    {
        await service.ShowAsync<SettingsPopup>(Translations.Module.Help.Settings, Options(MaxWidth.Small));
    }

    public static async Task SubscriptionPopup(this IDialogService service)
    {
        var parameters = new DialogParameters<SubscriptionPopup> { };

        await service.ShowAsync<SubscriptionPopup>(Translations.Module.Subscription.MySubscription, parameters, Options(MaxWidth.Medium));
    }

    public static async Task OnboardingPopup(this IDialogService service, string culture)
    {
        var parameters = new DialogParameters<OnboardingPopup>
        {
            { x => x.Culture, culture },
        };

        await service.ShowAsync<OnboardingPopup>(Translations.Module.Help.WelcomeTo.CustomFormat(AppInfo.Title), parameters, Options(MaxWidth.Medium));
    }

    public static async Task AskReviewPopup(this IDialogService service)
    {
        await service.ShowAsync<AskReviewPopup>(Translations.Module.Help.WriteReviewTitle.CustomFormat(AppInfo.Title), Options(MaxWidth.Small, allowClose: false, showHeader: false));
    }

    public static DialogOptions Options(MaxWidth width, bool allowClose = true, bool showHeader = true)
    {
        return new DialogOptions
        {
            CloseOnEscapeKey = allowClose,
            CloseButton = allowClose,
            BackdropClick = allowClose,
            NoHeader = !showHeader,
            Position = DialogPosition.Center,
            MaxWidth = width,
        };
    }
}
