using Shooter.Calendar.Core.Attributes;
using Shooter.Calendar.Core.Dialog;

namespace Shooter.Calendar.Core.Common.Extensions.DialogExtensions
{
    public static class ButtonExtensions
    {
        public static bool IsPositive([NotNull] this IButton button)
            => button.ButtonType == ButtonType.Positive;

        public static bool IsNegative([NotNull] this IButton button)
            => button.ButtonType == ButtonType.Negative;
    }
}
