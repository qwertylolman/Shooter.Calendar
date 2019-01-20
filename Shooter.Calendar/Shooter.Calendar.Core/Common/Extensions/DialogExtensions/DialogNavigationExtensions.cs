using System.Threading.Tasks;
using MvvmCross.Navigation;
using Shooter.Calendar.Core.Attributes;
using Shooter.Calendar.Core.Common.Extensions.DialogExtensions;
using Shooter.Calendar.Core.Dialog;
using Shooter.Calendar.Core.Dialog.Buttons;
using Shooter.Calendar.Core.Localization;

namespace Shooter.Calendar.Core.Common.Extensions
{
    public static class DialogNavigationExtensions
    {
        public static async Task<bool> ShowAreYouSureDialog([NotNull] this IMvxNavigationService navigationService)
        {
            var result =
                await ShowOkCancelDialogAsync(
                    navigationService,
                    LocalizationExtensions.Get("are_you_sure_dialog_title"),
                    LocalizationExtensions.Get("are_you_sure_dialog_text"));

            return result.IsPositive();
        }

        public static Task<IButton> ShowOkCancelDialogAsync(
            [NotNull] this IMvxNavigationService navigationService,
            string title,
            string text)
            => ShowDialogAsync(navigationService, title, text, new OkButton(), new CancelButton());

        public static Task<IButton> ShowDialogAsync(
            [NotNull] this IMvxNavigationService navigationService,
            string title,
            string text,
            params IButton[] buttons)
        {
            if (buttons.Length == 0)
            {
                buttons = new IButton[] { new OkButton() };
            }

            return Navigate(navigationService, DialogParameterArgs.Build(title, text, buttons));
        }

        private static Task<IButton> Navigate(IMvxNavigationService navigationService, DialogParameterArgs args)
            => navigationService.Navigate<DialogViewModel, DialogParameterArgs, IButton>(args);
    }
}
