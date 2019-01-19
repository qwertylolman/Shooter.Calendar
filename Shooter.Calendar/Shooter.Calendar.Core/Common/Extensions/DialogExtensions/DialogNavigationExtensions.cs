using MvvmCross.Navigation;
using System.Threading.Tasks;
using Shooter.Calendar.Core.Attributes;
using Shooter.Calendar.Core.Dialog;
using Shooter.Calendar.Core.Dialog.Buttons;

namespace Shooter.Calendar.Core.Common.Extensions
{
    public static class DialogNavigationExtensions
    {
        public static Task<IButton> ShowOkCancelDialogAsync(
            [NotNull] IMvxNavigationService navigationService,
            string title,
            string text)
            => ShowDialogAsync(navigationService, title, text, new OkButton(), new CancelButton());

        public static Task<IButton> ShowDialogAsync(
            [NotNull] IMvxNavigationService navigationService,
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
