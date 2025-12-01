using Microsoft.AspNetCore.Components;
using MudBlazor;
using NS.WEB.Modules.Auth;
using NS.WEB.Modules.Profile.Resources;
using NS.WEB.Modules.Subscription.Components;
using NS.WEB.Modules.Support;
using NS.WEB.Shared;

namespace NS.WEB.Core.Helper;

public static class PopupHelper
{
    public static readonly EventCallbackFactory Factory = new();

    public static async Task OpenAccountPopup(this IDialogService service)
    {
        var parameters = new DialogParameters<ProfilePopup> { };

        await service.ShowAsync<ProfilePopup>(Translations.MyProfile, parameters, Options(MaxWidth.ExtraSmall));
    }

    public static async Task SettingsPopup(this IDialogService service)
    {
        await service.ShowAsync<SettingsPopup>(GlobalTranslations.Settings, Options(MaxWidth.Small));
    }

    public static async Task SubscriptionPopup(this IDialogService service)
    {
        var parameters = new DialogParameters<SubscriptionPopup> { };

        await service.ShowAsync<SubscriptionPopup>(Modules.Subscription.Resources.Translations.MySubscription, parameters, Options(MaxWidth.Medium));
    }

    public static async Task OnboardingPopup(this IDialogService service)
    {
        await service.ShowAsync<Onboarding>(string.Format(GlobalTranslations.WelcomeTo, SeoTranslations.AppName), Options(MaxWidth.Medium));
    }

    public static async Task AskReviewPopup(this IDialogService service)
    {
        await service.ShowAsync<AskReview>(string.Format("Want to help {0} grow?", SeoTranslations.AppName), Options(MaxWidth.Small));
    }

    public static async Task LoginPopup(this IDialogService service)
    {
        await service.ShowAsync<LoginPopup>("Log in or sign up", Options(MaxWidth.ExtraSmall));
    }

    public static DialogOptions Options(MaxWidth width)
    {
        return new DialogOptions
        {
            CloseOnEscapeKey = true,
            CloseButton = true,
            Position = DialogPosition.Center,
            MaxWidth = width
        };
    }
}
