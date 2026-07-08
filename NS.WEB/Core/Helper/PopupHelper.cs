using Microsoft.AspNetCore.Components;
using MudBlazor;
using NS.WEB.Modules.Auth;
using NS.WEB.Modules.Help;
using NS.WEB.Modules.Subscription.Components;
using NS.WEB.Shared;

namespace NS.WEB.Core.Helper;

public static class PopupHelper
{
    public static readonly EventCallbackFactory Factory = new();

    public static async Task AccountPopup(this IDialogService service)
    {
        var parameters = new DialogParameters<AccountPopup> { };

        await service.ShowAsync<AccountPopup>(Translations.Module.Auth.MyAccount, parameters, Options(MaxWidth.Small));
    }

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
        var parameters = new DialogParameters<Onboarding>
        {
            { x => x.Culture, culture },
        };

        await service.ShowAsync<Onboarding>(string.Format(Translations.Module.Help.WelcomeTo, AppInfo.Title), parameters, Options(MaxWidth.Medium));
    }

    public static async Task AskReviewPopup(this IDialogService service)
    {
        await service.ShowAsync<AskReview>(string.Format(Translations.Module.Help.WriteReviewTitle, AppInfo.Title), Options(MaxWidth.Small, false, false));
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
            MaxWidth = width
        };
    }
}
