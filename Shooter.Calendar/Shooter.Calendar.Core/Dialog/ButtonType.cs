using Shooter.Calendar.Core.Common;

namespace Shooter.Calendar.Core.Dialog
{
    public class ButtonType : ConfigurationEnum
    {
        public const string PositiveButton = nameof(PositiveButton);
        public const string NegativeButton = nameof(NegativeButton);
        public const string NeutralButton = nameof(NeutralButton);
        public const string DestructiveButton = nameof(DestructiveButton);

        public static readonly ButtonType Positive = new ButtonType(PositiveButton);
        public static readonly ButtonType Negative = new ButtonType(NegativeButton);
        public static readonly ButtonType Neutral = new ButtonType(NeutralButton);
        public static readonly ButtonType Destructive = new ButtonType(DestructiveButton);

        public ButtonType(string value)
            : base(value)
        {
        }
    }
}
