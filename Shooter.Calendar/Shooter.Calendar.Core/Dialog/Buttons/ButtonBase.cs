using Shooter.Calendar.Core.Attributes;

namespace Shooter.Calendar.Core.Dialog.Buttons
{
    public abstract class ButtonBase : IButton
    {
        protected ButtonBase([NotNull] ButtonType buttonType, string title)
        {
            ButtonType = buttonType;
            Title = title;
        }

        public ButtonType ButtonType { get; }

        public string Title { get; }
    }
}
